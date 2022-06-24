using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookbookBackEnd.DataLayer.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public int CookingTime { get; set; }
        public string Description { get; set; }
    }

    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes");
            builder.HasKey(r => r.Id);
            builder.HasIndex(u => u.Name).IsUnique();
            builder.Property(u => u.Ingredients).HasMaxLength(128).IsRequired();
            builder.Property(u => u.CookingTime).IsRequired();
            builder.Property(u => u.Description).HasMaxLength(128).IsRequired();
        }
    }
}
