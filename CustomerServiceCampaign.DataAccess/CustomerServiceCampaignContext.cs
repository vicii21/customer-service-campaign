using Microsoft.EntityFrameworkCore;
using CustomerServiceCampaign.DataAccess.Configurations;
using CustomerServiceCampaign.Domain.Entities;
using System;

namespace CustomerServiceCampaign.DataAccess
{
    public class CustomerServiceCampaignContext : DbContext
    {
        public CustomerServiceCampaignContext() { }
        public CustomerServiceCampaignContext(DbContextOptions opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerServiceCampaignContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=CustomerServiceCampaign;TrustServerCertificate=True;Integrated security=True").EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
        public DbSet<PersonUseCase> PersonUseCase { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<PersonColor> PersonColor { get; set; }
        public DbSet<Agent> Agent { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<CustomerDiscount> CustomerDiscount { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<LogEntry> LogEntry { get; set; }

    }
}
