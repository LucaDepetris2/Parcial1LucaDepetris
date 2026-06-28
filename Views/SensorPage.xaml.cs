using Parcial1LucaDepetris.ViewModels;

namespace Parcial1LucaDepetris.Views
{
    public partial class SensorPage : ContentPage
    {
        public SensorPage(SensorViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
