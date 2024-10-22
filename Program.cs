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
            application.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            application.Run();
        }
    }
}