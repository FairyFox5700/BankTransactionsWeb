using BankTransactionWeb.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.DAL.EfCore
{
   public class BankTransactionContext :DbContext
    {
        private readonly ILogger<BankTransactionContext> logger;

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Shareholder> Shareholders { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BankTransactionContext(DbContextOptions<BankTransactionContext> options, ILogger<BankTransactionContext> logger)
            : base(options)
        {
            this.logger = logger;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Transaction>()
            .HasOne<Account>(t => t.AccountSource)
            .WithMany(a => a.TransactionsForSource);
            modelBuilder.Entity<Transaction>()
            .HasOne<Account>(t => t.AccountDestination)
            .WithMany(a => a.TransactionsForDestination);
            base.OnModelCreating(modelBuilder);
            try
            {
                modelBuilder.Seed();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Database.");
            }
        }
    }
}
