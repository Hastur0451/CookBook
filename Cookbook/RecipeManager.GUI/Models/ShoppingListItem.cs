using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CookBook.RecipeManager.GUI.Models
{
    public class ShoppingListItem : INotifyPropertyChanged
    {
        private string _name;
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

        public ShoppingListItem()
        {
        }

        public ShoppingListItem(string ingredient)
        {
            Name = ingredient;
            IsSelected = true;
        }
    }
}