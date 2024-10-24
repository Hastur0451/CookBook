using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RecipeManager.DataBase
{
    public class ShoppingListDatabase
    {
        private readonly string _filePath;

        public ShoppingListDatabase(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> LoadShoppingList()
        {
            if (!File.Exists(_filePath))
            {
                return new List<string>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        public void SaveShoppingList(List<string> shoppingList)
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(shoppingList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
