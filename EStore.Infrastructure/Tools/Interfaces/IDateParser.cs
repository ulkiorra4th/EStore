using CSharpFunctionalExtensions;

namespace EStore.Infrastructure.Tools.Interfaces;

public interface IDateParser
{
    Result<DateTime> ParseString(string? dateStr);
}