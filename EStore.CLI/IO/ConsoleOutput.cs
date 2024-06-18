using System.Globalization;
using EStore.Domain.Models;
using EStoreCLI.Data;
using EStoreCLI.Info;

namespace EStoreCLI.IO;

internal static class ConsoleOutput
{
    private const string PathToManualFile = @"/Users/ulkiorra/RiderProjects/EStoreCLI/EStore.CLI/Manual.json";
    private const short AlignmentValue = -35;
    
    private static readonly Manual Manual;

    static ConsoleOutput()
    {
        Manual = new Manual(File.ReadAllText(PathToManualFile));
    }

    public static void ShowWelcomeMessage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("---------------------------Welcome to EStore CLI---------------------------\n");
        Console.WriteLine("Supported commands (for more info enter \"help\"):\n");
        
        
        foreach (var commandDesc in Manual.GetSupportedCommands())
        {
            WriteColored($"{$"{commandDesc.Name}:", AlignmentValue}", ConsoleColor.Yellow);
            WriteLineColored(commandDesc.Syntax, ConsoleColor.DarkCyan);
        }
        
        WriteLineColored("\n---------------------------------------------------------------------------\n", 
            ConsoleColor.Cyan);
    }

    public static void ShowRegisterOrLoginMessage()
    {
        WriteLineColored("Type R to register, or L to login", ConsoleColor.Cyan);
    }

    public static void SayWelcomeToUser(string userName)
    {
        WriteLineColored($"\nWelcome, {userName}!\n", ConsoleColor.Cyan);
    }
    
    public static void SayBye()
    {
        WriteLineColored("\nBye!\n", ConsoleColor.Cyan);
    }

    public static void ShowError(string error)
    {
        WriteLineColored($"ERROR: {error.ToUpper()}", ConsoleColor.Red);
    }

    public static void ShowInfoAboutUser()
    {
        WriteColored("UserName:\t", ConsoleColor.DarkCyan);
        WriteLineColored(UserData.UserName, ConsoleColor.Yellow);
        WriteColored("BirthDate:\t", ConsoleColor.DarkCyan);
        WriteLineColored(UserData.BirthDate.ToString(CultureInfo.InvariantCulture), ConsoleColor.Yellow);
        WriteColored("Role:\t\t", ConsoleColor.DarkCyan);
        WriteLineColored(UserData.UserRole!.Name, ConsoleColor.Yellow);
    }

    public static void ShowCart(List<Product> entities)
    {
        WriteLineColored("Your cart:", ConsoleColor.DarkCyan);
        
        foreach (var entity in entities)
        {
            WriteColored($"\t{entity.Id}\t", ConsoleColor.Yellow);
            WriteLineColored($"{entity.Name}\t", ConsoleColor.DarkCyan);
        }
    }
    
    public static void ShowProducts(List<Product> entities)
    {
        foreach (var entity in entities)
        {
            WriteColored($"\t{entity.Id}\t", ConsoleColor.Yellow);
            WriteColored($"{entity.Name, AlignmentValue}", ConsoleColor.DarkCyan);
            WriteLineColored($"Category: {entity.Category!.Name}", ConsoleColor.Yellow);
        }
    }

    public static void ShowInfoAboutProduct(Product entity)
    {
        WriteColored("Id: ", ConsoleColor.Yellow);
        WriteLineColored(entity.Id.ToString(), ConsoleColor.DarkCyan);
        
        WriteColored("Name: ", ConsoleColor.Yellow);
        WriteLineColored(entity.Name, ConsoleColor.DarkCyan);
        
        WriteColored("Description: ", ConsoleColor.Yellow);
        WriteLineColored(entity.Description!, ConsoleColor.DarkCyan);
        
        WriteColored("Price: ", ConsoleColor.Yellow);
        WriteLineColored($"{entity.Cost.ToString(CultureInfo.InvariantCulture)} rub", ConsoleColor.DarkCyan);
        
        WriteColored("Seller: ", ConsoleColor.Yellow);
        WriteLineColored(entity.Seller!.UserName, ConsoleColor.DarkCyan);
        
        WriteColored("Creation Date: ", ConsoleColor.Yellow);
        WriteLineColored(entity.CreationDate.ToString(CultureInfo.InvariantCulture), ConsoleColor.DarkCyan);
    }

    public static void ShowDoneMessage()
    {
        WriteLineColored("Done.", ConsoleColor.Green);
    }
    
    public static void ShowAllCommandsInfo()
    {
        foreach (var commandInfo in Manual.GetSupportedCommands())
        {
            WriteLineColored($"{commandInfo.Name} command:", ConsoleColor.Yellow);
            WriteColored("\tSyntax:\t", ConsoleColor.DarkYellow);
            WriteLineColored($"\t{commandInfo.Syntax}", ConsoleColor.Cyan);

            if (commandInfo.ArgumentsAndDescriptions.Count != 0) 
                WriteLineColored("\tArguments:", ConsoleColor.DarkYellow);
                
            foreach (var commandArg in commandInfo.ArgumentsAndDescriptions)
            {
                WriteColored($"\t\t{commandArg.Key}\t", ConsoleColor.Cyan);
                WriteLineColored($"{commandArg.Value}", ConsoleColor.DarkCyan);
            }
            
            WriteColored("\tDescription:", ConsoleColor.DarkYellow);
            WriteLineColored($"\t{commandInfo.Description}\n", ConsoleColor.DarkCyan);
        }
    }

    public static void WriteColored(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ResetColor();
    }
    
    public static void WriteLineColored(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}