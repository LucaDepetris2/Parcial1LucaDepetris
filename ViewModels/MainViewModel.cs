using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Parcial1LucaDepetris.Models;
using Parcial1LucaDepetris.Services;

namespace Parcial1LucaDepetris.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPostService _postService;

        [ObservableProperty]
        private ObservableCollection<Post> _posts = [];

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool _isLoading;

        public bool IsNotLoading => !IsLoading;

        public MainViewModel(IPostService postService)
        {
            _postService = postService;
        }

        [RelayCommand]
        private async Task LoadPostsAsync()
        {
            IsLoading = true;
            StatusMessage = "Cargando datos...";
            Posts.Clear();

            try
            {
                var result = await _postService.GetPostsAsync();
                foreach (var post in result)
                    Posts.Add(post);

                StatusMessage = $"{Posts.Count} posts cargados correctamente.";
            }
            catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
            {
                int code = (int)ex.StatusCode!.Value;
                StatusMessage = code switch
                {
                    404 => "Error 404: El recurso no fue encontrado en el servidor.",
                    500 => "Error 500: El servidor encontró un error interno.",
                    _   => $"Error HTTP {code}: respuesta inesperada del servidor."
                };
            }
            catch (HttpRequestException)
            {
                StatusMessage = "Error de red: no se pudo conectar al servidor. Verifique su conexión.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectPostAsync(Post post)
        {
            if (post is null) return;

            var parameters = new Dictionary<string, object>
            {
                { "Post", post }
            };

            await Shell.Current.GoToAsync("DetailPage", parameters);
        }
    }
}
