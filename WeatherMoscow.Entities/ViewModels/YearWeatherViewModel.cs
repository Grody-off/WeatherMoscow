namespace WeatherMoscow.Entity.ViewModels;

public class YearWeatherViewModel
{
    public int Year { get; set; }
    public List<MonthWeatherViewModel> MonthWeather { get; set; } = new();
}