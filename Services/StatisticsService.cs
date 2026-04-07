using CarModelTracker.Models;

namespace CarModelTracker.Services;

public class StatisticsResult
{
    public int TotalCount { get; set; }
    public decimal TotalValue { get; set; }
    public Dictionary<string, int> BrandCounts { get; set; } = new();
    public Dictionary<string, int> ScaleCounts { get; set; } = new();
    
    // 快速统计
    public decimal AvgPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public DateTime? EarliestDate { get; set; }
    public int BrandCount { get; set; }
    public string MostScale { get; set; } = string.Empty;
}

public interface IStatisticsService
{
    Task<StatisticsResult> GetStatisticsAsync();
}

public class StatisticsService : IStatisticsService
{
    private readonly IDatabaseService _databaseService;

    public StatisticsService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<StatisticsResult> GetStatisticsAsync()
    {
        var models = await _databaseService.GetAllAsync();
        var result = new StatisticsResult
        {
            TotalCount = models.Sum(x => x.Quantity),
            TotalValue = models.Sum(x => x.TotalValue)
        };

        // 按品牌分组
        foreach (var group in models.GroupBy(x => x.Brand))
        {
            result.BrandCounts[group.Key] = group.Sum(x => x.Quantity);
        }
        result.BrandCount = result.BrandCounts.Count;

        // 按比例分组
        foreach (var group in models.GroupBy(x => x.Scale))
        {
            result.ScaleCounts[group.Key] = group.Sum(x => x.Quantity);
        }
        
        // 最多的比例
        if (result.ScaleCounts.Count > 0)
        {
            result.MostScale = result.ScaleCounts.OrderByDescending(x => x.Value).First().Key;
        }

        // 平均价格
        if (models.Count > 0)
        {
            result.AvgPrice = models.Average(x => x.PurchasePrice);
            result.MinPrice = models.Min(x => x.PurchasePrice);
            result.MaxPrice = models.Max(x => x.PurchasePrice);
            result.EarliestDate = models.Min(x => x.PurchaseDate);
        }

        return result;
    }
}
