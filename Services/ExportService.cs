using CarModelTracker.Models;
using ClosedXML.Excel;

namespace CarModelTracker.Services;

public interface IExportService
{
    Task<string> ExportToExcelAsync(List<CarModel> models);
}

public class ExportService : IExportService
{
    public async Task<string> ExportToExcelAsync(List<CarModel> models)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("车模收藏");

        // 设置表头样式
        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // 添加表头
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "品牌";
        worksheet.Cell(1, 3).Value = "车型名称";
        worksheet.Cell(1, 4).Value = "比例";
        worksheet.Cell(1, 5).Value = "颜色";
        worksheet.Cell(1, 6).Value = "购买日期";
        worksheet.Cell(1, 7).Value = "购买价格";
        worksheet.Cell(1, 8).Value = "数量";
        worksheet.Cell(1, 9).Value = "总价值";
        worksheet.Cell(1, 10).Value = "备注";

        // 填充数据
        var row = 2;
        foreach (var model in models)
        {
            worksheet.Cell(row, 1).Value = model.Id;
            worksheet.Cell(row, 2).Value = model.Brand;
            worksheet.Cell(row, 3).Value = model.ModelName;
            worksheet.Cell(row, 4).Value = model.Scale;
            worksheet.Cell(row, 5).Value = model.Color;
            worksheet.Cell(row, 6).Value = model.PurchaseDate.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 7).Value = model.PurchasePrice;
            worksheet.Cell(row, 7).Style.NumberFormat.Format = "¥#,##0.00";
            worksheet.Cell(row, 8).Value = model.Quantity;
            worksheet.Cell(row, 9).Value = model.TotalValue;
            worksheet.Cell(row, 9).Style.NumberFormat.Format = "¥#,##0.00";
            worksheet.Cell(row, 10).Value = model.Notes;
            row++;
        }

        // 自动调整列宽
        worksheet.Columns().AdjustToContents();

        // 保存到 Downloads 目录
        var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var fileName = $"车模收藏_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        var filePath = Path.Combine(downloadsPath, "Downloads", fileName);

        // 确保 Downloads 目录存在
        var downloadsDir = Path.Combine(downloadsPath, "Downloads");
        if (!Directory.Exists(downloadsDir))
        {
            Directory.CreateDirectory(downloadsDir);
        }

        await Task.Run(() => workbook.SaveAs(filePath));

        return filePath;
    }
}
