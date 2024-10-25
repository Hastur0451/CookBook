using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CookBook.RecipeManager.GUI.Models;
using RecipeManager.DataBase;
using CookBook.RecipeManager.GUI.Windows; // Add this line

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class CustomRecipePage : UserControl
    {
        private ObservableCollection<CustomRecipe> _recipes;
        private CustomRecipe? _currentRecipe;
        private readonly RecipeDatabase _recipeDatabase;

        public CustomRecipePage()
        {
            InitializeComponent();
            _recipeDatabase = new RecipeDatabase("recipes.json");
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            var loadedRecipes = _recipeDatabase.LoadRecipes();
            _recipes = new ObservableCollection<CustomRecipe>(loadedRecipes);
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
            _currentRecipe.Ingredients = new ObservableCollection<Ingredient>();
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
                        _recipeDatabase.SaveRecipes(_recipes.ToList());
                        LoadRecipes();
                    }
                }
            }
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null)
            {
                _currentRecipe.Ingredients.Add(new Ingredient { Name = "", Quantity = "" });
                RefreshIngredientsList();
            }
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null && sender is Button button && button.DataContext is Ingredient ingredient)
            {
                _currentRecipe.Ingredients.Remove(ingredient);
                RefreshIngredientsList();
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

            // Remove empty ingredients
            _currentRecipe.Ingredients = new ObservableCollection<Ingredient>(
                _currentRecipe.Ingredients.Where(i => !string.IsNullOrWhiteSpace(i.Name))
            );

            if (!_recipes.Contains(_currentRecipe))
            {
                _recipes.Add(_currentRecipe);
            }

            _recipeDatabase.SaveRecipes(_recipes.ToList());
            LoadRecipes();
            ClearInputs();
            _currentRecipe = null;
        }

        private void AddToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null && _currentRecipe.Ingredients.Any())
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                var shoppingListPage = mainWindow.GetShoppingListPage();
                var ingredients = _currentRecipe.Ingredients
                    .Select(i => $"{i.Quantity} {i.Name}")
                    .ToList();
                shoppingListPage.AddIngredientsToShoppingList(ingredients);
                MessageBox.Show("Ingredients added to shopping list!", "Success");
            }
            else
            {
                MessageBox.Show("No ingredients to add to the shopping list.", "Information");
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
