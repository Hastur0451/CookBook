using System;
using System.Windows;

namespace CookBook
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new CookBook.RecipeManager.GUI.Windows.App();
            app.Run();
        }
    }
}