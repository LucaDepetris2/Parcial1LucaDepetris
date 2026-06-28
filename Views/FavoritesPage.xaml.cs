using Parcial1LucaDepetris.ViewModels;

namespace Parcial1LucaDepetris.Views
{
    public partial class FavoritesPage : ContentPage
    {
        private readonly FavoritesViewModel _viewModel;

        public FavoritesPage(FavoritesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadFavoritesCommand.Execute(null);
        }
    }
}
