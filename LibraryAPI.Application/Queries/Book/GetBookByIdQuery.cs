using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookByIdQuery :IRequest<Domain.Models.Book>
{
    public int Id { get; set; }
}