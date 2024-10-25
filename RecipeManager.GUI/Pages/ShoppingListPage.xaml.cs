using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using CookBook.RecipeManager.GUI.Models;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class ShoppingListPage : Page
    {
        private ObservableCollection<ShoppingListItem> _shoppingItems;

        public ShoppingListPage()
        {
            InitializeComponent();
            _shoppingItems = new ObservableCollection<ShoppingListItem>();
            shoppingList.ItemsSource = _shoppingItems;
        }

        public void AddToShoppingList(string ingredient)
        {
            // 检查是否已存在
            var existingItem = _shoppingItems.FirstOrDefault(item =>
                item.Name.Equals(ingredient, StringComparison.OrdinalIgnoreCase));

            if (existingItem != null)
            {
                // 如果已存在，只更新选中状态
                existingItem.IsSelected = true;
            }
            else
            {
                // 如果不存在，添加新项
                _shoppingItems.Add(new ShoppingListItem
                {
                    Name = ingredient,
                    Quantity = "1",
                    IsSelected = true
                });
            }
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            txtNewItemName.Text = "";
            txtNewItemQuantity.Text = "";
            addItemPopup.IsOpen = true;
        }

        private void BtnConfirmAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNewItemName.Text))
            {
                _shoppingItems.Add(new ShoppingListItem
                {
                    Name = txtNewItemName.Text.Trim(),
                    Quantity = txtNewItemQuantity.Text.Trim(),
                    IsSelected = true
                });
                addItemPopup.IsOpen = false;
            }
        }

        private void BtnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            addItemPopup.IsOpen = false;
        }

        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ShoppingListItem item)
            {
                _shoppingItems.Remove(item);
            }
        }

        private void Quantity_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = _shoppingItems.Where(item => item.IsSelected).ToList();
            if (selectedItems.Any())
            {
                string shoppingList = "Shopping List:\n\n";
                foreach (var item in selectedItems)
                {
                    shoppingList += $"- {item.Name}: {item.Quantity}\n";
                }

                Clipboard.SetText(shoppingList);
                MessageBox.Show("Shopping list copied to clipboard!");
            }
            else
            {
                MessageBox.Show("No items selected to copy.");
            }
        }

        private void BtnSaveAsText_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = _shoppingItems.Where(item => item.IsSelected).ToList();
            if (!selectedItems.Any())
            {
                MessageBox.Show("No items selected to save.");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt",
                FileName = "ShoppingList.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string shoppingList = "Shopping List:\n\n";
                    foreach (var item in selectedItems)
                    {
                        shoppingList += $"- {item.Name}: {item.Quantity}\n";
                    }

                    File.WriteAllText(saveFileDialog.FileName, shoppingList);
                    MessageBox.Show("Shopping list saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}");
                }
            }
        }
    }
}