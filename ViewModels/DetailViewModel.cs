using CommunityToolkit.Mvvm.ComponentModel;
using Parcial1LucaDepetris.Models;

namespace Parcial1LucaDepetris.ViewModels
{
    [QueryProperty(nameof(Post), "Post")]
    public partial class DetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private Post? _post;
    }
}
