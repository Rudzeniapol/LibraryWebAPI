using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, Domain.Models.User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByUsernameQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Domain.Models.User> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByUsernameAsync(query.Username, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с именем {query.Username} не найден");
        }
        return user;
    }
}