using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<string>> GetOverdueBooksAsync(CancellationToken cancellationToken = default);
    }
}