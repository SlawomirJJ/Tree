using Microsoft.EntityFrameworkCore;

namespace Tree.Entities
{
    public class TreeDbContext: DbContext
    {
        public DbSet <File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<File>()
                .Property(r => r.Path)
                .IsRequired();

            modelBuilder.Entity<File>()
                .Property(r => r.Format)
                .IsRequired();
        }
    }
}
