using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CookBook.RecipeManager.GUI.Models
{
    public partial class FavoriteButton : UserControl, INotifyPropertyChanged
    {
        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged(nameof(IsFavorite));
                }
            }
        }

        public string RecipeId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FavoriteButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            IsFavorite = !IsFavorite;
            FavoriteChanged?.Invoke(this, new FavoriteEventArgs(RecipeId, IsFavorite));
        }

        public event FavoriteChangedEventHandler FavoriteChanged;
    }

    public delegate void FavoriteChangedEventHandler(object sender, FavoriteEventArgs e);

    public class FavoriteEventArgs : EventArgs
    {
        public string RecipeId { get; }
        public bool IsFavorite { get; }

        public FavoriteEventArgs(string recipeId, bool isFavorite)
        {
            RecipeId = recipeId;
            IsFavorite = isFavorite;
        }
    }
}