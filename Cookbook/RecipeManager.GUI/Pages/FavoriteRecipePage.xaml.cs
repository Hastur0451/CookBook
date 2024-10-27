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
        private readonly RecipeLogic _recipeLogic;
        private List<RecipeSearchResult> _favoriteRecipes;

        public FavoriteRecipePage(RecipeLogic recipeLogic)
        {
            InitializeComponent();
            _recipeLogic = recipeLogic;
        }

        public void LoadFavoriteRecipes()
        {
            try
            {
                _favoriteRecipes = _recipeLogic.GetFavoriteRecipes();
                lstFavorites.ItemsSource = null; // Clear the current ItemsSource
                lstFavorites.ItemsSource = _favoriteRecipes;

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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                lstFavorites.ItemsSource = _favoriteRecipes;
            }
            else
            {
                lstFavorites.ItemsSource = _favoriteRecipes
                    .Where(r => r.Name.ToLower().Contains(searchTerm));
            }
        }

        private async void LstFavorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFavorites.SelectedItem is RecipeSearchResult selectedResult)
            {
                try
                {
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    favoriteButton.RecipeId = selectedResult.Id;
                    favoriteButton.IsFavorite = true;

                    ingredientsList.ItemsSource = _recipeLogic.GetFormattedIngredients(recipe);

                    recipeInstructions.Text = recipe.Instructions;
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        recipeInstructions.Text = "No detailed instructions available.";
                    }

                    instructionsExpander.IsExpanded = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading recipe details: {ex.Message}");
                }
            }
            else
            {
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void FavoriteButton_FavoriteChanged(object sender, Models.FavoriteEventArgs e)
        {
            if (!e.IsFavorite)
            {
                _recipeLogic.RemoveFavoriteRecipe(e.RecipeId);
                LoadFavoriteRecipes();
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnSaveList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Shopping list save feature will be available in a future update.", "Notice");
        }
    }
}
