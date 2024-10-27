using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.DataBase
{
    public class ShoppingListDatabase
    {
        private readonly string _filePath;

        public ShoppingListDatabase(string filePath)
        {
            _filePath = filePath;
        }

        public List<ShoppingListItem> LoadShoppingList()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<ShoppingListItem>();
                }

                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<ShoppingListItem>>(json) ?? new List<ShoppingListItem>();
            }
            catch (JsonException)
            {
                return new List<ShoppingListItem>();
            }
            catch (Exception)
            {
                return new List<ShoppingListItem>();
            }
        }

        public void SaveShoppingList(List<ShoppingListItem> shoppingList)
        {
            var json = JsonSerializer.Serialize(shoppingList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
