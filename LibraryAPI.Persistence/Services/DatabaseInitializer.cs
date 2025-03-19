using LibraryAPI.Persistence.Data;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Persistence.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly LibraryDbContext _context;

    public DatabaseInitializer(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
    }
}
