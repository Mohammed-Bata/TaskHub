using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Task_mangement_System.Models;
using Task = Task_mangement_System.Models.Task;

namespace Task_mangement_System.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
            
        }
        public DbSet<ApplicationUser> applicationusers { get; set; }
        public DbSet<Task> tasks { get; set; }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(new Category
            {
                Id = 1, Name = "Work"
            },new Category
            {
                Id = 2, Name = "Personal"
            },new Category
            {
                Id = 3, Name = "Finance"
            }, new Category
            {
                Id = 4,Name = "Health"
            }, new Category
            {
                Id = 5, Name = "Travel"
            });
            builder.Entity<Task>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Task>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        }

    }
}
