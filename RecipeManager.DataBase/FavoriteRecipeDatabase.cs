using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RecipeManager.BusinessLogic;

namespace RecipeManager.DataBase
{
    public class FavoriteRecipeDatabase
    {
        private readonly string _filePath;

        public FavoriteRecipeDatabase(string filePath)
        {
            _filePath = filePath;
        }

        public List<RecipeSearchResult> LoadFavoriteRecipes()
        {
            if (!File.Exists(_filePath))
            {
                return new List<RecipeSearchResult>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<RecipeSearchResult>>(json) ?? new List<RecipeSearchResult>();
        }

        public void SaveFavoriteRecipes(List<RecipeSearchResult> favoriteRecipes)
        {
            var json = JsonSerializer.Serialize(favoriteRecipes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void AddFavoriteRecipe(RecipeSearchResult recipe)
        {
            var favoriteRecipes = LoadFavoriteRecipes();
            if (!favoriteRecipes.Exists(r => r.Id == recipe.Id))
            {
                favoriteRecipes.Add(recipe);
                SaveFavoriteRecipes(favoriteRecipes);
            }
        }

        public void RemoveFavoriteRecipe(string recipeId)
        {
            var favoriteRecipes = LoadFavoriteRecipes();
            favoriteRecipes.RemoveAll(r => r.Id == recipeId);
            SaveFavoriteRecipes(favoriteRecipes);
        }

        public bool IsFavoriteRecipe(string recipeId)
        {
            var favoriteRecipes = LoadFavoriteRecipes();
            return favoriteRecipes.Exists(r => r.Id == recipeId);
        }
    }
}
