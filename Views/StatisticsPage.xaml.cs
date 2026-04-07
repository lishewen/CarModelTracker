using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class StatisticsPage : ContentPage
{
    private StatisticsViewModel? _viewModel;

    public StatisticsPage()
    {
        InitializeComponent();
        
        // 通过 DI 容器解析 ViewModel
        if (Application.Current is IPlatformApplication app)
        {
            _viewModel = app.Services.GetRequiredService<StatisticsViewModel>();
            BindingContext = _viewModel;
        }
    }

    public StatisticsPage(StatisticsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.LoadStatisticsCommand.ExecuteAsync(null);
        }
    }
}
