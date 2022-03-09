using Microsoft.AspNetCore.Http;
using WeatherMoscow.Entity.ViewModels;

namespace WeatherMoscow.Services.Interfaces;

public interface IWeatherService
{
    public List<int> GetAvailableYears();
    public YearWeatherViewModel GetYearWeather(int year);
    public MonthWeatherViewModel GetMonthWeather(int year, string month);
    public List<WeatherViewModel> GetWeatherForYear(int year);
    public Task UploadData(IFormFileCollection files);
}