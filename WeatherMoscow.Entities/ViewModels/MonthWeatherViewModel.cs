namespace WeatherMoscow.Entity.ViewModels;

public class MonthWeatherViewModel
{
    public string Month { get; set; }
    public List<WeatherViewModel> Weather { get; set; }
}