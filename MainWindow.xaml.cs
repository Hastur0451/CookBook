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
        private UserControl currentShoppingListForm;
        private UserControl currentNutritionForm;

        public MainWindow()
        {
            InitializeComponent();
            string fatSecretConsumerKey = "614e76da537c4a61a07a73763b373951";
            string fatSecretConsumerSecret = "f72938e96e4e4ee7bf42873070c91110";
            _recipeLogic = new RecipeLogic(fatSecretConsumerKey, fatSecretConsumerSecret);

            // 取消导航列表的默认选中项
            navigationList.SelectedIndex = -1;
        }

        private void NavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (navigationList.SelectedItem == null) return;

            // 隐藏欢迎页面
            welcomePage.Visibility = Visibility.Collapsed;

            var selectedItem = ((ListBoxItem)navigationList.SelectedItem).Content.ToString();

            switch (selectedItem)
            {
                case "Recipe Management":
                    ShowRecipeManagementPage();
                    break;

               /* case "Shopping List Generator":
                    if (currentShoppingListForm == null)
                    {
                        currentShoppingListForm = new ShoppingListForm();
                    }
                    mainContent.Content = currentShoppingListForm;
                    break;

                case "Nutrition Information":
                    if (currentNutritionForm == null)
                    {
                        currentNutritionForm = new NutritionForm();
                    }
                    mainContent.Content = currentNutritionForm;
                    break;*/
            }
        }

        private void ShowRecipeManagementPage()
        {
            recipeManagementPage.Visibility = Visibility.Visible;
            mainContent.Content = null;
        }

        private async void PerformSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("请输入搜索词");
                return;
            }

            try
            {
                // 切换到配方管理页面
                navigationList.SelectedIndex = 0;
                ShowRecipeManagementPage();

                // 复制搜索词到搜索框
                txtSearch.Text = searchTerm;

                // 禁用搜索按钮
                btnSearch.IsEnabled = false;
                welcomeSearchButton.IsEnabled = false;

                // 执行搜索
                _searchResults = await _recipeLogic.SearchRecipes(searchTerm);
                lstResults.Items.Clear();

                foreach (var result in _searchResults)
                {
                    lstResults.Items.Add(result.Name);
                }

                if (_searchResults.Count == 0)
                {
                    MessageBox.Show("没有找到匹配的食谱");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索过程中发生错误: {ex.Message}");
            }
            finally
            {
                btnSearch.IsEnabled = true;
                welcomeSearchButton.IsEnabled = true;
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

        private async void lstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResults.SelectedIndex != -1 && _searchResults != null)
            {
                var selectedResult = _searchResults[lstResults.SelectedIndex];
                try
                {
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);

                    // 设置标题
                    txtTitle.Text = recipe.Name;

                    // 清空并添加食材列表
                    ingredientsList.Items.Clear();
                    foreach (var (measure, ingredient) in _recipeLogic.GetFormattedIngredients(recipe))
                    {
                        ingredientsList.Items.Add($"• {measure} {ingredient}");
                    }

                    // 设置烹饪步骤
                    txtInstructions.Text = recipe.Instructions;

                    // 如果没有内容，显示提示信息
                    if (string.IsNullOrWhiteSpace(recipe.Instructions))
                    {
                        txtInstructions.Text = "暂无详细的烹饪步骤信息。";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取食谱详情时发生错误: {ex.Message}");
                }
            }
        }

    }
}