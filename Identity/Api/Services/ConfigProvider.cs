using Duende.IdentityServer.Models;
using Identity.Api.Options;
using Microsoft.Extensions.Options;

namespace Identity.Api.Services;

public class ConfigProvider : IConfigProvider {
    private readonly IdentityDataParams _identityDataParams;

    public ConfigProvider(IOptions<IdentityDataParams> identityDataParams) {
        _identityDataParams = identityDataParams.Value;
    }

    public IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope> {
        new ApiScope(name: _identityDataParams.ApiScope, displayName: "My API")
    };

    public IEnumerable<Client> GetClients() => new List<Client> {
        new Client {
            ClientId = _identityDataParams.ClientId,
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Secret(_identityDataParams.Secret.Sha256()) },
            AllowedScopes = { _identityDataParams.ApiScope }
        }
    };
}
