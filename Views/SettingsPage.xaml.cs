using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel? _viewModel;

    public SettingsPage()
    {
        InitializeComponent();
        
        // 通过 DI 容器解析 ViewModel
        if (Application.Current is IPlatformApplication app)
        {
            _viewModel = app.Services.GetRequiredService<SettingsViewModel>();
            BindingContext = _viewModel;
        }
    }

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}
