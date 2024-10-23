using System;
using System.Windows;

namespace RecipeManager
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var mainWindow = new MainWindow();
            application.MainWindow = mainWindow;
            mainWindow.Show();
            application.Run();
        }
    }
}