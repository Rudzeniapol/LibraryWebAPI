using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<string>> GetOverdueBooksAsync(CancellationToken cancellationToken = default);
    }
}