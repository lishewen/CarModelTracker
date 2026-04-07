using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarModelTracker.Models;
using CarModelTracker.Services;

namespace CarModelTracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;
    private readonly IExportService _exportService;
    private readonly IStatisticsService _statisticsService;

    [ObservableProperty]
    private ObservableCollection<CarModel> _carModels = new();

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private string _selectedScaleFilter = string.Empty;

    [ObservableProperty]
    private List<string> _scales = new();

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    public MainViewModel(
        IDatabaseService databaseService,
        IExportService exportService,
        IStatisticsService statisticsService)
    {
        _databaseService = databaseService;
        _exportService = exportService;
        _statisticsService = statisticsService;
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsRefreshing = true;
        try
        {
            var models = await _databaseService.GetAllAsync();
            CarModels.Clear();
            foreach (var model in models)
            {
                CarModels.Add(model);
            }

            Scales = await _databaseService.GetScalesAsync();
            StatusMessage = $"共 {CarModels.Count} 条记录";
        }
        catch (Exception ex)
        {
            StatusMessage = $"加载失败：{ex.Message}";
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        try
        {
            var models = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _databaseService.GetAllAsync()
                : await _databaseService.SearchAsync(SearchTerm);

            CarModels.Clear();
            foreach (var model in models)
            {
                CarModels.Add(model);
            }

            StatusMessage = $"找到 {CarModels.Count} 条记录";
        }
        catch (Exception ex)
        {
            StatusMessage = $"搜索失败：{ex.Message}";
        }
    }

    [RelayCommand]
    private async Task FilterByScaleAsync()
    {
        try
        {
            var models = string.IsNullOrWhiteSpace(SelectedScaleFilter)
                ? await _databaseService.GetAllAsync()
                : await _databaseService.FilterByScaleAsync(SelectedScaleFilter);

            CarModels.Clear();
            foreach (var model in models)
            {
                CarModels.Add(model);
            }

            StatusMessage = $"找到 {CarModels.Count} 条记录";
        }
        catch (Exception ex)
        {
            StatusMessage = $"筛选失败：{ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ExportAsync()
    {
        try
        {
            var filePath = await _exportService.ExportToExcelAsync(CarModels.ToList());
            StatusMessage = $"已导出到：{filePath}";
            await App.Current.MainPage.DisplayAlert("导出成功", $"文件已保存到:\n{filePath}", "确定");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("导出失败", ex.Message, "确定");
        }
    }

    [RelayCommand]
    private async Task NavigateToStatisticsAsync()
    {
        var statsViewModel = new StatisticsViewModel(_statisticsService);
        await App.Current.MainPage.Navigation.PushAsync(new Views.StatisticsPage(statsViewModel));
    }

    [RelayCommand]
    private async Task AddNewAsync()
    {
        var viewModel = new CarModelViewModel(_databaseService, null);
        var page = new Views.CarModelPage(viewModel);
        await App.Current.MainPage.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task EditAsync(CarModel? model)
    {
        if (model == null) return;

        var viewModel = new CarModelViewModel(_databaseService, model);
        var page = new Views.CarModelPage(viewModel);
        await App.Current.MainPage.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task DeleteAsync(CarModel? model)
    {
        if (model == null) return;

        var confirm = await App.Current.MainPage.DisplayAlert(
            "确认删除",
            $"确定要删除 \"{model.Brand} - {model.ModelName}\" 吗？",
            "删除",
            "取消");

        if (confirm)
        {
            try
            {
                await _databaseService.DeleteAsync(model.Id);
                CarModels.Remove(model);
                StatusMessage = "删除成功";
            }
            catch (Exception ex)
            {
                StatusMessage = $"删除失败：{ex.Message}";
                await App.Current.MainPage.DisplayAlert("删除失败", ex.Message, "确定");
            }
        }
    }

    public void OnCarModelSaved()
    {
        LoadDataCommand.Execute(null);
    }
}
