using CookBook.RecipeManager.GUI.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CookBook.RecipeManager.GUI.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                // 可以在这里添加其他初始化代码
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}");
            }
        }

        private void NavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (navigationList.SelectedItem == null) return;

            // Hide all pages
            welcomePage.Visibility = Visibility.Collapsed;
            recipeManagementPage.Visibility = Visibility.Collapsed;
            customRecipePage.Visibility = Visibility.Collapsed;
            if (recipeDetailPanel != null)
            {
                recipeDetailPanel.Visibility = Visibility.Collapsed;
            }

            var selectedItem = ((ListBoxItem)navigationList.SelectedItem).Name;
            switch (selectedItem)
            {
                case "searchRecipeItem":
                    recipeManagementPage.Visibility = Visibility.Visible;
                    break;
                case "customRecipeItem":
                    customRecipePage.Visibility = Visibility.Visible;
                    break;
                case "favoriteRecipeItem":
                    MessageBox.Show("Favorite recipes feature coming soon!");
                    break;
                case "shoppingListItem":
                    MessageBox.Show("Shopping list feature coming soon!");
                    break;
            }
        }

        private void WelcomeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchRecipes(welcomeSearchBox.Text);
        }

        private void WelcomeSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchRecipes(welcomeSearchBox.Text);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchRecipes(txtSearch.Text);
        }

        private void SearchRecipes(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a search term");
                return;
            }

            // TODO: Implement search functionality
            MessageBox.Show($"Searching for: {searchTerm}");

            // Switch to recipe management page
            navigationList.SelectedItem = searchRecipeItem;
            recipeManagementPage.Visibility = Visibility.Visible;
            txtSearch.Text = searchTerm;
        }

        private void lstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResults.SelectedIndex != -1)
            {
                // Show recipe details
                recipeDetailPanel.Visibility = Visibility.Visible;
                // TODO: Load recipe details
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