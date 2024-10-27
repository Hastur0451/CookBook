using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.DataBase;

namespace RecipeManager.BusinessLogic
{
    public class RecipeLogic
    {
        private readonly TheMealDBClient _mealDbClient;
        private readonly FavoriteRecipeDatabase _favoriteRecipeDatabase;

        public RecipeLogic(string fatSecretConsumerKey, string fatSecretConsumerSecret)
        {
            _mealDbClient = new TheMealDBClient();
            _favoriteRecipeDatabase = new FavoriteRecipeDatabase("favoriteRecipes.json");
        }

        public async Task<List<RecipeSearchResult>> SearchRecipes(string searchTerm)
        {
            return await _mealDbClient.SearchMeals(searchTerm);
        }

        public async Task<Recipe> GetRecipeDetails(string recipeId)
        {
            return await _mealDbClient.GetRecipeById(recipeId);
        }

        public List<(string Measure, string Ingredient)> GetFormattedIngredients(Recipe recipe)
        {
            var formattedIngredients = new List<(string Measure, string Ingredient)>();

            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(recipe.Ingredients[i]))
                {
                    formattedIngredients.Add((recipe.Measures[i].Trim(), recipe.Ingredients[i].Trim()));
                }
            }

            return formattedIngredients;
        }

        public void AddFavoriteRecipe(RecipeSearchResult recipe)
        {
            _favoriteRecipeDatabase.AddFavoriteRecipe(recipe);
        }

        public void RemoveFavoriteRecipe(string recipeId)
        {
            _favoriteRecipeDatabase.RemoveFavoriteRecipe(recipeId);
        }

        public bool IsFavoriteRecipe(string recipeId)
        {
            return _favoriteRecipeDatabase.IsFavoriteRecipe(recipeId);
        }

        public List<RecipeSearchResult> GetFavoriteRecipes()
        {
            return _favoriteRecipeDatabase.LoadFavoriteRecipes();
        }
    }
}
