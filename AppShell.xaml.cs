using Parcial1LucaDepetris.Views;

namespace Parcial1LucaDepetris
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("DetailPage", typeof(DetailPage));
        }
    }
}
