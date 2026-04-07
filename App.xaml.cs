namespace CarModelTracker;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        var viewModel = _serviceProvider.GetRequiredService<ViewModels.MainViewModel>();
        MainPage = new NavigationPage(new Views.MainPage(viewModel));
    }
}
