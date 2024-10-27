using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RecipeManager.BusinessLogic;

namespace RecipeManager.DataBase
{
    public class FavoriteRecipeDatabase
    {
        private readonly string _filePath; // File path for storing favorite recipes

        public FavoriteRecipeDatabase(string filePath)
        {
            _filePath = filePath;
        }

        // Loads favorite recipes, returns empty if file is missing
        public List<RecipeSearchResult> LoadFavoriteRecipes()
        {
            if (!File.Exists(_filePath)) return new List<RecipeSearchResult>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<RecipeSearchResult>>(json) ?? new List<RecipeSearchResult>();
        }

        // Saves the list of favorite recipes to the file
        public void SaveFavoriteRecipes(List<RecipeSearchResult> favoriteRecipes)
        {
            var json = JsonSerializer.Serialize(favoriteRecipes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        // Adds a recipe to favorites if it's not already there
        public void AddFavoriteRecipe(RecipeSearchResult recipe)
        {
            var favoriteRecipes = LoadFavoriteRecipes();
            if (!favoriteRecipes.Exists(r => r.Id == recipe.Id))
            {
                favoriteRecipes.Add(recipe);
                SaveFavoriteRecipes(favoriteRecipes);
            }
        }

        // Removes a recipe from favorites by ID
        public void RemoveFavoriteRecipe(string recipeId)
        {
            var favoriteRecipes = LoadFavoriteRecipes();
            favoriteRecipes.RemoveAll(r => r.Id == recipeId);
            SaveFavoriteRecipes(favoriteRecipes);
        }

        // Checks if a recipe is a favorite by ID
        public bool IsFavoriteRecipe(string recipeId)
        {
            var favoriteRecipes = LoadFavoriteRecipes();
            return favoriteRecipes.Exists(r => r.Id == recipeId);
        }
    }
}
