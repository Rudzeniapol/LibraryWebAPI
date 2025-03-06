using MediatR;

namespace LibraryAPI.Application.Queries.Book;

public class GetBookImageQuery : IRequest<Stream>
{
    public string Filename { get; set; }
}