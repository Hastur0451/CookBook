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
            LoadFavoriteRecipes();
        }

        private async void LoadFavoriteRecipes()
        {
            try
            {
                // TODO: 从存储中获取收藏的食谱列表
                _favoriteRecipes = new List<RecipeSearchResult>();
                lstFavorites.ItemsSource = _favoriteRecipes;
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
                    favoriteButton.IsFavorite = true; // 因为是收藏页面，所以默认为true

                    ingredientsList.ItemsSource = recipe.Ingredients;

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
                // 从收藏列表中移除
                var recipeToRemove = _favoriteRecipes.FirstOrDefault(r => r.Id == e.RecipeId);
                if (recipeToRemove != null)
                {
                    _favoriteRecipes.Remove(recipeToRemove);
                    lstFavorites.ItemsSource = null;
                    lstFavorites.ItemsSource = _favoriteRecipes;
                    recipeDetailPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BtnSaveList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Shopping list save feature will be available in a future update.", "Notice");
        }
    }
}