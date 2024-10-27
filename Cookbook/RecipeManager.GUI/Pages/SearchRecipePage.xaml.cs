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
                    favoriteButton.IsFavorite = _recipeLogic.IsFavoriteRecipe(selectedResult.Id);

                    // Update this part to use CheckBoxes
                    ingredientsList.ItemsSource = _recipeLogic.GetFormattedIngredients(recipe)
                        .Select(ingredient => new CheckBox { Content = $"{ingredient.Measure} {ingredient.Ingredient}", IsChecked = false });

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
            var checkedIngredients = ingredientsList.Items
                .Cast<object>()
                .Where(item => item is CheckBox checkBox && checkBox.IsChecked == true)
                .Select(item => ((CheckBox)item).Content.ToString())
                .ToList();

            if (checkedIngredients.Any())
            {
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

        private void FavoriteButton_FavoriteChanged(object sender, FavoriteEventArgs e)
        {
            try
            {
                if (e.IsFavorite)
                {
                    var recipe = _searchResults.FirstOrDefault(r => r.Id == e.RecipeId);
                    if (recipe != null)
                    {
                        _recipeLogic.AddFavoriteRecipe(recipe);
                        MessageBox.Show("Recipe added to favorites!");
                    }
                }
                else
                {
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
