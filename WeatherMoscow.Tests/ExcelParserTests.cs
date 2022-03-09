using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using WeatherMoscow.Services;

namespace WeatherMoscow.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SendFile_WhenAllCorrect_ShouldPass()
    {
        var path = @"C:\Users\Lenovo\OneDrive\Рабочий стол\Weather.Moscow.2010-2014\Weather.Moscow.2010-2014\moskva_2010.xlsx";
        var stream = File.Open(path, FileMode.Open);
        var file = new FormFile(stream, 0, stream.Length, "streamFile", path.Split(@"\").Last());
        
        var yearWeather = ExcelParser.GetExcelDataTable(file).Result;
        
        Assert.IsTrue(yearWeather != null);
    }
    
    [Test]
    public void SendUnsupportedFileType_WhenAllCorrect_ShouldThrowEx()
    {
        var path = @"C:\Users\Lenovo\OneDrive\Рабочий стол\Испытательное задание.docx";
        var stream = File.Open(path, FileMode.Open);
        var file = new FormFile(stream, 0, stream.Length, "streamFile", path.Split(@"\").Last());
        
        Assert.Throws<AggregateException>( () =>
        {
            var yearWeather = ExcelParser.GetExcelDataTable(file).Result;
        });
    }
}