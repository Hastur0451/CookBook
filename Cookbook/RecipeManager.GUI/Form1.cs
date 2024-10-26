using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始时隐藏所有 Panel
            panelRecipeManagement.Visible = false;
            panelShoppingList.Visible = false;
            panelNutritionInfo.Visible = false;
        }

        private void RMF_Click(object sender, EventArgs e)
        {
            // 显示 Recipe Management Panel，隐藏其他 Panel
            panelRecipeManagement.Visible = true;
            panelShoppingList.Visible = false;
            panelNutritionInfo.Visible = false;
        }

        private void SLGF_Click(object sender, EventArgs e)
        {
            // 显示 Shopping List Panel，隐藏其他 Panel
            panelRecipeManagement.Visible = false;
            panelShoppingList.Visible = true;
            panelNutritionInfo.Visible = false;
        }

        private void NIF_Click(object sender, EventArgs e)
        {
            // 显示 Nutrition Information Panel，隐藏其他 Panel
            panelRecipeManagement.Visible = false;
            panelShoppingList.Visible = false;
            panelNutritionInfo.Visible = true;
        }
    }
}
