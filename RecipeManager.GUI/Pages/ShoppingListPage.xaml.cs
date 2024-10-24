using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RecipeManager.DataBase;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class ShoppingListPage : Page
    {
        private readonly ShoppingListDatabase _shoppingListDatabase;
        private List<string> _shoppingList;

        public ShoppingListPage()
        {
            InitializeComponent();
            _shoppingListDatabase = new ShoppingListDatabase("shoppingList.json");
            _shoppingList = _shoppingListDatabase.LoadShoppingList();
            shoppingListBox.ItemsSource = _shoppingList;
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string item)
            {
                _shoppingList.Remove(item);
                shoppingListBox.ItemsSource = null;
                shoppingListBox.ItemsSource = _shoppingList;
            }
        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            _shoppingList.Clear();
            shoppingListBox.ItemsSource = null;
            shoppingListBox.ItemsSource = _shoppingList;
        }

        private void SaveList_Click(object sender, RoutedEventArgs e)
        {
            _shoppingListDatabase.SaveShoppingList(_shoppingList);
            MessageBox.Show("Shopping list saved!");
        }
    }
}
