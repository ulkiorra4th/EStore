using CSharpFunctionalExtensions;
using EStore.Domain.Models;
using EStore.Infrastructure.Abstractions.Mappers.Abstractions;
using EStore.Persistence.Entities;

namespace EStore.Persistence.Mappers.Implementations;

internal sealed class CategoryMapper : IMapper<Category, CategoryEntity>
{
    public Result<Category> MapFrom(CategoryEntity? obj)
    {
        return obj is null 
            ? Result.Failure<Category>("argument is null") 
            : Category.Create(obj.Id, obj.Name, obj.CreationDate);
    }

    public Result<CategoryEntity> MapFrom(Category? obj)
    {
        return obj is null
            ? Result.Failure<CategoryEntity>("argument is null")
            : Result.Success(new CategoryEntity
            {
                Id = obj.Id,
                Name = obj.Name,
                CreationDate = obj.CreationDate
            });
    }
}