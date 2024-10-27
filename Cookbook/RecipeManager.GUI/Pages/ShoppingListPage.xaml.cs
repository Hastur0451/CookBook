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
        // Observable collection to hold shopping list items
        private ObservableCollection<ShoppingListItem> _shoppingItems;

        // Database object for loading and saving shopping list
        private readonly ShoppingListDatabase _shoppingListDatabase;

        // Merger utility for combining duplicate items
        private readonly ShoppingListMerger _merger = new ShoppingListMerger();

        public ShoppingListPage()
        {
            InitializeComponent();
            _shoppingListDatabase = new ShoppingListDatabase("shoppingList.json");
            LoadShoppingList();
        }

        // Loads shopping list items from the database
        private void LoadShoppingList()
        {
            var loadedItems = _shoppingListDatabase.LoadShoppingList();
            _shoppingItems = new ObservableCollection<ShoppingListItem>(loadedItems);
            shoppingList.ItemsSource = _shoppingItems;
        }

        // Saves the current shopping list to the database
        private void SaveShoppingList()
        {
            _shoppingListDatabase.SaveShoppingList(_shoppingItems.ToList());
        }

        // Adds a single ingredient to the shopping list
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
                    IsSelected = true
                });
            }
            RefreshShoppingList();
            SaveShoppingList();
        }

        // Adds multiple ingredients to the shopping list
        public void AddIngredientsToShoppingList(List<string> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                AddToShoppingList(ingredient);
            }
            RefreshShoppingList();
            SaveShoppingList();
        }

        // Refreshes the shopping list UI to reflect updates
        private void RefreshShoppingList()
        {
            var temp = shoppingList.ItemsSource;
            shoppingList.ItemsSource = null;
            shoppingList.ItemsSource = temp;
        }

        // Opens the popup for adding a new item
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            txtNewItemName.Text = "";
            txtNewItemQuantity.Text = "1";
            addItemPopup.IsOpen = true;
        }

        // Confirms the addition of a new item to the shopping list
        private void BtnConfirmAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNewItemName.Text))
            {
                var ingredient = txtNewItemName.Text.Trim();
                if (!string.IsNullOrWhiteSpace(txtNewItemQuantity.Text))
                {
                    ingredient = $"{txtNewItemQuantity.Text.Trim()} {ingredient}";
                }
                AddToShoppingList(ingredient);
                addItemPopup.IsOpen = false;
            }
        }

        // Cancels the addition of a new item
        private void BtnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            addItemPopup.IsOpen = false;
        }

        // Deletes a selected item from the shopping list
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ShoppingListItem item)
            {
                _shoppingItems.Remove(item);
                SaveShoppingList();
            }
        }

        // Copies the selected shopping list items to the clipboard
        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = _shoppingItems.Where(item => item.IsSelected).ToList();
            if (selectedItems.Any())
            {
                string shoppingList = "Shopping List:\n\n";
                foreach (var item in selectedItems)
                {
                    shoppingList += $"- {item.Name}\n";
                }

                try
                {
                    Clipboard.SetText(shoppingList);
                    MessageBox.Show("Shopping list copied to clipboard!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error copying to clipboard: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No items selected to copy.");
            }
        }

        // Saves the selected shopping list items to a text file
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
                        shoppingList += $"- {item.Name}\n";
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

        // Merges duplicate items in the shopping list
        private void BtnMergeItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentItems = _shoppingItems.ToList();
                var mergedItems = _merger.MergeItems(currentItems);

                _shoppingItems.Clear();
                foreach (var item in mergedItems)
                {
                    _shoppingItems.Add(item);
                }

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
    }
}
