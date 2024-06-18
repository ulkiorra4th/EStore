using CSharpFunctionalExtensions;

namespace EStore.Infrastructure.Abstractions.Mappers.Abstractions;

public interface IMapper<T1, T2>
{
    Result<T1> MapFrom(T2 obj);
    Result<T2> MapFrom(T1 obj);
}