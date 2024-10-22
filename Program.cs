using System;
using System.Windows.Forms;
using RecipeManager.GUI;

namespace RecipeManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RecipeSearchForm());
        }
    }
}