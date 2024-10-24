namespace CookBook.RecipeManager.GUI.Models
{
    public class CustomRecipe
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public NutritionalInfo Nutrition { get; set; } = new NutritionalInfo();
    }

    public class Ingredient
    {
        public string Name { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class NutritionalInfo
    {
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
    }
}
