using Microsoft.EntityFrameworkCore;
using HyphenProject.Entities.Models;

namespace HyphenProject.DataAccess.Concrete
{
    public class HyphenProjectDbContext : DbContext
    {
        public HyphenProjectDbContext()
        {
        }

        public HyphenProjectDbContext(DbContextOptions<HyphenProjectDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Password=Sunday24;Persist Security Info=True;User ID=sa;Initial Catalog=TestSolution;Data Source=.");
        }

        public DbSet<Product> Product { get; set; }
      }
}