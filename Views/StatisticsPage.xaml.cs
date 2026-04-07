using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage(StatisticsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is StatisticsViewModel vm)
        {
            await vm.LoadStatisticsCommand.ExecuteAsync(null);
        }
    }
}
