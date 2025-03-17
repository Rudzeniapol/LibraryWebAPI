namespace LibraryAPI.Persistence.Services.Interfaces;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}