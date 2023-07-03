
using Microsoft.EntityFrameworkCore;
using Transactions.Model.Entities;

namespace Transactions.Data.Context
{
    public class TransactionsDbContext :DbContext
    {
        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionUpdateRequest> TransactionUpdateRequests { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Currency);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Network);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionStatus);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionHash);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.FromAddress);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionType);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.CreatedAt);



            modelBuilder.Entity<TransactionUpdateRequest>()
                .HasIndex(t => t.TransactionHash);

            modelBuilder.Entity<TransactionUpdateRequest>()
                .HasIndex(t => t.WalletAddress);

            modelBuilder.Entity<TransactionUpdateRequest>()
                .HasIndex(t => t.ClientId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
