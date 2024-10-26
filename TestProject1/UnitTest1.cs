using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using RecipeManager.DataBase;
using System.Text.Json;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.Tests
{
    [TestFixture]
    public class ShoppingListDatabaseTests
    {
        private string _testFilePath;
        private ShoppingListDatabase _database;
        private List<ShoppingListItem> _testShoppingList;

        [SetUp]
        public void Setup()
        {
            // Use temporary file path for testing
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_shopping_list.json");
            _database = new ShoppingListDatabase(_testFilePath);

            // Prepare test data matching the actual JSON structure
            _testShoppingList = new List<ShoppingListItem>
            {
                new ShoppingListItem
                {
                    Name = "1300g Potatoes",
                    Quantity = "1",
                    IsSelected = true
                },
                new ShoppingListItem
                {
                    Name = "500g Carrots",
                    Quantity = "2",
                    IsSelected = false
                }
            };
        }

        [TearDown]
        public void Cleanup()
        {
            // Clean up test file after each test
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Test]
        public void LoadShoppingList_WhenFileDoesNotExist_ShouldReturnEmptyList()
        {
            // Arrange
            EnsureFileDoesNotExist();

            // Act
            var result = _database.LoadShoppingList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void SaveAndLoadShoppingList_WithValidData_ShouldPreserveAllProperties()
        {
            // Arrange & Act
            _database.SaveShoppingList(_testShoppingList);
            var loadedList = _database.LoadShoppingList();

            // Assert
            Assert.That(loadedList, Has.Count.EqualTo(2));

            var firstItem = loadedList[0];
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.Name, Is.EqualTo("1300g Potatoes"));
                Assert.That(firstItem.Quantity, Is.EqualTo("1"));
                Assert.That(firstItem.IsSelected, Is.True);
            });
        }

        [Test]
        public void SaveShoppingList_WithSingleItem_ShouldMatchExpectedJsonFormat()
        {
            // Arrange
            var singleItemList = new List<ShoppingListItem>
            {
                new ShoppingListItem
                {
                    Name = "1300g Potatoes",
                    Quantity = "1",
                    IsSelected = true
                }
            };

            // Act
            _database.SaveShoppingList(singleItemList);

            // Assert
            Assert.That(File.Exists(_testFilePath));
            var fileContent = File.ReadAllText(_testFilePath);
            var expectedJson = "[\r\n  {\r\n    \"Name\": \"1300g Potatoes\",\r\n    \"Quantity\": \"1\",\r\n    \"IsSelected\": true\r\n  }\r\n]";
            Assert.That(fileContent, Is.EqualTo(expectedJson));
        }

        [Test]
        public void SaveShoppingList_WhenUpdatingExistingItems_ShouldOverwriteOldData()
        {
            // Arrange
            _database.SaveShoppingList(_testShoppingList);

            var updatedList = new List<ShoppingListItem>
            {
                new ShoppingListItem
                {
                    Name = "2000g Rice",
                    Quantity = "1",
                    IsSelected = false
                }
            };

            // Act
            _database.SaveShoppingList(updatedList);
            var loadedList = _database.LoadShoppingList();

            // Assert
            Assert.That(loadedList, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(loadedList[0].Name, Is.EqualTo("2000g Rice"));
                Assert.That(loadedList[0].Quantity, Is.EqualTo("1"));
                Assert.That(loadedList[0].IsSelected, Is.False);
            });
        }

        [Test]
        public void LoadShoppingList_WithInvalidJson_ShouldReturnEmptyList()
        {
            // Arrange
            File.WriteAllText(_testFilePath, "invalid json content");

            // Act
            var result = _database.LoadShoppingList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void SaveShoppingList_WithEmptyList_ShouldCreateEmptyJsonArray()
        {
            // Arrange
            var emptyList = new List<ShoppingListItem>();

            // Act
            _database.SaveShoppingList(emptyList);

            // Assert
            Assert.That(File.Exists(_testFilePath));
            var fileContent = File.ReadAllText(_testFilePath);
            var deserializedList = JsonSerializer.Deserialize<List<ShoppingListItem>>(fileContent);
            Assert.That(deserializedList, Is.Empty);
        }

        [Test]
        public void ToggleItemSelection_ShouldPersistSelectionState()
        {
            // Arrange
            _database.SaveShoppingList(_testShoppingList);
            var loadedList = _database.LoadShoppingList();

            // Act
            loadedList[0].IsSelected = false;
            _database.SaveShoppingList(loadedList);
            var reloadedList = _database.LoadShoppingList();

            // Assert
            Assert.That(reloadedList[0].IsSelected, Is.False);
        }

        [Test]
        public void SaveShoppingList_WithInvalidFilePath_ShouldThrowDirectoryNotFoundException()
        {
            // Arrange
            var invalidPath = Path.Combine("NonExistentDirectory", "shopping_list.json");
            var invalidDatabase = new ShoppingListDatabase(invalidPath);

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() =>
                invalidDatabase.SaveShoppingList(_testShoppingList));
        }

        private void EnsureFileDoesNotExist()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}