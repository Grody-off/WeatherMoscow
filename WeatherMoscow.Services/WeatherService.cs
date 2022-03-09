using Microsoft.AspNetCore.Http;
using WeatherMoscow.DataAccess.Interfaces;
using WeatherMoscow.Entity.Entity;
using WeatherMoscow.Entity.ViewModels;
using WeatherMoscow.Services.Interfaces;

namespace WeatherMoscow.Services;

public class WeatherService : IWeatherService
{
    private readonly IRepository<YearWeather> _yearWeatherRepository;
    private readonly IRepository<MonthWeather> _monthWeatherRepository;
    

    public WeatherService(IRepository<YearWeather> yearWeatherRepository, IRepository<MonthWeather> monthWeatherRepository)
    {
        _yearWeatherRepository = yearWeatherRepository;
        _monthWeatherRepository = monthWeatherRepository;
    }
    public List<int> GetAvailableYears()
    {
        return _yearWeatherRepository.GetAll().Select(weather => weather.Year).ToList();
    }
    public YearWeatherViewModel GetYearWeather(int year)
    {
        var monthsWeather = _monthWeatherRepository.Include(weather => weather.Weathers,
                                                    weather => weather.YearWeather)
                                                    .Where(weather => weather.YearWeather.Year == year).ToList();

        var yearWeather = new YearWeatherViewModel
        {
            Year = year,
            MonthWeather = monthsWeather.Select(monthWeather => new MonthWeatherViewModel
            {
                Month = monthWeather.Month,
                Weather = GetWeather(monthWeather)
            }).ToList()
        };

        return yearWeather;
    }
    public MonthWeatherViewModel GetMonthWeather(int year, string month)
    {
        var monthsWeather = _monthWeatherRepository.Include(weather => weather.Weathers,
                weather => weather.YearWeather)
            .FirstOrDefault(weather => weather.YearWeather.Year == year
                    && weather.Month == month);

        if (monthsWeather == null)
        {
            return new MonthWeatherViewModel();
        }

        var monthWeather = new MonthWeatherViewModel
        {
            Month = monthsWeather.Month,
            Weather = GetWeather(monthsWeather),
        };

        return monthWeather;
    }
    private List<WeatherViewModel> GetWeather(MonthWeather weathers)
    {
        return weathers.Weathers.Select(weather => new WeatherViewModel
        {
            Date = weather.Date,
            Time = weather.Time,
            Temperature = weather.Temperature,
            Humidity = weather.Humidity,
            DewPoint = weather.DewPoint,
            Mercury = weather.Mercury,
            DirectionOfWind = weather.DirectionOfWind,
            SpeedOfWind = weather.SpeedOfWind,
            Cloudiness = weather.Cloudiness,
            LowerCloudLimit = weather.LowerCloudLimit,
            HorizontalVisibility = weather.HorizontalVisibility,
            WeatherConditions = weather.WeatherConditions
        }).ToList();
    }
    public List<WeatherViewModel> GetWeatherForYear(int year)
    {
        var yearWeather = _monthWeatherRepository.Include(weather => weather.Weathers,
                weather => weather.YearWeather).Where(weather => weather.YearWeather.Year == year).ToList();

        var weather = new List<WeatherViewModel>();
        foreach (var monthWeather in yearWeather)
        {
            weather.AddRange(GetWeather(monthWeather));
        }

        return weather;
    }

    public async Task UploadData(IFormFileCollection files)
    {
        var data = new List<YearWeather>();
        var tasks = files.Select(async file =>
        {
            var yearWeather = await ExcelParser.GetExcelDataTable(file);
            data.Add(yearWeather);
        });
        await Task.WhenAll(tasks);
        await _yearWeatherRepository.CreateRangeAsync(data);
    }
}