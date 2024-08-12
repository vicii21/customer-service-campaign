using CustomerServiceCampaign.API.DTO;
using CustomerServiceCampaign.API.ErrorLogging;
using CustomerServiceCampaign.API.Jwt;
using CustomerServiceCampaign.API.Jwt.TokenStorage;
using CustomerServiceCampaign.Application.Actor;
using CustomerServiceCampaign.Application.Logging;
using CustomerServiceCampaign.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CustomerServiceCampaign.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);

            services.AddTransient<CustomerServiceCampaignContext>(e =>
            {
                var options = new DbContextOptionsBuilder<CustomerServiceCampaignContext>()
                    .UseSqlServer(appSettings.ConnectionString)
                    .Options;
                return new CustomerServiceCampaignContext(options);
            });

            services.AddTransient<JwtManager>();
            services.AddTransient<ITokenStorage, InMemoryTokenStorage>();
            services.AddTransient<IErrorLogger, EfErrorLogger>();

            services.AddTransient<IApplicationActor>(e =>
            {
                var accessor = e.GetService<IHttpContextAccessor>();
                if (accessor.HttpContext == null)
                {
                    return new UnauthorizedActor();
                }
                return e.GetService<IActorFetch>().Fetch();
            });

            services.AddTransient<IActorFetch>(e =>
            {
                var accessor = e.GetService<IHttpContextAccessor>();
                var req = accessor.HttpContext.Request;
                var auth = req.Headers.Authorization.ToString();

                return new ActorFetch(auth);
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(sw =>
            {
                sw.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerServiceCampaign API", Version = "v1" });
            });

            services.AddAuthentication(auth =>
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


        }
    }
}
