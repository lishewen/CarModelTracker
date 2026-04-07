using CarModelTracker.ViewModels;

namespace CarModelTracker.Views;

public partial class HomePage : ContentPage
{
    private HomeViewModel? _viewModel;

    public HomePage()
    {
        InitializeComponent();
    }

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (_viewModel == null && BindingContext is HomeViewModel vm)
        {
            _viewModel = vm;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            _viewModel.OnCarModelSaved();
        }
    }
}
