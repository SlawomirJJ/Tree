using Microsoft.EntityFrameworkCore;

namespace Tree.Entities
{
    public class TreeDbContext: DbContext
    {
        private string _connectionString = "Server=(localdb)\\MSSQLLocalDB; Database=TreeDb;Trusted_Connection=True;";
        public DbSet <Register> Registers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Register>()
                .Property(r => r.Path)
                .IsRequired();

            modelBuilder.Entity<Register>()
                .Property(r => r.Format)
                .IsRequired()
                .HasMaxLength(15);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
