namespace Parcial1LucaDepetris.Services
{
    public class MauiNavigationService : INavigationService
    {
        public Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            return parameters is null
                ? Shell.Current.GoToAsync(route)
                : Shell.Current.GoToAsync(route, parameters);
        }
    }
}
