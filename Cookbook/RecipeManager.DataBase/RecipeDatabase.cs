using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.DataBase
{
    public class RecipeDatabase
    {
        // File path for saving and loading recipes
        private readonly string _filePath;

        // Constructor that initializes the file path
        public RecipeDatabase(string filePath)
        {
            _filePath = filePath;
        }

        // Loads recipes from the file, returns an empty list if the file doesn't exist or an error occurs
        public List<CustomRecipe> LoadRecipes()
        {
            if (!File.Exists(_filePath))
            {
                return new List<CustomRecipe>(); // Return empty list if file doesn't exist
            }

            var json = File.ReadAllText(_filePath); // Read file content as JSON
            return JsonSerializer.Deserialize<List<CustomRecipe>>(json) ?? new List<CustomRecipe>(); // Deserialize JSON to list of recipes
        }

        // Saves the list of recipes to the file in JSON format
        public void SaveRecipes(List<CustomRecipe> recipes)
        {
            var json = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true }); // Serialize list to JSON with indentation
            File.WriteAllText(_filePath, json); // Write JSON to file
        }

        // Adds a new recipe to the existing list and saves it to the file
        public void AddRecipe(CustomRecipe recipe)
        {
            var recipes = LoadRecipes(); // Load existing recipes
            recipes.Add(recipe); // Add new recipe to list
            SaveRecipes(recipes); // Save updated list back to file
        }
    }
}
