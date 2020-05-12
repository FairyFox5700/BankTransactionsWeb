using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.DAL.Implementation.Core
{
    public class BankTransactionContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ILogger<BankTransactionContext> logger;

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Shareholder> Shareholders { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public BankTransactionContext(DbContextOptions<BankTransactionContext> options, ILogger<BankTransactionContext> logger)
            : base(options)
        {
            this.logger = logger;
            // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationUser>().HasKey(k => k.Id);
            modelBuilder.Entity<Person>().HasOne(p => p.ApplicationUser).WithOne(a => a.Person).HasForeignKey<Person>(p => p.ApplicationUserFkId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Transaction>()
                .HasOne<Account>(t => t.SourceAccount)
                .WithMany(a => a.Transactions);
            try
            {
                modelBuilder.Seed();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Database.");
            }
            base.OnModelCreating(modelBuilder);

        }

        public Company Company
        {
            get => default;
            set
            {
            }
        }

        public Person Person
        {
            get => default;
            set
            {
            }
        }

        public Shareholder Shareholder
        {
            get => default;
            set
            {
            }
        }

        public Account Account
        {
            get => default;
            set
            {
            }
        }
    }
}
