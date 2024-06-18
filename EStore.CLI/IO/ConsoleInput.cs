using EStoreCLI.Models;

namespace EStoreCLI.IO;

internal static class ConsoleInput
{
    public static string? Read()
    {
        ConsoleOutput.WriteColored("> ", ConsoleColor.DarkYellow);
        return ReadLineColored(ConsoleColor.DarkYellow);
    }

    public static LoginModel ReadUserLoginInfo()
    {
        while (true)
        {
            ConsoleOutput.WriteColored("Enter username: ", ConsoleColor.Cyan);
            var userName = ReadLineColored(ConsoleColor.DarkYellow);

            ConsoleOutput.WriteColored("Enter password: ", ConsoleColor.Cyan);
            var password = ReadLineColored(ConsoleColor.DarkYellow);

            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            {
                ConsoleOutput.ShowError("username and password are required!");
                continue;
            }

            return new LoginModel(userName, password);
        }
    }

    public static RegisterModel ReadUserRegisterInfo()
    {
        while (true)
        {
            ConsoleOutput.WriteColored("Enter username: ", ConsoleColor.Cyan);
            var userName = ReadLineColored(ConsoleColor.DarkYellow);

            ConsoleOutput.WriteColored("Enter password: ", ConsoleColor.Cyan);
            var password = ReadLineColored(ConsoleColor.DarkYellow);

            ConsoleOutput.WriteColored("Enter birth date (dd.mm.yyyy): ", ConsoleColor.Cyan);
            var birthDateStr = ReadLineColored(ConsoleColor.DarkYellow);
            
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            {
                ConsoleOutput.ShowError("username and password are required!");
                continue;
            }

            return new RegisterModel(userName, password, birthDateStr);
        }
    }

    public static ConsoleKey ReadKey()
    {
        return Console.ReadKey().Key;
    }

    public static string? ReadLineColored(ConsoleColor color)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        var str = Console.ReadLine();

        Console.ResetColor();
        return str;
    }
}