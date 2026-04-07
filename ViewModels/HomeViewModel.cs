using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarModelTracker.Models;
using CarModelTracker.Services;
using CarModelTracker.Views;

namespace CarModelTracker.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;
    private readonly IStatisticsService _statisticsService;

    [ObservableProperty]
    private ObservableCollection<CarModel> _carModels = new();

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private int _totalCount;

    [ObservableProperty]
    private decimal _totalValue;

    [ObservableProperty]
    private int _brandCount;

    [ObservableProperty]
    private int _weekAdded;

    [ObservableProperty]
    private List<string> _scales = new();

    [ObservableProperty]
    private bool _isRefreshing;

    public HomeViewModel(
        IDatabaseService databaseService,
        IStatisticsService statisticsService)
    {
        _databaseService = databaseService;
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
            
            // 更新统计数据
            await UpdateStatisticsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载失败：{ex.Message}");
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"搜索失败：{ex.Message}");
        }
    }

    private async Task UpdateStatisticsAsync()
    {
        try
        {
            var stats = await _statisticsService.GetStatisticsAsync();
            TotalCount = stats.TotalCount;
            TotalValue = stats.TotalValue;
            BrandCount = stats.BrandCount;
            
            // 计算本周新增
            var weekAgo = DateTime.Now.AddDays(-7);
            WeekAdded = CarModels.Count(x => x.CreatedAt >= weekAgo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"统计更新失败：{ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AddNewAsync()
    {
        if (Application.Current?.MainPage is Shell shell)
        {
            var viewModel = new CarModelViewModel(_databaseService, null);
            var page = new CarModelPage(viewModel);
            await shell.Navigation.PushAsync(page);
        }
    }

    [RelayCommand]
    private async Task EditAsync(CarModel? model)
    {
        if (model == null) return;

        if (Application.Current?.MainPage is Shell shell)
        {
            var viewModel = new CarModelViewModel(_databaseService, model);
            var page = new CarModelPage(viewModel);
            await shell.Navigation.PushAsync(page);
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(CarModel? model)
    {
        if (model == null) return;

        var confirm = await Application.Current.MainPage.DisplayAlertAsync(
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
                await UpdateStatisticsAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlertAsync("删除失败", ex.Message, "确定");
            }
        }
    }

    public void OnCarModelSaved()
    {
        LoadDataCommand.Execute(null);
    }
}
