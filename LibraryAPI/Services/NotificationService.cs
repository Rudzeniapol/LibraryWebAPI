using LibraryAPI.Data;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LibraryDbContext _context;

        public NotificationService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetOverdueBooksAsync()
        {
            var overdueBooks = await _context.Books
                .Where(b => b.ReturnBy < DateTime.UtcNow && b.UserId != null)
                .Select(b => $"Книга '{b.Title}' просрочена! Верните её как можно скорее.")
                .ToListAsync();

            return overdueBooks;
        }
    }
}