namespace WeatherMoscow.Services.Exceptions;

public class UnsupportedTypeException : Exception
{
    public UnsupportedTypeException(string message) : base(message){}
}