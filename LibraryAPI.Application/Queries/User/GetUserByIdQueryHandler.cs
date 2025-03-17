using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Domain.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var user =  await _userRepository.GetByIdAsync(query.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id {query.Id} не найден");
        }
        
        return _mapper.Map<UserDTO>(user);
    }
}