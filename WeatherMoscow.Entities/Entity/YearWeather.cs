namespace WeatherMoscow.Entity.Entity;

public class YearWeather
{
    public long Id { get; set; }
    public int Year { get; set; }
    public ICollection<MonthWeather> MonthWeathers { get; set; } = new List<MonthWeather>();
}