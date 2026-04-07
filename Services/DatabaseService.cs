using CarModelTracker.Models;
using SQLite;

namespace CarModelTracker.Services;

public interface IDatabaseService
{
    Task<List<CarModel>> GetAllAsync();
    Task<List<CarModel>> SearchAsync(string searchTerm);
    Task<List<CarModel>> FilterByScaleAsync(string scale);
    Task<CarModel?> GetByIdAsync(int id);
    Task<int> SaveAsync(CarModel carModel);
    Task<int> DeleteAsync(int id);
    Task<List<string>> GetBrandsAsync();
    Task<List<string>> GetScalesAsync();
}

public class DatabaseService : IDatabaseService
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized = false;

    public DatabaseService()
    {
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CarModels.db3");

        _database = new SQLiteAsyncConnection(dbPath);
    }

    private async Task EnsureInitializedAsync()
    {
        if (!_initialized)
        {
            await _database.CreateTableAsync<CarModel>();
            _initialized = true;
        }
    }

    public async Task<List<CarModel>> GetAllAsync()
    {
        await EnsureInitializedAsync();
        var results = await _database.Table<CarModel>()
            .OrderByDescending(x => x.PurchaseDate)
            .ToListAsync();
        return results;
    }

    public async Task<List<CarModel>> SearchAsync(string searchTerm)
    {
        await EnsureInitializedAsync();
        var results = await _database.Table<CarModel>()
            .Where(x => x.Brand.Contains(searchTerm) || x.ModelName.Contains(searchTerm))
            .OrderByDescending(x => x.PurchaseDate)
            .ToListAsync();
        return results;
    }

    public async Task<List<CarModel>> FilterByScaleAsync(string scale)
    {
        await EnsureInitializedAsync();
        var results = await _database.Table<CarModel>()
            .Where(x => x.Scale == scale)
            .OrderByDescending(x => x.PurchaseDate)
            .ToListAsync();
        return results;
    }

    public async Task<CarModel?> GetByIdAsync(int id)
    {
        await EnsureInitializedAsync();
        return await _database.FindAsync<CarModel>(id);
    }

    public async Task<int> SaveAsync(CarModel carModel)
    {
        await EnsureInitializedAsync();
        carModel.UpdatedAt = DateTime.Now;

        if (carModel.Id == 0)
        {
            carModel.CreatedAt = DateTime.Now;
            return await _database.InsertAsync(carModel);
        }
        else
        {
            return await _database.UpdateAsync(carModel);
        }
    }

    public async Task<int> DeleteAsync(int id)
    {
        await EnsureInitializedAsync();
        return await _database.DeleteAsync<CarModel>(id);
    }

    public async Task<List<string>> GetBrandsAsync()
    {
        await EnsureInitializedAsync();
        var all = await GetAllAsync();
        return all.Select(x => x.Brand).Distinct().OrderBy(x => x).ToList();
    }

    public async Task<List<string>> GetScalesAsync()
    {
        await EnsureInitializedAsync();
        var all = await GetAllAsync();
        return all.Select(x => x.Scale).Distinct().OrderBy(x => x).ToList();
    }
}
