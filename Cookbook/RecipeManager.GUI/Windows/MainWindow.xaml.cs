using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
<<<<<<< HEAD
=======
using System.IO;
>>>>>>> Yao
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
<<<<<<< HEAD
=======
        private readonly string _shoppingListPath = "shoppingList.json";
>>>>>>> Yao
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

<<<<<<< HEAD
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
=======
                _shoppingListDatabase = new ShoppingListDatabase(_shoppingListPath);

                _addedRecipes = new ObservableCollection<RecipeItem>();
                addedRecipesList.ItemsSource = _addedRecipes;

                InitializePages();
                LoadShoppingListItems();

                // Add window closing event handler
                this.Closing += MainWindow_Closing;
>>>>>>> Yao
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}");
            }
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Initialize all pages of the application
        /// </summary>
        private void InitializePages()
        {
            _searchPage = new SearchRecipePage(_recipeLogic);
            searchFrame.Content = _searchPage;

            _favoritePage = new FavoriteRecipePage(_recipeLogic);
            favoriteFrame.Content = _favoritePage;

            _shoppingPage = new ShoppingListPage();
            shoppingFrame.Content = _shoppingPage;
        }

        /// <summary>
        /// Load shopping list items and display them in the list
        /// </summary>
>>>>>>> Yao
        private void LoadShoppingListItems()
        {
            try
            {
<<<<<<< HEAD
                _addedRecipes.Clear(); // 清除现有项目
                var shoppingItems = _shoppingListDatabase.LoadShoppingList();

                // 按名称分组并只添加一次
=======
                _addedRecipes.Clear();
                var shoppingItems = _shoppingListDatabase.LoadShoppingList();

>>>>>>> Yao
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

<<<<<<< HEAD
        // 用于绑定的食谱项类
=======
        /// <summary>
        /// Clean up database when application closes
        /// </summary>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (File.Exists(_shoppingListPath))
                {
                    File.Delete(_shoppingListPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cleaning up database: {ex.Message}");
            }
        }

        /// <summary>
        /// Recipe item class for binding
        /// </summary>
>>>>>>> Yao
        public class RecipeItem
        {
            public string Name { get; set; }
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Handle navigation between different pages
        /// </summary>
>>>>>>> Yao
        private void NavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (navigationList.SelectedItem == null) return;

<<<<<<< HEAD
            // Hide all pages
=======
            // 当选择导航项时，确保隐藏欢迎页
>>>>>>> Yao
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
<<<<<<< HEAD
                    LoadShoppingListItems(); // 刷新购物清单显示
=======
                    LoadShoppingListItems();
>>>>>>> Yao
                    break;
            }
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Handle search button click in welcome page
        /// </summary>
>>>>>>> Yao
        private void WelcomeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = welcomeSearchBox.Text;
            navigationList.SelectedItem = searchRecipeItem;
            _searchPage.SearchRecipes(searchTerm);
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Handle enter key press in welcome page search box
        /// </summary>
>>>>>>> Yao
        private void WelcomeSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                WelcomeSearchButton_Click(sender, e);
            }
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Get the shopping list page instance
        /// </summary>
>>>>>>> Yao
        public ShoppingListPage GetShoppingListPage()
        {
            return _shoppingPage;
        }
<<<<<<< HEAD
=======

        private void AppTitle_Click(object sender, RoutedEventArgs e)
        {
            // 清除导航列表的选择
            navigationList.SelectedItem = null;

            // 隐藏所有页面
            recipeManagementPage.Visibility = Visibility.Collapsed;
            customRecipePage.Visibility = Visibility.Collapsed;
            favoriteRecipePage.Visibility = Visibility.Collapsed;
            shoppingListPage.Visibility = Visibility.Collapsed;

            // 显示欢迎页
            welcomePage.Visibility = Visibility.Visible;

            // 清空欢迎页搜索框
            welcomeSearchBox.Text = string.Empty;
        }

      

>>>>>>> Yao
    }
}