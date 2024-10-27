using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.DataBase;

namespace RecipeManager.BusinessLogic
{
    public class RecipeLogic
    {
        // API client for interacting with TheMealDB
        private readonly TheMealDBClient _mealDbClient;

        // Database for storing and managing favorite recipes
        private readonly FavoriteRecipeDatabase _favoriteRecipeDatabase;

        // Initializes RecipeLogic with API credentials and sets up the database
        public RecipeLogic(string fatSecretConsumerKey, string fatSecretConsumerSecret)
        {
            _mealDbClient = new TheMealDBClient();
            _favoriteRecipeDatabase = new FavoriteRecipeDatabase("favoriteRecipes.json");
        }

        // Searches for recipes based on a search term
        public async Task<List<RecipeSearchResult>> SearchRecipes(string searchTerm)
        {
            return await _mealDbClient.SearchMeals(searchTerm);
        }

        // Retrieves recipe details by recipe ID
        public async Task<Recipe> GetRecipeDetails(string recipeId)
        {
            return await _mealDbClient.GetRecipeById(recipeId);
        }

        // Formats ingredients and measures as a tuple list
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

        // Adds a recipe to the favorites database
        public void AddFavoriteRecipe(RecipeSearchResult recipe)
        {
            _favoriteRecipeDatabase.AddFavoriteRecipe(recipe);
        }

        // Removes a recipe from the favorites database by ID
        public void RemoveFavoriteRecipe(string recipeId)
        {
            _favoriteRecipeDatabase.RemoveFavoriteRecipe(recipeId);
        }

        // Checks if a recipe is marked as favorite by ID
        public bool IsFavoriteRecipe(string recipeId)
        {
            return _favoriteRecipeDatabase.IsFavoriteRecipe(recipeId);
        }

        // Retrieves all favorite recipes from the database
        public List<RecipeSearchResult> GetFavoriteRecipes()
        {
            return _favoriteRecipeDatabase.LoadFavoriteRecipes();
        }
    }
}
