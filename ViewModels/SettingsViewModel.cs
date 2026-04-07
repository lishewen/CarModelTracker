using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarModelTracker.ViewModels;

public partial class ExportFields : ObservableObject
{
    [ObservableProperty]
    private bool _name = true;

    [ObservableProperty]
    private bool _brand = true;

    [ObservableProperty]
    private bool _model = true;

    [ObservableProperty]
    private bool _scale = true;

    [ObservableProperty]
    private bool _quantity = true;

    [ObservableProperty]
    private bool _price = true;

    [ObservableProperty]
    private bool _totalPrice = true;

    [ObservableProperty]
    private bool _purchaseDate = true;
}

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private ExportFields _exportFields = new();

    [RelayCommand]
    private void SelectAllFields()
    {
        ExportFields.Name = true;
        ExportFields.Brand = true;
        ExportFields.Model = true;
        ExportFields.Scale = true;
        ExportFields.Quantity = true;
        ExportFields.Price = true;
        ExportFields.TotalPrice = true;
        ExportFields.PurchaseDate = true;
    }

    [RelayCommand]
    private void DeselectAllFields()
    {
        ExportFields.Name = false;
        ExportFields.Brand = false;
        ExportFields.Model = false;
        ExportFields.Scale = false;
        ExportFields.Quantity = false;
        ExportFields.Price = false;
        ExportFields.TotalPrice = false;
        ExportFields.PurchaseDate = false;
    }

    [RelayCommand]
    private async Task ExportAsync()
    {
        await Application.Current.MainPage.DisplayAlert("导出", "导出功能开发中...", "确定");
    }
}
