using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using WeatherMoscow.Entity.Entity;
using WeatherMoscow.Services.Exceptions;

namespace WeatherMoscow.Services;

public static class ExcelParser
{
    private static IWorkbook Workbook;
    private const int RowStartIndex = 4;

    public static async Task<YearWeather> GetExcelDataTable(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        if (extension != ".xls" && extension != ".xlsx")
            throw new UnsupportedTypeException($"{file.FileName} has unsuppordet type");

        await using var stream = file.OpenReadStream();
        Workbook = WorkbookFactory.Create(stream);


        var yearWeather = new YearWeather();

        for (var i = 0; i < Workbook.NumberOfSheets; i++)
        {
            var sheet = Workbook.GetSheetAt(i);
            var monthWeather = new MonthWeather();

            var sheetName = sheet.SheetName.Split(" ");
            const int monthIndex = 0;
            const int yearIndex = 1;

            for (var row = RowStartIndex; row <= sheet.LastRowNum; row++)
            {
                var currentRow = sheet.GetRow(row);
                if (currentRow != null)
                {
                    monthWeather.Month = sheetName[monthIndex];
                    monthWeather.Weathers.Add(GetWeather(currentRow));
                }
            }

            yearWeather.Year = Convert.ToInt32(sheetName[yearIndex]);
            yearWeather.MonthWeathers.Add(monthWeather);
        }

        return yearWeather;
    }

    private static Weather GetWeather(IRow currentRow)
    {
        var weather = new Weather
        {
            Date = DateTime.TryParse(GetCellValue(currentRow.GetCell(0)), out var date) ? date : new DateTime(),
            Time = DateTime.TryParse(GetCellValue(currentRow.GetCell(1)), out var time) ? time : new DateTime(),
            Temperature = decimal.TryParse(GetCellValue(currentRow.GetCell(2)), out var temperature) ? temperature : 0m,
            Humidity = int.TryParse(GetCellValue(currentRow.GetCell(3)), out var humidity) ? humidity : 0,
            DewPoint = decimal.TryParse(GetCellValue(currentRow.GetCell(4)), out var dewPoint) ? dewPoint : 0m,
            Mercury = int.TryParse(GetCellValue(currentRow.GetCell(5)), out var mercury) ? mercury : 0,
            DirectionOfWind = GetCellValue(currentRow.GetCell(6)),
            SpeedOfWind = int.TryParse(GetCellValue(currentRow.GetCell(7)), out var speedOfWind) ? speedOfWind : 0,
            Cloudiness = int.TryParse(GetCellValue(currentRow.GetCell(8)), out var cloudiness) ? cloudiness : 0,
            LowerCloudLimit = int.TryParse(GetCellValue(currentRow.GetCell(9)), out var lowerCloudLimit) ? lowerCloudLimit : 0,
            HorizontalVisibility = int.TryParse(GetCellValue(currentRow.GetCell(10)), out var horizontalVisibility) ? horizontalVisibility : 0,
            WeatherConditions = GetCellValue(currentRow.GetCell(11))
        };
        
        return weather;
    }
    private static string GetCellValue(ICell cell)
    {
        if (cell == null)
        {
            return string.Empty;
        }

        switch (cell.CellType)
        {
            case CellType.Blank:
                return string.Empty;
            case CellType.Boolean:
                return cell.BooleanCellValue.ToString();
            case CellType.Error:
                return cell.ErrorCellValue.ToString();
            case CellType.Numeric:
            case CellType.Unknown:
            default:
                return cell.ToString();
            case CellType.String:
                return cell.StringCellValue;
            case CellType.Formula:
                try
                {
                    var formulaEvaluator = new HSSFFormulaEvaluator(cell.Sheet.Workbook);

                    formulaEvaluator.EvaluateInCell(cell);
                    return cell.ToString();
                }
                catch
                {
                    return cell.NumericCellValue.ToString();
                }
        }
    }
}