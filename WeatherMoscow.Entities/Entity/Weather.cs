namespace WeatherMoscow.Entity.Entity;

public class Weather
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public decimal Temperature { get; set; }
    public int Humidity { get; set; }
    public decimal DewPoint { get; set; }
    public int Mercury { get; set; }
    public string DirectionOfWind { get; set; }
    public int SpeedOfWind { get; set; }
    public int Cloudiness { get; set; }
    public int LowerCloudLimit { get; set; }
    public int HorizontalVisibility { get; set; }
    public string WeatherConditions { get; set; }
    public long MonthWeatherId { get; set; }
    public MonthWeather MonthWeather { get; set; }
}