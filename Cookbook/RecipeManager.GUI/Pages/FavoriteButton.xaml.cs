using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CookBook.RecipeManager.GUI.Models
{
    public partial class FavoriteButton : UserControl, INotifyPropertyChanged
    {
        // Tracks the favorite state of the button
        private bool _isFavorite;

        // Property to get or set the favorite status
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged(nameof(IsFavorite)); // Notify of property change
                }
            }
        }

        // Property to hold the recipe ID associated with this button
        public string RecipeId { get; set; }

        // Event for notifying property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor to initialize the FavoriteButton
        public FavoriteButton()
        {
            InitializeComponent();
            DataContext = this; // Set data context for binding
        }

        // Event handler for button click to toggle favorite status
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            IsFavorite = !IsFavorite; // Toggle favorite status
            FavoriteChanged?.Invoke(this, new FavoriteEventArgs(RecipeId, IsFavorite)); // Trigger FavoriteChanged event
        }

        // Event for notifying when favorite status changes
        public event FavoriteChangedEventHandler FavoriteChanged;
    }

    // Delegate for handling favorite status change events
    public delegate void FavoriteChangedEventHandler(object sender, FavoriteEventArgs e);

    // Custom event arguments for FavoriteButton events
    public class FavoriteEventArgs : EventArgs
    {
        // ID of the associated recipe
        public string RecipeId { get; }

        // New favorite status
        public bool IsFavorite { get; }

        // Constructor to initialize event arguments
        public FavoriteEventArgs(string recipeId, bool isFavorite)
        {
            RecipeId = recipeId;
            IsFavorite = isFavorite;
        }
    }
}
