using System.Runtime.Serialization;

namespace WeatherMoscow.Entity.Enums;

public enum Month
{
    [EnumMember(Value = "Январь")] 
    January = 0,
    [EnumMember(Value = "Февраль")]
    February,
    [EnumMember(Value = "Март")]
    March,
    [EnumMember(Value = "Апрель")]
    April,
    [EnumMember(Value = "Май")]
    May,
    [EnumMember(Value = "Июнь")]
    June,
    [EnumMember(Value = "Июль")]
    July,
    [EnumMember(Value = "Август")]
    August,
    [EnumMember(Value = "Сентябрь")]
    September,
    [EnumMember(Value = "Октябрь")]
    October,
    [EnumMember(Value = "Ноябрь")]
    November,
    [EnumMember(Value = "Декабрь")]
    December
}