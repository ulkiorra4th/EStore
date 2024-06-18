using CSharpFunctionalExtensions;
using EStore.Application.Interfaces.Services;
using EStore.Infrastructure.Tools.Interfaces;
using EStoreCLI.Data;
using EStoreCLI.IO;

namespace EStoreCLI.QueryProcess;

internal sealed class QueryHandler
{
    private readonly IUserService _userService;
    private readonly IProductService _productService;
    private readonly IRoleService _roleService;
    private readonly IDateParser _dateParser;

    public QueryHandler(IUserService userService, IProductService productService, IRoleService roleService, 
        IDateParser dateParser)
    {
        _userService = userService;
        _productService = productService;
        _roleService = roleService;
        _dateParser = dateParser;
    }

    public async Task RequestUserAuthInfo()
    {
        while (true)
        {
            ConsoleOutput.ShowRegisterOrLoginMessage();
            
            var key = ConsoleInput.ReadKey();

            switch (key)
            {
                case ConsoleKey.L:
                {
                    var model = ConsoleInput.ReadUserLoginInfo();
                    var loginResult = await _userService.Login(model.UserName!, model.Password!);

                    if (loginResult.IsFailure)
                    {
                        ConsoleOutput.ShowError(loginResult.Error);
                        break;
                    }
                    
                    ConsoleOutput.SayWelcomeToUser(model.UserName!);
                    
                    var user = (await _userService.GetUserWithRoleById(loginResult.Value.UserId)).Value;
                    var userRole = (await _roleService.GetRoleByUserId(user.UserId)).Value;

                    UserData.Load(user.UserId, user.UserName, userRole, user.BirthDate, user.CreationDate);
                    return;
                }
                
                case ConsoleKey.R:
                {
                    var model = ConsoleInput.ReadUserRegisterInfo();
                    var birthDateResult = _dateParser.ParseString(model.BirthDate);
                    
                    if (birthDateResult.IsFailure)
                    {
                        ConsoleOutput.ShowError(birthDateResult.Error);
                        break;
                    }
                    
                    var registerResult = await _userService.Register(model.UserName!, model.Password!, 
                        birthDateResult.Value);

                    if (registerResult.IsFailure)
                    {
                        ConsoleOutput.ShowError(registerResult.Error);
                        break;
                    }
                    
                    ConsoleOutput.SayWelcomeToUser(model.UserName!);
                    
                    var user = (await _userService.GetUserWithRoleById(registerResult.Value)).Value;
                    var userRole = (await _roleService.GetRoleByUserId(user.UserId)).Value;
                    
                    UserData.Load(user.UserId, user.UserName, userRole, user.BirthDate, user.CreationDate);
                    return;
                }
            }
        }
    }
    
    public async Task Listen()
    {
        while (true)
        {
            var query = ConsoleInput.Read();
            var queryTypeResult = DefineQueryType(query);

            if (queryTypeResult.IsFailure)
            {
                ConsoleOutput.ShowError(queryTypeResult.Error);
                continue;
            }
            
            switch (queryTypeResult.Value)
            {
                case QueryType.HelpQuery:
                {
                    ConsoleOutput.ShowAllCommandsInfo();
                    break;
                }

                case QueryType.WhoAmIQuery:
                { 
                    ConsoleOutput.ShowInfoAboutUser();
                    break;
                }

                case QueryType.ShowCartQuery:
                {
                    var userResult = await _userService.GetWithCartById(UserData.UserId);
                    
                    if (userResult.IsFailure)
                    {
                        ConsoleOutput.ShowError(userResult.Error);
                        break;
                    }

                    ConsoleOutput.ShowCart(userResult.Value.UserCart!.Products.ToList());
                    break;
                }

                case QueryType.AddToCartQuery:
                {
                    var queryElements = query!.Split(" ");
                    var argument = queryElements[^1];

                    if (!Guid.TryParse(argument, out var productId))
                    {
                        ConsoleOutput.ShowError("invalid product id");
                        break;
                    }
                    
                    var addToCartResult = await _userService.AddProductToCart(UserData.UserId, productId);

                    if (addToCartResult.IsFailure)
                    { 
                        ConsoleOutput.ShowError(addToCartResult.Error); 
                        break;
                    }
                    
                    ConsoleOutput.ShowDoneMessage();
                    break;
                }

                case QueryType.DeleteFromCartQuery:
                {
                    var queryElements = query!.Split(" ");
                    var argument = queryElements[^1];

                    if (!Guid.TryParse(argument, out var productId))
                    {
                        ConsoleOutput.ShowError("invalid product id");
                        break;
                    }

                    var deleteFromCartResult = await _userService.DeleteProductFromCart(UserData.UserId, productId);

                    if (deleteFromCartResult.IsFailure) 
                    { 
                        ConsoleOutput.ShowError(deleteFromCartResult.Error); 
                        break;
                    }
                    
                    ConsoleOutput.ShowDoneMessage(); 
                    break;
                }

                case QueryType.ShowAllProductsQuery:
                {
                    ConsoleOutput.ShowProducts(await _productService.GetAll());
                    break;
                }

                case QueryType.ShowInfoAboutProductQuery:
                {
                    var queryElements = query!.Split(" ");
                    var argument = queryElements[^1];

                    if (!Guid.TryParse(argument, out var productId))
                    {
                        ConsoleOutput.ShowError("invalid product id");
                        break;
                    }
                    
                    var productResult = await _productService.GetProduct(productId);
                    
                    if (productResult.IsFailure) 
                    { 
                        ConsoleOutput.ShowError(productResult.Error); 
                        break;
                    }
                    ConsoleOutput.ShowInfoAboutProduct(productResult.Value); break;
                }

                case QueryType.ExitQuery:
                {
                    ConsoleOutput.SayBye();
                    return;
                }
            }
        }
    }

    private Result<QueryType> DefineQueryType(string? query)
    {
        if (String.IsNullOrEmpty(query))
            return Result.Failure<QueryType>("EMPTY QUERY");
        
        var queryElements = query.ToUpper().Split(" ");

        return queryElements[0] switch
        {
            "SHOW" => queryElements[1] switch
            {
                "CART" => queryElements.Length == 2 
                    ? Result.Success(QueryType.ShowCartQuery) 
                    : Result.Failure<QueryType>("invalid query"),
                
                "ALL" => queryElements.Length == 3 
                    ? Result.Success(QueryType.ShowAllProductsQuery) 
                    : Result.Failure<QueryType>("invalid query"),
                
                "INFO" => queryElements.Length == 3 
                    ? Result.Success(QueryType.ShowInfoAboutProductQuery)
                    : Result.Failure<QueryType>("invalid query"),
                
                _ => Result.Failure<QueryType>("Invalid query")
            },
            "ADD" => queryElements.Length == 4 
                ? Result.Success(QueryType.AddToCartQuery)
                : Result.Failure<QueryType>("invalid query"),
            
            "DELETE" => queryElements.Length == 4 
                ? Result.Success(QueryType.DeleteFromCartQuery)
                : Result.Failure<QueryType>("invalid query"),
            
            "WHOAMI" => queryElements.Length == 1
                ? Result.Success(QueryType.WhoAmIQuery)
                : Result.Failure<QueryType>("invalid query"),
            
            "HELP" => queryElements.Length == 1
                ? Result.Success(QueryType.HelpQuery)
                : Result.Failure<QueryType>("invalid query"),
            
            "EXIT" => queryElements.Length == 1
                ? Result.Success(QueryType.ExitQuery)
                : Result.Failure<QueryType>("invalid query"),
            
            _ => Result.Failure<QueryType>("Invalid query")
        };
    }
}