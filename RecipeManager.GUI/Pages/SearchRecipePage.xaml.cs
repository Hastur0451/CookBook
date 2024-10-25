using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CookBook.RecipeManager.GUI.Models;
using RecipeManager.BusinessLogic;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class SearchRecipePage : Page
    {
        private readonly RecipeLogic _recipeLogic;
        private List<RecipeSearchResult> _searchResults;

        public SearchRecipePage(RecipeLogic recipeLogic)
        {
            InitializeComponent();
            _recipeLogic = recipeLogic;
        }

        public async void SearchRecipes(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a search term");
                return;
            }

            try
            {
                btnSearch.IsEnabled = false;
                txtSearch.Text = searchTerm;

                _searchResults = await _recipeLogic.SearchRecipes(searchTerm);
                lstResults.ItemsSource = _searchResults;

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
                btnSearch.IsEnabled = true;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchRecipes(txtSearch.Text);
        }

        private async void LstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResults.SelectedItem is RecipeSearchResult selectedResult)
            {
                try
                {
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    favoriteButton.RecipeId = selectedResult.Id;
                    favoriteButton.IsFavorite = false; // TODO: 从存储中获取实际的收藏状态

                    ingredientsList.ItemsSource = recipe.Ingredients;

                    recipeInstructions.Text = recipe.Instructions;
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        recipeInstructions.Text = "No detailed instructions available.";
                    }

                    instructionsExpander.IsExpanded = false;

                    // 重置滚动位置
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
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnSaveList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Shopping list save feature will be available in a future update.", "Notice");
        }

        private void FavoriteButton_FavoriteChanged(object sender, FavoriteEventArgs e)
        {
            try
            {
                if (e.IsFavorite)
                {
                    // TODO: 保存收藏状态
                    MessageBox.Show("Recipe added to favorites!");
                }
                else
                {
                    // TODO: 移除收藏状态
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