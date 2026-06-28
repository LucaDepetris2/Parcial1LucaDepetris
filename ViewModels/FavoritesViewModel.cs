using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;

namespace Parcial1LucaDepetris.ViewModels
{
    public partial class FavoritesViewModel : ObservableObject
    {
        private readonly IFavoriteRepository _repository;

        [ObservableProperty]
        private ObservableCollection<FavoritePost> _favorites = [];

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public FavoritesViewModel(IFavoriteRepository repository)
        {
            _repository = repository;
        }

        [RelayCommand]
        private async Task LoadFavoritesAsync()
        {
            try
            {
                var items = await _repository.GetAllAsync();
                Favorites = new ObservableCollection<FavoritePost>(items);
                StatusMessage = Favorites.Count == 0 ? "No hay favoritos guardados." : string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar favoritos: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task DeleteFavoriteAsync(FavoritePost favorite)
        {
            if (favorite is null) return;
            try
            {
                await _repository.DeleteAsync(favorite.PostId);
                Favorites.Remove(favorite);
                if (Favorites.Count == 0)
                    StatusMessage = "No hay favoritos guardados.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar: {ex.Message}";
            }
        }
    }
}
