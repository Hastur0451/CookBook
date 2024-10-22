using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = txtSearch.Text;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("请输入搜索词");
                return;
            }

            try
            {
                btnSearch.IsEnabled = false;
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
                    txtTitle.Text = recipe.Name;

                    var detailsText = "食材:\n\n";
                    foreach (var (measure, ingredient) in _recipeLogic.GetFormattedIngredients(recipe))
                    {
                        detailsText += $"• {measure} {ingredient}\n";
                    }

                    detailsText += "\n烹饪步骤:\n\n" + recipe.Instructions;
                    txtDetails.Text = detailsText;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取食谱详情时发生错误: {ex.Message}");
                }
            }
        }
    }
}