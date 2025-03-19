namespace LibraryAPI.Application.Services.Interfaces;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}