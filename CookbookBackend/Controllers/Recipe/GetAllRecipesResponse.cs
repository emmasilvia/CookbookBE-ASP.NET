using System.Collections.Generic;

namespace CookbookBackEnd.Controllers.Recipe
{
    public class GetAllRecipesResponse
    {
        public List<RecipeDTO> Recipes { get; set; }
    }

    public class RecipeDTO
    {
        public string Name { get; set; }

        public string Ingredients { get; set; }

        public int CookingTime { get; set; }

    }
}
