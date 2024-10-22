using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using RecipeManager.BusinessLogic;

namespace RecipeManager.GUI
{
    [SupportedOSPlatform("windows")]
    public partial class RecipeSearchForm : Form
    {
        private readonly RecipeLogic _recipeLogic;
        private List<RecipeSearchResult> searchResults;

        public RecipeSearchForm()
        {
            InitializeComponent();
            string fatSecretConsumerKey = "614e76da537c4a61a07a73763b373951";
            string fatSecretConsumerSecret = "f72938e96e4e4ee7bf42873070c91110";
            _recipeLogic = new RecipeLogic(fatSecretConsumerKey, fatSecretConsumerSecret);
        }

        private void InitializeComponent()
        {
            this.txtSearchTerm = new TextBox();
            this.btnSearch = new Button();
            this.lstResults = new ListBox();
            this.txtIngredients = new RichTextBox();
            this.txtNutrition = new RichTextBox();
            this.SuspendLayout();

            // txtSearchTerm
            this.txtSearchTerm.Location = new Point(12, 12);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new Size(250, 20);
            this.txtSearchTerm.TabIndex = 0;

            // btnSearch
            this.btnSearch.Location = new Point(268, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // lstResults
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new Point(12, 38);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new Size(331, 324);
            this.lstResults.TabIndex = 2;
            this.lstResults.SelectedIndexChanged += new EventHandler(this.lstResults_SelectedIndexChanged);

            // txtIngredients
            this.txtIngredients.Location = new Point(349, 38);
            this.txtIngredients.Name = "txtIngredients";
            this.txtIngredients.ReadOnly = true;
            this.txtIngredients.Size = new Size(400, 324);
            this.txtIngredients.TabIndex = 3;
            this.txtIngredients.Text = "";

            // txtNutrition
            this.txtNutrition.Location = new Point(755, 38);
            this.txtNutrition.Name = "txtNutrition";
            this.txtNutrition.ReadOnly = true;
            this.txtNutrition.Size = new Size(250, 324);
            this.txtNutrition.TabIndex = 4;
            this.txtNutrition.Text = "";

            // RecipeSearchForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1020, 374);
            this.Controls.Add(this.txtNutrition);
            this.Controls.Add(this.txtIngredients);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchTerm);
            this.Name = "RecipeSearchForm";
            this.Text = "食谱搜索";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtSearchTerm;
        private Button btnSearch;
        private ListBox lstResults;
        private RichTextBox txtIngredients;
        private RichTextBox txtNutrition;

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
            txtIngredients.Clear();
            txtNutrition.Clear();
            var searchTerm = txtSearchTerm.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("请输入搜索词");
                return;
            }

            try
            {
                searchResults = await _recipeLogic.SearchRecipes(searchTerm);
                foreach (var result in searchResults)
                {
                    lstResults.Items.Add(result.Name);
                }

                if (searchResults.Count == 0)
                {
                    MessageBox.Show("没有找到匹配的食谱");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索过程中发生错误: {ex.Message}");
            }
        }

        private async void lstResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstResults.SelectedIndex != -1 && searchResults != null && lstResults.SelectedIndex < searchResults.Count)
            {
                var selectedResult = searchResults[lstResults.SelectedIndex];
                try
                {
                    var recipe = await _recipeLogic.GetRecipeDetails(selectedResult.Id);
                    DisplayRecipeIngredients(recipe);

                    txtNutrition.Clear();
                    txtNutrition.AppendText("正在分析营养信息...\n请稍候");

                    var (ingredientsNutrition, totalNutrition) = await _recipeLogic.CalculateRecipeNutrition(recipe);
                    DisplayNutritionInfo(ingredientsNutrition, totalNutrition);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取食谱详情时发生错误: {ex.Message}");
                }
            }
        }

        private void DisplayRecipeIngredients(Recipe recipe)
        {
            txtIngredients.Clear();
            txtIngredients.SelectionFont = new Font(txtIngredients.Font, FontStyle.Bold);
            txtIngredients.AppendText($"{recipe.Name}\n\n");
            txtIngredients.SelectionFont = txtIngredients.Font;

            txtIngredients.AppendText("食材:\n\n");

            foreach (var (measure, ingredient) in _recipeLogic.GetFormattedIngredients(recipe))
            {
                txtIngredients.AppendText($"• {measure} {ingredient}\n");
            }
        }

        private void DisplayNutritionInfo(List<IngredientNutrition> ingredientsNutrition, NutritionInfo totalNutrition)
        {
            txtNutrition.Clear();
            txtNutrition.SelectionFont = new Font(txtNutrition.Font, FontStyle.Bold);
            txtNutrition.AppendText("总营养信息:\n\n");
            txtNutrition.SelectionFont = txtNutrition.Font;

            txtNutrition.AppendText($"总卡路里: {totalNutrition.Calories:F2} kcal\n");
            txtNutrition.AppendText($"总蛋白质: {totalNutrition.Protein:F2} g\n");
            txtNutrition.AppendText($"总碳水化合物: {totalNutrition.Carbohydrates:F2} g\n");
            txtNutrition.AppendText($"总脂肪: {totalNutrition.Fat:F2} g\n\n");

            txtNutrition.SelectionFont = new Font(txtNutrition.Font, FontStyle.Bold);
            txtNutrition.AppendText("各食材卡路里:\n\n");
            txtNutrition.SelectionFont = txtNutrition.Font;

            foreach (var item in ingredientsNutrition)
            {
                txtNutrition.AppendText($"{item.Ingredient}: {item.Nutrition.Calories:F2} kcal\n");
            }
        }
    }
}