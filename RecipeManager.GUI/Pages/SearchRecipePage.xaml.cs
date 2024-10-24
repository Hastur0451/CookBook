using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RecipeManager.BusinessLogic;
using RecipeManager.DataBase;
using CookBook.RecipeManager.GUI.Models;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class SearchRecipePage : Page
    {
        private readonly RecipeLogic _recipeLogic;
        private List<RecipeSearchResult> _searchResults;
        private readonly RecipeDatabase _recipeDatabase;
        private List<CustomRecipe> _localRecipes;

        public SearchRecipePage(RecipeLogic recipeLogic)
        {
            InitializeComponent();
            _recipeLogic = recipeLogic;
            _recipeDatabase = new RecipeDatabase("recipes.json");
            _localRecipes = _recipeDatabase.LoadRecipes();
            Console.WriteLine($"Loaded {_localRecipes.Count} local recipes.");
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

                // Search online recipes
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

        private async void LstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResults.SelectedItem is RecipeSearchResult selectedResult)
            {
                try
                {
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    ingredientsList.ItemsSource = recipe.Ingredients;

                    recipeInstructions.Text = recipe.Instructions;
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        recipeInstructions.Text = "No detailed instructions available.";
                    }

                    instructionsExpander.IsExpanded = false;

                    // Reset scroll position
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

        private void DisplayLocalRecipeDetails(CustomRecipe recipe)
        {
            recipeDetailPanel.Visibility = Visibility.Visible;
            recipeTitle.Text = recipe.Name;
            ingredientsList.ItemsSource = recipe.Ingredients.Select(i => $"{i.Quantity} {i.Unit} {i.Name}");
            recipeInstructions.Text = "Instructions not available for local recipes.";
            instructionsExpander.IsExpanded = false;
        }

        private void DisplayOnlineRecipeDetails(Recipe recipe)
        {
            recipeDetailPanel.Visibility = Visibility.Visible;
            recipeTitle.Text = recipe.Name;
            ingredientsList.ItemsSource = recipe.Ingredients;
            recipeInstructions.Text = string.IsNullOrWhiteSpace(recipe.Instructions) ? "No detailed instructions available." : recipe.Instructions;
            instructionsExpander.IsExpanded = false;
        }

        private void BtnSaveList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Shopping list save feature will be available in a future update.", "Notice");
        }
    }
}
