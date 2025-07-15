using Duende.IdentityServer.Models;

namespace Identity.Api.Services;

public interface IConfigProvider {
    IEnumerable<ApiScope> GetApiScopes();
    IEnumerable<Client> GetClients();
}
