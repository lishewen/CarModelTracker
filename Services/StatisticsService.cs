using CarModelTracker.Models;

namespace CarModelTracker.Services;

public class StatisticsResult
{
    public int TotalCount { get; set; }
    public decimal TotalValue { get; set; }
    public Dictionary<string, int> BrandCounts { get; set; } = new();
    public Dictionary<string, int> ScaleCounts { get; set; } = new();
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

        // 按比例分组
        foreach (var group in models.GroupBy(x => x.Scale))
        {
            result.ScaleCounts[group.Key] = group.Sum(x => x.Quantity);
        }

        return result;
    }
}
