using CSharpFunctionalExtensions;
using EStore.Domain.Extensions;
using EStore.Infrastructure.Tools.Interfaces;

namespace EStore.Infrastructure.Tools;

internal sealed class DateParser : IDateParser
{
    private const short DayIndex = 0;
    private const short MonthIndex = 1;
    private const short YearIndex = 2;
    
    public Result<DateTime> ParseString(string? dateStr)
    {
        if (String.IsNullOrEmpty(dateStr)) return Result.Failure<DateTime>("Empty date string");
        var splitDate = dateStr.Split(".");

        try
        {
            var date = new DateTime(
                int.Parse(splitDate[YearIndex]), 
                int.Parse(splitDate[MonthIndex]), 
                int.Parse(splitDate[DayIndex]));
            
            return Result.Success(date.SetKindUtc());
        }
        catch(Exception)
        {
            return Result.Failure<DateTime>("wrong birth date format");
        }
    }
}