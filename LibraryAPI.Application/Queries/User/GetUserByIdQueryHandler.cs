using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Domain.Models.User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Domain.Models.User> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var user =  await _userRepository.GetByIdAsync(query.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id {query.Id} не найден");
        }

        return user;
    }
}