using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class CarModelPage : ContentPage
{
    private readonly CarModelViewModel _viewModel;

    public CarModelPage(CarModelViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}
