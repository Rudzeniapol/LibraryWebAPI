using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByUsernameQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByUsernameAsync(query.Username, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с именем {query.Username} не найден");
        }
        return _mapper.Map<UserDTO>(user);
    }
}