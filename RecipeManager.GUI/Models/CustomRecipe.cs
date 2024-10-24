namespace CookBook.RecipeManager.GUI.Models
{
    public class CustomRecipe
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new List<string>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
    }
}