using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CookBook.RecipeManager.GUI.Models;
using RecipeManager.DataBase;
using CookBook.RecipeManager.GUI.Windows;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class CustomRecipePage : UserControl
    {
        // Collection to store recipes
        private ObservableCollection<CustomRecipe> _recipes;

        // Currently selected recipe for editing or viewing
        private CustomRecipe? _currentRecipe;

        // Database for saving and loading recipes
        private readonly RecipeDatabase _recipeDatabase;

        public CustomRecipePage()
        {
            InitializeComponent();
            _recipeDatabase = new RecipeDatabase("recipes.json");
            LoadRecipes();
            HideDetailsContent();
        }

        // Loads recipes from the database and updates UI
        private void LoadRecipes()
        {
            var loadedRecipes = _recipeDatabase.LoadRecipes();
            _recipes = new ObservableCollection<CustomRecipe>(loadedRecipes);
            recipeList.ItemsSource = _recipes;
        }

        // Filters recipes based on search term
        private void BtnCustomSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = customRecipeSearch.Text.ToLower();
            var filteredRecipes = _recipes.Where(r =>
                r.Name.ToLower().Contains(searchTerm)).ToList();
            recipeList.ItemsSource = filteredRecipes;
        }

        // Shows the details content area
        private void ShowDetailsContent()
        {
            var mainGrid = recipeDetailsPanel.Child as Grid;
            if (mainGrid != null)
            {
                foreach (var child in mainGrid.Children)
                {
                    if (child is FrameworkElement element)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        // Hides the details content area
        private void HideDetailsContent()
        {
            var mainGrid = recipeDetailsPanel.Child as Grid;
            if (mainGrid != null)
            {
                foreach (var child in mainGrid.Children)
                {
                    if (child is FrameworkElement element)
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        // Initializes a new recipe for adding
        private void BtnAddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            _currentRecipe = new CustomRecipe();
            ClearInputs();
            ShowDetailsContent();
            _currentRecipe.Ingredients = new ObservableCollection<Ingredient>();
            ingredientsList.ItemsSource = _currentRecipe.Ingredients;
        }

        // Edits the selected recipe
        private void EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string recipeId)
            {
                _currentRecipe = _recipes.FirstOrDefault(r => r.Id == recipeId);
                if (_currentRecipe != null)
                {
                    ShowDetailsContent();
                    txtRecipeName.Text = _currentRecipe.Name;
                    ingredientsList.ItemsSource = _currentRecipe.Ingredients;
                }
            }
        }

        // Deletes the selected recipe
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

        // Adds a new ingredient to the current recipe
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null)
            {
                _currentRecipe.Ingredients.Add(new Ingredient { Name = "", Quantity = "" });
                RefreshIngredientsList();
            }
        }

        // Removes an ingredient from the current recipe
        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe != null && sender is Button button && button.DataContext is Ingredient ingredient)
            {
                _currentRecipe.Ingredients.Remove(ingredient);
                RefreshIngredientsList();
            }
        }

        // Saves the current recipe to the database
        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecipe == null) return;

            _currentRecipe.Name = txtRecipeName.Text;

            if (string.IsNullOrWhiteSpace(_currentRecipe.Name))
            {
                MessageBox.Show("Please enter a recipe name.");
                return;
            }

            // Remove any empty ingredients
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
            HideDetailsContent();
            _currentRecipe = null;
        }

        // Adds ingredients of the current recipe to the shopping list
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

        // Clears input fields for recipe details
        private void ClearInputs()
        {
            txtRecipeName.Text = string.Empty;
            ingredientsList.ItemsSource = null;
        }

        // Refreshes the ingredients list UI
        private void RefreshIngredientsList()
        {
            ingredientsList.ItemsSource = null;
            ingredientsList.ItemsSource = _currentRecipe?.Ingredients;
        }

        // Cancels editing the current recipe
        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            HideDetailsContent();
            _currentRecipe = null;
        }
    }
}
