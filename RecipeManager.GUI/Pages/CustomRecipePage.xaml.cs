using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CookBook.RecipeManager.GUI.Models;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class CustomRecipePage : UserControl
    {
        private List<CustomRecipe> _recipes = new List<CustomRecipe>();
        private CustomRecipe? _currentRecipe;

        public CustomRecipePage()
        {
            InitializeComponent();
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            // TODO: Load recipes from storage
            recipeList.ItemsSource = _recipes;
        }

        private void BtnCustomSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = customRecipeSearch.Text.ToLower();
            var filteredRecipes = _recipes.Where(r =>
                r.Name.ToLower().Contains(searchTerm)).ToList();
            recipeList.ItemsSource = filteredRecipes;
        }

        private void BtnAddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            _currentRecipe = new CustomRecipe();
            ClearInputs();
            ingredientsList.ItemsSource = _currentRecipe.Ingredients;
        }

        private void EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string recipeId)
            {
                _currentRecipe = _recipes.FirstOrDefault(r => r.Id == recipeId);
                if (_currentRecipe != null)
                {
                    txtRecipeName.Text = _currentRecipe.Name;
                    ingredientsList.ItemsSource = _currentRecipe.Ingredients;
                }
            }
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string recipeId)
            {
                var recipe = _recipes.FirstOrDefault(r => r.Id == recipeId);
                if (recipe != null)
                {
                    var result = MessageBox.Show(
                        "Are you sure you want to delete this recipe?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _recipes.Remove(recipe);
                        // TODO: Save changes to storage
                        LoadRecipes();
                    }
                }
            }
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null)
            {
                _currentRecipe.Ingredients.Add(string.Empty);
                RefreshIngredientsList();
            }
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null && sender is Button button)
            {
                var ingredient = button.DataContext as string;
                if (ingredient != null)
                {
                    _currentRecipe.Ingredients.Remove(ingredient);
                    RefreshIngredientsList();
                }
            }
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe == null) return;

            _currentRecipe.Name = txtRecipeName.Text;

            if (string.IsNullOrWhiteSpace(_currentRecipe.Name))
            {
                MessageBox.Show("Please enter a recipe name.");
                return;
            }

            if (!_recipes.Contains(_currentRecipe))
            {
                _recipes.Add(_currentRecipe);
            }

            // TODO: Save changes to storage
            LoadRecipes();
            ClearInputs();
            _currentRecipe = null;
        }

        private void AddToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null)
            {
                // TODO: Implement shopping list functionality
                MessageBox.Show("Added to shopping list!");
            }
        }

        private void ClearInputs()
        {
            txtRecipeName.Text = string.Empty;
            ingredientsList.ItemsSource = null;
        }

        private void RefreshIngredientsList()
        {
            ingredientsList.ItemsSource = null;
            ingredientsList.ItemsSource = _currentRecipe?.Ingredients;
        }
    }
}