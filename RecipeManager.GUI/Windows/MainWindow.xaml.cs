using CookBook.RecipeManager.GUI.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeManager.BusinessLogic;

namespace CookBook.RecipeManager.GUI.Windows
{
    public partial class MainWindow : Window
    {
        private readonly RecipeLogic _recipeLogic;
        private SearchRecipePage _searchPage;
        private FavoriteRecipePage _favoritePage;
        private ShoppingListPage _shoppingPage;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                string fatSecretConsumerKey = "614e76da537c4a61a07a73763b373951";
                string fatSecretConsumerSecret = "f72938e96e4e4ee7bf42873070c91110";
                _recipeLogic = new RecipeLogic(fatSecretConsumerKey, fatSecretConsumerSecret);

                // 初始化搜索页面
                _searchPage = new SearchRecipePage(_recipeLogic);
                searchFrame.Content = _searchPage;

                _favoritePage = new FavoriteRecipePage(_recipeLogic);
                favoriteFrame.Content = _favoritePage;

                _shoppingPage = new ShoppingListPage();
                shoppingFrame.Content = _shoppingPage;
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
            favoriteRecipePage.Visibility = Visibility.Collapsed;
            shoppingListPage.Visibility = Visibility.Collapsed;

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
                    favoriteRecipePage.Visibility = Visibility.Visible;
                    break;
                case "shoppingListItem":
                    shoppingListPage.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void WelcomeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = welcomeSearchBox.Text;
            navigationList.SelectedItem = searchRecipeItem;
            _searchPage.SearchRecipes(searchTerm);
        }

        private void WelcomeSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                WelcomeSearchButton_Click(sender, e);
            }
        }
    }
}