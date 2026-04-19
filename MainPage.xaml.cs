using Parcial1LucaDepetris.ViewModels;

namespace Parcial1LucaDepetris
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
