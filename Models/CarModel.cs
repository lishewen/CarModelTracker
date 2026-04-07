using SQLite;

namespace CarModelTracker.Models;

public class CarModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// 品牌 (AutoArt/Minichamps 等)
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// 车型名称
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 比例 (1:18/1:43/1:64 等)
    /// </summary>
    public string Scale { get; set; } = "1:18";

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// 购买日期
    /// </summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// 购买价格
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// 备注
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 总价值
    /// </summary>
    [Ignore]
    public decimal TotalValue => PurchasePrice * Quantity;
}
