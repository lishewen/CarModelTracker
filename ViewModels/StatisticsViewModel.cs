using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarModelTracker.Services;

namespace CarModelTracker.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    private readonly IStatisticsService _statisticsService;

    [ObservableProperty]
    private int _totalCount;

    [ObservableProperty]
    private decimal _totalValue;

    [ObservableProperty]
    private string _brandStatistics = string.Empty;

    [ObservableProperty]
    private string _scaleStatistics = string.Empty;
    
    // 快速统计属性
    [ObservableProperty]
    private decimal _avgPrice;
    
    [ObservableProperty]
    private decimal _minPrice;
    
    [ObservableProperty]
    private decimal _maxPrice;
    
    [ObservableProperty]
    private string _earliestDate = string.Empty;
    
    [ObservableProperty]
    private int _brandCount;
    
    [ObservableProperty]
    private string _mostScale = string.Empty;
    
    [ObservableProperty]
    private int _weekAdded;

    [ObservableProperty]
    private bool _isRefreshing;

    public StatisticsViewModel(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [RelayCommand]
    private async Task LoadStatisticsAsync()
    {
        IsRefreshing = true;
        try
        {
            var stats = await _statisticsService.GetStatisticsAsync();

            TotalCount = stats.TotalCount;
            TotalValue = stats.TotalValue;
            
            // 快速统计
            AvgPrice = stats.AvgPrice;
            MinPrice = stats.MinPrice;
            MaxPrice = stats.MaxPrice;
            EarliestDate = stats.EarliestDate.HasValue ? stats.EarliestDate.Value.ToString("yy 年") : "-";
            BrandCount = stats.BrandCount;
            MostScale = stats.MostScale;

            // 品牌统计
            var brandText = new System.Text.StringBuilder();
            if (stats.BrandCounts.Count == 0)
            {
                BrandStatistics = "暂无数据";
            }
            else
            {
                foreach (var kvp in stats.BrandCounts.OrderByDescending(x => x.Value))
                {
                    brandText.AppendLine($"{kvp.Key}: {kvp.Value} 个");
                }
                BrandStatistics = brandText.ToString();
            }

            // 比例统计
            var scaleText = new System.Text.StringBuilder();
            if (stats.ScaleCounts.Count == 0)
            {
                ScaleStatistics = "暂无数据";
            }
            else
            {
                foreach (var kvp in stats.ScaleCounts.OrderByDescending(x => x.Value))
                {
                    scaleText.AppendLine($"{kvp.Key}: {kvp.Value} 个");
                }
                ScaleStatistics = scaleText.ToString();
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("加载失败", ex.Message, "确定");
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}
