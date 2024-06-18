namespace EStore.Domain.Extensions;

public static class DateTimeExtensions
{
    public static int CountAge(this DateTime date)
    {
        var now = DateTime.Today;
        return now.Month > date.Month && now.Day > date.Day ? now.Year - date.Year : now.Year - date.Year - 1;
    }

    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}