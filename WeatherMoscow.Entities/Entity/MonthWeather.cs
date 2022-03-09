using System.ComponentModel.DataAnnotations;
using WeatherMoscow.Entity.Enums;

namespace WeatherMoscow.Entity.Entity;

public class MonthWeather
{
    public long Id { get; set; }
    
    [EnumDataType(typeof(Month))]
    public string Month { get; set; }
    public ICollection<Weather> Weathers { get; set; } = new List<Weather>();
    public long YearId { get; set; }
    public YearWeather YearWeather { get; set; }
}