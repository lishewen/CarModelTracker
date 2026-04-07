using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarModelTracker.Models;
using CarModelTracker.Services;

namespace CarModelTracker.ViewModels;

public partial class CarModelViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;
    private readonly CarModel? _originalModel;

    [ObservableProperty]
    private string _brand = string.Empty;

    [ObservableProperty]
    private string _modelName = string.Empty;

    [ObservableProperty]
    private string _scale = "1:18";

    [ObservableProperty]
    private string _color = string.Empty;

    [ObservableProperty]
    private DateTime _purchaseDate = DateTime.Today;

    [ObservableProperty]
    private decimal _purchasePrice;

    [ObservableProperty]
    private int _quantity = 1;

    [ObservableProperty]
    private string _notes = string.Empty;

    [ObservableProperty]
    private List<string> _brands = new();

    [ObservableProperty]
    private List<string> _scales = new()
    {
        "1:10", "1:12", "1:18", "1:24", "1:32", "1:43", "1:64", "1:76"
    };

    public CarModelViewModel(IDatabaseService databaseService, CarModel? model)
    {
        _databaseService = databaseService;
        _originalModel = model;

        if (model != null)
        {
            Brand = model.Brand;
            ModelName = model.ModelName;
            Scale = model.Scale;
            Color = model.Color;
            PurchaseDate = model.PurchaseDate;
            PurchasePrice = model.PurchasePrice;
            Quantity = model.Quantity;
            Notes = model.Notes;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Brand))
        {
            await App.Current.MainPage.DisplayAlert("验证失败", "请输入品牌", "确定");
            return;
        }

        if (string.IsNullOrWhiteSpace(ModelName))
        {
            await App.Current.MainPage.DisplayAlert("验证失败", "请输入车型名称", "确定");
            return;
        }

        try
        {
            var carModel = _originalModel ?? new CarModel();
            carModel.Brand = Brand;
            carModel.ModelName = ModelName;
            carModel.Scale = Scale;
            carModel.Color = Color;
            carModel.PurchaseDate = PurchaseDate;
            carModel.PurchasePrice = PurchasePrice;
            carModel.Quantity = Quantity;
            carModel.Notes = Notes;

            await _databaseService.SaveAsync(carModel);

            if (App.Current.MainPage.Navigation.NavigationStack.LastOrDefault() is Views.CarModelPage page)
            {
                await page.Navigation.PopAsync();
            }

            // 通知主页面刷新数据
            if (App.Current.MainPage is NavigationPage navPage &&
                navPage.CurrentPage is Views.MainPage mainPage &&
                mainPage.BindingContext is MainViewModel mainViewModel)
            {
                mainViewModel.OnCarModelSaved();
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("保存失败", ex.Message, "确定");
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        if (App.Current.MainPage.Navigation.NavigationStack.LastOrDefault() is Views.CarModelPage page)
        {
            await page.Navigation.PopAsync();
        }
    }
}
