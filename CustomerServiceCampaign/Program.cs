using CustomerServiceCampaign;
using CustomerServiceCampaign.API.ErrorLogging;
using CustomerServiceCampaign.API.Jwt;
using CustomerServiceCampaign.API.Jwt.TokenStorage;
using CustomerServiceCampaign.API.Middleware;
using CustomerServiceCampaign.Application.Actor;
using CustomerServiceCampaign.Application.Logging;
using CustomerServiceCampaign.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NuGet.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddSingleton(appSettings.Jwt);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<CustomerServiceCampaignContext>(e =>
{
    var options = new DbContextOptionsBuilder<CustomerServiceCampaignContext>()
        .UseSqlServer(appSettings.ConnectionString)
        .Options;
    return new CustomerServiceCampaignContext(options);
});

builder.Services.AddTransient<JwtManager>();
builder.Services.AddTransient<ITokenStorage, InMemoryTokenStorage>();
builder.Services.AddTransient<IErrorLogger, EfErrorLogger>();

builder.Services.AddTransient<IApplicationActor>(e =>
{
    var accessor = e.GetService<IHttpContextAccessor>();
    if (accessor.HttpContext == null)
    {
        return new UnauthorizedActor();
    }
    return e.GetService<IActorFetch>().Fetch();
});

builder.Services.AddTransient<IActorFetch>(e =>
{
    var accessor = e.GetService<IHttpContextAccessor>();
    var req = accessor.HttpContext.Request;
    var auth = req.Headers.Authorization.ToString();

    return new ActorFetch(auth);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sw =>
{
    sw.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerServiceCampaign API", Version = "v1" });
});

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer =>
{
    bearer.RequireHttpsMetadata = false;
    bearer.SaveToken = true;
    bearer.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var auth = context.Request.Headers["Authorization"].ToString();
            var actorFetch = new ActorFetch(auth);
            var actor = actorFetch.Fetch();

            if (actor is UnauthorizedActor)
            {
                context.Fail("Invalid token");
                return;
            }

            var tokenIdClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;

            if (string.IsNullOrEmpty(tokenIdClaim))
            {
                context.Fail("Token ID claim not found");
                return;
            }

            var tokenStorage = context.HttpContext.RequestServices.GetRequiredService<ITokenStorage>();

            if (!tokenStorage.TokenExists(tokenIdClaim))
            {
                context.Fail("Token is not valid");
            }

            await Task.CompletedTask;
        }
    };
    bearer.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidAudience = "Any",
        ValidateIssuer = true,
        ValidIssuer = appSettings.Jwt.Issuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.SecretKey)),
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
