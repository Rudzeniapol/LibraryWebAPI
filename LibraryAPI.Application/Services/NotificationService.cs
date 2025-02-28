﻿using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Persistence.Data;

namespace LibraryAPI.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LibraryDbContext _context;

        public NotificationService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetOverdueBooksAsync(CancellationToken cancellationToken)
        {
            var overdueBooks = await _context.Books
                .Where(b => b.ReturnBy < DateTime.UtcNow && b.UserId != null)
                .Select(b => $"Книга '{b.Title}' просрочена! Верните её как можно скорее.")
                .ToListAsync(cancellationToken);

            return overdueBooks;
        }
    }
}