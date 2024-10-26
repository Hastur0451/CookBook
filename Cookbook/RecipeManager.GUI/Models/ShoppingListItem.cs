using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CookBook.RecipeManager.GUI.Models
{
    public class ShoppingListItem : INotifyPropertyChanged
    {
        private string _originalText;
        private string _name;
        private string _quantity = "1";
        private bool _isSelected = true;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    UpdateOriginalText();
                }
            }
        }

        public string OriginalText
        {
            get => _originalText;
            set
            {
                _originalText = value;
                ParseIngredient(value);
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ParseIngredient(string ingredient)
        {
            // 使用正则表达式匹配数量和单位
            var match = Regex.Match(ingredient, @"^([\d./]+\s*(?:cup|tsp|tbsp|g|oz|cloves|piece|pieces|)?)\s*(.+)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                Quantity = match.Groups[1].Value.Trim();
                Name = match.Groups[2].Value.Trim();
            }
            else
            {
                Quantity = "1";
                Name = ingredient.Trim();
            }
        }

        private void UpdateOriginalText()
        {
            _originalText = $"{Quantity} {Name}".Trim();
        }

        // 构造函数
        public ShoppingListItem()
        {
        }

        public ShoppingListItem(string ingredient)
        {
            OriginalText = ingredient;
        }
    }
}