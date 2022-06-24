using System.Reflection;
using CookbookBackEnd.DataLayer.Entities;
using CookbookBE.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;


namespace CookbookBackend.DataLayer
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions opts) : base(opts)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }


}
