using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using RecipeManager.BusinessLogic;
using System.DirectoryServices;

namespace RecipeManager.GUI
{
    public partial class RecipeSearchForm : Form
    {
        private readonly TheMealDBClient _client;
        private List<RecipeSearchResult> searchResults;

        public RecipeSearchForm()
        {
            InitializeComponent();
            _client = new TheMealDBClient();
        }

        private void InitializeComponent()
        {
            this.txtSearchTerm = new TextBox();
            this.btnSearch = new Button();
            this.lstResults = new ListBox();
            this.SuspendLayout();

            // txtSearchTerm
            this.txtSearchTerm.Location = new System.Drawing.Point(12, 12);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(200, 20);
            this.txtSearchTerm.TabIndex = 0;

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(218, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // lstResults
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(12, 38);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(281, 212);
            this.lstResults.TabIndex = 2;
            this.lstResults.SelectedIndexChanged += new System.EventHandler(this.lstResults_SelectedIndexChanged);

            // RecipeSearchForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 262);
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

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
            var searchTerm = txtSearchTerm.Text;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("请输入搜索词");
                return;
            }

            try
            {
                searchResults = await _client.SearchMeals(searchTerm);
                foreach (var result in searchResults)
                {
                    lstResults.Items.Add(result.Name);  // 直接添加食谱名称
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
                    var recipe = await _client.GetRecipeById(selectedResult.Id);
                    var detailsForm = new RecipeDetailsForm(recipe);
                    detailsForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取食谱详情时发生错误: {ex.Message}");
                }
            }
        }
    }

    public class RecipeDetailsForm : Form
    {
        public RecipeDetailsForm(Recipe recipe)
        {
            InitializeComponent(recipe);
        }

        private void InitializeComponent(Recipe recipe)
        {
            this.txtDetails = new TextBox();
            this.SuspendLayout();

            // txtDetails
            this.txtDetails.Location = new System.Drawing.Point(12, 12);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = ScrollBars.Vertical;
            this.txtDetails.Size = new System.Drawing.Size(360, 337);
            this.txtDetails.TabIndex = 0;

            // RecipeDetailsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.txtDetails);
            this.Name = "RecipeDetailsForm";
            this.Text = "食谱详情";
            this.ResumeLayout(false);
            this.PerformLayout();

            // Populate details
            var details = $"名称: {recipe.Name}\r\n";
            details += $"类别: {recipe.Category}\r\n";
            details += $"区域: {recipe.Area}\r\n\r\n";
            details += "食材:\r\n";
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                details += $"- {recipe.Measures[i]} {recipe.Ingredients[i]}\r\n";
            }
            details += "\r\n烹饪步骤:\r\n";
            details += recipe.Instructions;

            this.txtDetails.Text = details;
        }

        private TextBox txtDetails;
    }
}