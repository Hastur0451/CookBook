using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeManager.BusinessLogic;
using CookBook.RecipeManager.GUI.Pages;
using System.Linq;
using CookBook.RecipeManager.GUI.Models;
using RecipeManager.DataBase;

namespace CookBook.RecipeManager.GUI.Windows
{
    public partial class MainWindow : Window
    {
        private readonly RecipeLogic _recipeLogic;
        private SearchRecipePage _searchPage;
        private FavoriteRecipePage _favoritePage;
        private ShoppingListPage _shoppingPage;
        private ObservableCollection<RecipeItem> _addedRecipes;
        private readonly ShoppingListDatabase _shoppingListDatabase;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                string fatSecretConsumerKey = "614e76da537c4a61a07a73763b373951";
                string fatSecretConsumerSecret = "f72938e96e4e4ee7bf42873070c91110";
                _recipeLogic = new RecipeLogic(fatSecretConsumerKey, fatSecretConsumerSecret);

                // 初始化购物清单数据库
                _shoppingListDatabase = new ShoppingListDatabase("shoppingList.json");

                // 初始化食谱列表
                _addedRecipes = new ObservableCollection<RecipeItem>();
                addedRecipesList.ItemsSource = _addedRecipes;

                // 初始化各页面
                _searchPage = new SearchRecipePage(_recipeLogic);
                searchFrame.Content = _searchPage;

                _favoritePage = new FavoriteRecipePage(_recipeLogic);
                favoriteFrame.Content = _favoritePage;

                _shoppingPage = new ShoppingListPage();
                shoppingFrame.Content = _shoppingPage;

                // 加载购物清单中的食材
                LoadShoppingListItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}");
            }
        }

        private void LoadShoppingListItems()
        {
            try
            {
                _addedRecipes.Clear(); // 清除现有项目
                var shoppingItems = _shoppingListDatabase.LoadShoppingList();

                // 按名称分组并只添加一次
                var groupedItems = shoppingItems
                    .Select(item => item.Name)
                    .Distinct()
                    .OrderBy(name => name);

                foreach (var itemName in groupedItems)
                {
                    _addedRecipes.Add(new RecipeItem { Name = itemName });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shopping list items: {ex.Message}");
            }
        }

        // 用于绑定的食谱项类
        public class RecipeItem
        {
            public string Name { get; set; }
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
                    _favoritePage.LoadFavoriteRecipes();
                    break;
                case "shoppingListItem":
                    shoppingListPage.Visibility = Visibility.Visible;
                    LoadShoppingListItems(); // 刷新购物清单显示
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

        public ShoppingListPage GetShoppingListPage()
        {
            return _shoppingPage;
        }
    }
}