using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.PrimeApi
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((ctx, _) =>
            {
                if (!ctx.Set<TestUser>().Any())
                {
                    ctx.Set<TestUser>().Add(new TestUser { Name = "admin", Password = "admin" });
                    ctx.Set<TestUser>().Add(new TestUser { Name = "prime", Password = "prime" });
                    ctx.SaveChanges();
                }
            
            });
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<TestUser> Users { get; set; }
    }

    [Table("Users")]
    public class TestUser
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }
}
