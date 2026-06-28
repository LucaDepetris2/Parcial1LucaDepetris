using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;

namespace Parcial1LucaDepetris.ViewModels
{
#if MAUI_BUILD
    [QueryProperty(nameof(Post), "Post")]
#endif
    public partial class DetailViewModel : ObservableObject
    {
        private readonly IFavoriteRepository _repository;

        [ObservableProperty]
        private Post? _post;

        [ObservableProperty]
        private string _saveStatus = string.Empty;

        public DetailViewModel(IFavoriteRepository repository)
        {
            _repository = repository;
        }

        [RelayCommand]
        private async Task SaveFavoriteAsync()
        {
            if (Post is null) return;
            try
            {
                await _repository.SaveAsync(FavoritePost.FromPost(Post));
                SaveStatus = "Guardado en favoritos.";
            }
            catch (Exception ex)
            {
                SaveStatus = $"Error: {ex.Message}";
            }
        }
    }
}
