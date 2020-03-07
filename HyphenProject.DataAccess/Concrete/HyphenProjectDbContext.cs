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
            optionsBuilder.UseSqlServer("Password=Asdasdxx1;Persist Security Info=True;User ID=mustafa.cakiroglu;Initial Catalog=TestSolution;Data Source=192.168.2.43");
        }

        public DbSet<Product> Product { get; set; }
      }
}