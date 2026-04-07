using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel? _viewModel;

    public StatisticsPage()
    {
        var db = Handler.MauiContext?.Services.GetRequiredService<IStatisticsService>();
        if (db != null)
        {
            _viewModel = new StatisticsViewModel(db);
            BindingContext = _viewModel;
        }
        InitializeComponent();
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
