using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CookBook.RecipeManager.GUI.Models;
using RecipeManager.BusinessLogic;
using CookBook.RecipeManager.GUI.Windows;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class SearchRecipePage : Page
    {
        // Logic handler for recipe operations
        private readonly RecipeLogic _recipeLogic;

        // List to store search results
        private List<RecipeSearchResult> _searchResults;

        public SearchRecipePage(RecipeLogic recipeLogic)
        {
            InitializeComponent();
            _recipeLogic = recipeLogic;
        }

        // Searches recipes based on a search term
        public async void SearchRecipes(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a search term");
                return;
            }

            try
            {
                // Disable search button during search
                btnSearch.IsEnabled = false;
                txtSearch.Text = searchTerm;

                // Fetch search results
                _searchResults = await _recipeLogic.SearchRecipes(searchTerm);
                lstResults.ItemsSource = _searchResults;

                // Show message if no results found
                if (_searchResults.Count == 0)
                {
                    MessageBox.Show("No matching recipes found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred during search: {ex.Message}");
            }
            finally
            {
                // Re-enable search button after search
                btnSearch.IsEnabled = true;
            }
        }

        // Handles search button click event
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchRecipes(txtSearch.Text);
        }

        // Handles selection change in search results list
        private async void LstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResults.SelectedItem is RecipeSearchResult selectedResult)
            {
                try
                {
                    // Fetch recipe details for selected item
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    // Display recipe details
                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    // Set favorite button state
                    favoriteButton.RecipeId = selectedResult.Id;
                    favoriteButton.IsFavorite = _recipeLogic.IsFavoriteRecipe(selectedResult.Id);

                    // Populate ingredients list with checkboxes
                    ingredientsList.ItemsSource = _recipeLogic.GetFormattedIngredients(recipe)
                        .Select(ingredient => new CheckBox { Content = $"{ingredient.Measure} {ingredient.Ingredient}", IsChecked = false });

                    // Display instructions or placeholder if none
                    recipeInstructions.Text = recipe.Instructions;
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        recipeInstructions.Text = "No detailed instructions available.";
                    }

                    // Collapse instructions section
                    instructionsExpander.IsExpanded = false;

                    // Reset scroll position to top
                    if (recipeDetailPanel.Parent is ScrollViewer scrollViewer)
                    {
                        scrollViewer.ScrollToTop();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred while getting recipe details: {ex.Message}");
                }
            }
            else
            {
                // Hide recipe details panel if no selection
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        // Adds selected ingredients to the shopping list
        private void BtnSaveList_Click(object sender, RoutedEventArgs e)
        {
            var checkedIngredients = ingredientsList.Items
                .Cast<object>()
                .Where(item => item is CheckBox checkBox && checkBox.IsChecked == true)
                .Select(item => ((CheckBox)item).Content.ToString())
                .ToList();

            if (checkedIngredients.Any())
            {
                // Add ingredients to shopping list
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                var shoppingListPage = mainWindow.GetShoppingListPage();
                shoppingListPage.AddIngredientsToShoppingList(checkedIngredients);
                MessageBox.Show("Selected ingredients added to the shopping list!", "Success");
            }
            else
            {
                MessageBox.Show("No ingredients selected. Please check the ingredients you want to add to the shopping list.", "Information");
            }
        }

        // Handles favorite button state change event
        private void FavoriteButton_FavoriteChanged(object sender, FavoriteEventArgs e)
        {
            try
            {
                if (e.IsFavorite)
                {
                    // Add recipe to favorites
                    var recipe = _searchResults.FirstOrDefault(r => r.Id == e.RecipeId);
                    if (recipe != null)
                    {
                        _recipeLogic.AddFavoriteRecipe(recipe);
                        MessageBox.Show("Recipe added to favorites!");
                    }
                }
                else
                {
                    // Remove recipe from favorites
                    _recipeLogic.RemoveFavoriteRecipe(e.RecipeId);
                    MessageBox.Show("Recipe removed from favorites!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating favorite status: {ex.Message}");
            }
        }
    }
}
