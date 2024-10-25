using System;
using System.Collections.ObjectModel;

namespace CookBook.RecipeManager.GUI.Models
{
    public class CustomRecipe
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public ObservableCollection<Ingredient> Ingredients { get; set; } = new ObservableCollection<Ingredient>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
    }
}
