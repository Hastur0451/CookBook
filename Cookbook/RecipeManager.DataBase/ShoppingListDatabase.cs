using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.DataBase
{
    public class ShoppingListDatabase
    {
        // File path for saving and loading the shopping list
        private readonly string _filePath;

        // Constructor that initializes the file path
        public ShoppingListDatabase(string filePath)
        {
            _filePath = filePath;
        }

        // Loads the shopping list from the file, returns an empty list if file doesn't exist or an error occurs
        public List<ShoppingListItem> LoadShoppingList()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<ShoppingListItem>(); // Return empty list if file doesn't exist
                }

                var json = File.ReadAllText(_filePath); // Read file content as JSON
                return JsonSerializer.Deserialize<List<ShoppingListItem>>(json) ?? new List<ShoppingListItem>(); // Deserialize JSON to list of shopping items
            }
            catch (JsonException)
            {
                // Handle JSON format errors by returning an empty list
                return new List<ShoppingListItem>();
            }
            catch (Exception)
            {
                // Handle other exceptions by returning an empty list
                return new List<ShoppingListItem>();
            }
        }

        // Saves the shopping list to the file in JSON format
        public void SaveShoppingList(List<ShoppingListItem> shoppingList)
        {
            var json = JsonSerializer.Serialize(shoppingList, new JsonSerializerOptions { WriteIndented = true }); // Serialize list to JSON with indentation
            File.WriteAllText(_filePath, json); // Write JSON to file
        }
    }
}
