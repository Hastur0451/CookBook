using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeManager.BusinessLogic;

namespace RecipeManager
{
    public partial class MainWindow : Window
    {
        private readonly RecipeLogic _recipeLogic;
        private List<RecipeSearchResult> _searchResults;

        public MainWindow()
        {
            InitializeComponent();
            string fatSecretConsumerKey = "614e76da537c4a61a07a73763b373951";
            string fatSecretConsumerSecret = "f72938e96e4e4ee7bf42873070c91110";
            _recipeLogic = new RecipeLogic(fatSecretConsumerKey, fatSecretConsumerSecret);

            // Initialize page state
            welcomePage.Visibility = Visibility.Visible;
            recipeManagementPage.Visibility = Visibility.Collapsed;
            recipeDetailPanel.Visibility = Visibility.Collapsed;
            navigationList.SelectedIndex = -1;
        }

        private void NavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (navigationList.SelectedItem == null) return;

            welcomePage.Visibility = Visibility.Collapsed;
            recipeDetailPanel.Visibility = Visibility.Collapsed;

            var selectedItem = ((ListBoxItem)navigationList.SelectedItem).Content.ToString();
            switch (selectedItem)
            {
                case "Recipe Management":
                    recipeManagementPage.Visibility = Visibility.Visible;
                    break;

                case "Shopping List Generator":
                    // To be implemented
                    break;

                case "Nutrition Information":
                    // To be implemented
                    break;
            }
        }

        private void WelcomeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch(welcomeSearchBox.Text);
        }

        private void WelcomeSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch(welcomeSearchBox.Text);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch(txtSearch.Text);
        }

        private async void PerformSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a search term");
                return;
            }

            try
            {
                // UI state changes remain the same

                _searchResults = await _recipeLogic.SearchRecipes(searchTerm);
                lstResults.Items.Clear();

                foreach (var result in _searchResults)
                {
                    lstResults.Items.Add(result.Name);
                }

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
                welcomeSearchButton.IsEnabled = true;
            }
        }

        private async void lstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResults.SelectedIndex != -1 && _searchResults != null)
            {
                var selectedResult = _searchResults[lstResults.SelectedIndex];
                try
                {
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    ingredientsList.Items.Clear();
                    foreach (var (measure, ingredient) in _recipeLogic.GetFormattedIngredients(recipe))
                    {
                        ingredientsList.Items.Add($"{measure} {ingredient}");
                    }

                    recipeInstructions.Text = recipe.Instructions;
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        recipeInstructions.Text = "No detailed instructions available.";
                    }

                    instructionsExpander.IsExpanded = false;
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
    }
}