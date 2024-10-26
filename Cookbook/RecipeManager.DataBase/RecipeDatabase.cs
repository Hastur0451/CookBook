using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.DataBase
{
    public class RecipeDatabase
    {
        private readonly string _filePath;

        public RecipeDatabase(string filePath)
        {
            _filePath = filePath;
        }

        public List<CustomRecipe> LoadRecipes()
        {
            if (!File.Exists(_filePath))
            {
                return new List<CustomRecipe>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<CustomRecipe>>(json) ?? new List<CustomRecipe>();
        }

        public void SaveRecipes(List<CustomRecipe> recipes)
        {
            var json = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void AddRecipe(CustomRecipe recipe)
        {
            var recipes = LoadRecipes();
            recipes.Add(recipe);
            SaveRecipes(recipes);
        }
    }
}
