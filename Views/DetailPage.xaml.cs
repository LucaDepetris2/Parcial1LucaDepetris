using Parcial1LucaDepetris.ViewModels;

namespace Parcial1LucaDepetris.Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage(DetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
