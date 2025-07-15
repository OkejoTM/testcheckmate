using Identity.Api.Domain;

namespace Identity.Api.Repositories;

public interface ILdapRepository {
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(int userId);
    Task<User> AuthenticateUserAsync(string username, string password);
}
