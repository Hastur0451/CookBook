using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using CookBook.RecipeManager.GUI.Models;
using RecipeManager.DataBase;
using RecipeManager.BusinessLogic;

namespace CookBook.RecipeManager.GUI.Pages
{
    public partial class ShoppingListPage : Page
    {
        private ObservableCollection<ShoppingListItem> _shoppingItems;
        private readonly ShoppingListDatabase _shoppingListDatabase;
        private readonly ShoppingListMerger _merger = new ShoppingListMerger();


        public ShoppingListPage()
        {
            InitializeComponent();
            _shoppingListDatabase = new ShoppingListDatabase("shoppingList.json");
            LoadShoppingList();
        }

        private void LoadShoppingList()
        {
            var loadedItems = _shoppingListDatabase.LoadShoppingList();
            _shoppingItems = new ObservableCollection<ShoppingListItem>(loadedItems);
            shoppingList.ItemsSource = _shoppingItems;
        }

        private void SaveShoppingList()
        {
            _shoppingListDatabase.SaveShoppingList(_shoppingItems.ToList());
        }

        public void AddToShoppingList(string ingredient)
        {
            var existingItem = _shoppingItems.FirstOrDefault(item =>
                item.Name.Equals(ingredient, StringComparison.OrdinalIgnoreCase));

            if (existingItem != null)
            {
                existingItem.IsSelected = true;
            }
            else
            {
                _shoppingItems.Add(new ShoppingListItem
                {
                    Name = ingredient,
                    Quantity = "1",
                    IsSelected = true
                });
            }
            RefreshShoppingList();
            SaveShoppingList();
        }

        public void AddIngredientsToShoppingList(List<string> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                AddToShoppingList(ingredient);
            }
            RefreshShoppingList();
            SaveShoppingList();
        }

        private void RefreshShoppingList()
        {
            // Force the UI to refresh
            var temp = shoppingList.ItemsSource;
            shoppingList.ItemsSource = null;
            shoppingList.ItemsSource = temp;
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
                SaveShoppingList();
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
                SaveShoppingList();
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

        private void BtnMergeItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建当前列表的副本
                var currentItems = _shoppingItems.ToList();

                // 执行合并
                var mergedItems = _merger.MergeItems(currentItems);

                // 清空并更新列表
                _shoppingItems.Clear();
                foreach (var item in mergedItems)
                {
                    _shoppingItems.Add(item);
                }

                // 保存更新后的列表
                SaveShoppingList();

                MessageBox.Show("Shopping list items merged successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error merging items: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add this method to save changes when quantity is updated
        private void Quantity_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveShoppingList();
        }
    }
}
