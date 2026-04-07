using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class CarModelPage : ContentPage
{
    public CarModelPage(CarModelViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
