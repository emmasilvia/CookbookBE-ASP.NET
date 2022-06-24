using System;
using System.Linq;
using CookbookBackend.Controllers.User;
using CookbookBackend.DataLayer;
using CookbookBackEnd.Controllers.Recipe;
using Microsoft.AspNetCore.Mvc;

namespace CookbookBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationContext _db;

        public object TempData { get; private set; }

        public RecipeController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet("AllRecipes")]
        public ActionResult<GetAllUsersResponse> GetAllRecipes(int pageSize, int pageNumber, Enums.SortType sortType)
        {
            var allRecipesQuery = _db.Recipes.AsQueryable();

            switch (sortType)
            {
                case Enums.SortType.NameAscending:
                    allRecipesQuery = allRecipesQuery.OrderBy(x => x.Name);
                    break;
                case Enums.SortType.NameDescending:
                    allRecipesQuery = allRecipesQuery.OrderByDescending(x => x.Name);
                    break;
                case Enums.SortType.CookingTimeAscending:
                    allRecipesQuery = allRecipesQuery.OrderBy(x => x.CookingTime);
                    break;
                case Enums.SortType.CookingTimeDescending:
                    allRecipesQuery = allRecipesQuery.OrderByDescending(x => x.CookingTime);
                    break;
                default: throw new ApplicationException("Unknown sort type");
            }

            var allRecipes = allRecipesQuery
                .Select(r => new RecipeDTO
                {
                    Name = r.Name + ' ' + r.Name,
                    Ingredients = r.Ingredients,
                    CookingTime = r.CookingTime
                })
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return Ok(new GetAllRecipesResponse
            {
                Recipes = allRecipes,
            });
        }
    }
}
