using LibraryAPI.Application.DTOs;
using MediatR;

namespace LibraryAPI.Application.Queries.User;

public class GetUserByIdQuery : IRequest<UserDTO>
{
    public int Id { get; set; }
}