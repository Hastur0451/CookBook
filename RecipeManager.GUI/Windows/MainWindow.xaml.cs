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

            // 初始化页面状态
            welcomePage.Visibility = Visibility.Visible;
            recipeManagementPage.Visibility = Visibility.Collapsed;
            recipeDetailPanel.Visibility = Visibility.Collapsed;
        }

        // 窗口控制
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // 导航控制
        private void NavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (navigationList.SelectedItem is ListBoxItem selectedItem)
            {
                // 隐藏所有页面
                welcomePage.Visibility = Visibility.Collapsed;
                recipeManagementPage.Visibility = Visibility.Collapsed;
                recipeDetailPanel.Visibility = Visibility.Collapsed;

                // 根据选择显示相应页面
                var content = (selectedItem.Content as StackPanel)?.Children[1] as TextBlock;
                if (content != null)
                {
                    switch (content.Text)
                    {
                        case "Recipe Management":
                            recipeManagementPage.Visibility = Visibility.Visible;
                            break;
                        case "Shopping List Generator":
                            // TODO: 实现购物清单生成器页面
                            break;
                        case "Nutrition Information":
                            // TODO: 实现营养信息页面
                            break;
                    }
                }
            }
        }

        // 搜索功能
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
                // 切换到搜索结果页面
                welcomePage.Visibility = Visibility.Collapsed;
                recipeManagementPage.Visibility = Visibility.Visible;
                recipeDetailPanel.Visibility = Visibility.Collapsed;

                // 选中Recipe Management导航项
                navigationList.SelectedIndex = 0;

                // 复制搜索词并执行搜索
                txtSearch.Text = searchTerm;
                btnSearch.IsEnabled = false;
                welcomeSearchButton.IsEnabled = false;

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

                    // 更新UI
                    recipeDetailPanel.Visibility = Visibility.Visible;
                    recipeTitle.Text = recipe.Name;

                    // 更新食材列表
                    ingredientsList.Items.Clear();
                    foreach (var (measure, ingredient) in _recipeLogic.GetFormattedIngredients(recipe))
                    {
                        var checkBox = new CheckBox
                        {
                            Content = $"{measure} {ingredient}",
                            Margin = new Thickness(0, 4, 0, 4)
                        };
                        ingredientsList.Items.Add(checkBox);
                    }

                    // 更新烹饪步骤
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
            var selectedIngredients = new List<string>();
            foreach (CheckBox item in ingredientsList.Items)
            {
                if (item.IsChecked == true)
                {
                    selectedIngredients.Add(item.Content.ToString());
                }
            }

            if (selectedIngredients.Count > 0)
            {
                MessageBox.Show($"Selected {selectedIngredients.Count} ingredients.\nThis feature will be implemented in future updates.",
                              "Shopping List");
            }
            else
            {
                MessageBox.Show("Please select at least one ingredient.",
                              "Shopping List");
            }
        }
    }
}