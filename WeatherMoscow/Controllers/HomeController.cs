using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherMoscow.Models;
using WeatherMoscow.Services.Exceptions;
using WeatherMoscow.Services.Interfaces;

namespace WeatherMoscow.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWeatherService _weatherService;

    public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadData(IFormFileCollection files)
    {
        try
        {
            await _weatherService.UploadData(files);
        }
        catch (UnsupportedTypeException ex)
        {
            return View(new List<string>{ex.Message});
        }
        catch { }
        
        return View(files.Select(file => file.FileName + " successfully uploaded").ToList());
    }

    [HttpGet("availableYears")]
    public IActionResult GetAvailableYears()
    {
        return View(_weatherService.GetAvailableYears());
    }

    public IActionResult GetYearWeather(int year)
    {
        _logger.LogInformation($"Get weather for {year} year");
        return View(_weatherService.GetYearWeather(year));
    }

    public IActionResult GetWeatherForYear(int year)
    {
        _logger.LogInformation($"Get weather for {year} year");
        return View(_weatherService.GetWeatherForYear(year));
    }
    
    public IActionResult GetMonthWeather(int year, string month)
    {
        _logger.LogInformation($"Get weather for {year} - {month} month");
        return View(_weatherService.GetMonthWeather(year, month));
    }
}