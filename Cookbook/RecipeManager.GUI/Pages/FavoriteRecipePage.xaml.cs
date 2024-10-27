using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RecipeManager.BusinessLogic;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class FavoriteRecipePage : Page
    {
        // Logic handler for managing favorite recipes
        private readonly RecipeLogic _recipeLogic;

        // List to store favorite recipes
        private List<RecipeSearchResult> _favoriteRecipes;

        public FavoriteRecipePage(RecipeLogic recipeLogic)
        {
            InitializeComponent();
            _recipeLogic = recipeLogic;
        }

        // Loads favorite recipes and updates the UI
        public void LoadFavoriteRecipes()
        {
            try
            {
                // Fetch favorite recipes from the logic layer
                _favoriteRecipes = _recipeLogic.GetFavoriteRecipes();
                lstFavorites.ItemsSource = null; // Clear the current ItemsSource
                lstFavorites.ItemsSource = _favoriteRecipes;

                // Show message if there are no favorite recipes
                if (_favoriteRecipes.Count == 0)
                {
                    MessageBox.Show("You have no favorite recipes yet.", "No Favorites");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading favorite recipes: {ex.Message}");
            }
        }

        // Filters favorite recipes based on search input
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Show all favorites if search term is empty
                lstFavorites.ItemsSource = _favoriteRecipes;
            }
            else
            {
                // Filter favorites based on search term
                lstFavorites.ItemsSource = _favoriteRecipes
                    .Where(r => r.Name.ToLower().Contains(searchTerm));
            }
        }

        // Handles selection change in favorite recipes list
        private async void LstFavorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFavorites.SelectedItem is RecipeSearchResult selectedResult)
            {
                try
                {
                    // Fetch recipe details for the selected favorite recipe
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    // Display recipe details
                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    // Set favorite button state
                    favoriteButton.RecipeId = selectedResult.Id;
                    favoriteButton.IsFavorite = true;

                    // Populate ingredients list
                    ingredientsList.ItemsSource = _recipeLogic.GetFormattedIngredients(recipe);

                    // Display instructions or placeholder if none
                    recipeInstructions.Text = recipe.Instructions;
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        recipeInstructions.Text = "No detailed instructions available.";
                    }

                    // Collapse instructions section initially
                    instructionsExpander.IsExpanded = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading recipe details: {ex.Message}");
                }
            }
            else
            {
                // Hide recipe details panel if no selection
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        // Handles changes to the favorite button state
        private void FavoriteButton_FavoriteChanged(object sender, Models.FavoriteEventArgs e)
        {
            if (!e.IsFavorite)
            {
                // Remove recipe from favorites and refresh list
                _recipeLogic.RemoveFavoriteRecipe(e.RecipeId);
                LoadFavoriteRecipes();
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        // Placeholder for save list functionality
        private void BtnSaveList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Shopping list save feature will be available in a future update.", "Notice");
        }
    }
}
