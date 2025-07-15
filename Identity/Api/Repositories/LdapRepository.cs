using Identity.Api.Domain;
using Identity.Api.Options;
using Identity.Api.Extensions;
using Novell.Directory.Ldap;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Identity.Api.Repositories;

public class LdapRepository : ILdapRepository {
    private readonly LdapConnectionParams _connectionParams;
    private readonly ILogger<LdapRepository> _logger;

    public LdapRepository(IOptions<LdapConnectionParams> ldapConnectionParams, ILogger<LdapRepository> logger) {
        _connectionParams = ldapConnectionParams.Value;
        _logger = logger;
    }

    public async Task<List<User>> GetUsersAsync() {
        using var connection = new LdapConnection();
        await connection.ConnectAsync(_connectionParams.Host, _connectionParams.Port);
        await connection.BindAsync("cn=admin," + _connectionParams.BaseDn, _connectionParams.Password);

        var searchResult = await connection.SearchAsync(
            $"ou={_connectionParams.UsersOU},{_connectionParams.BaseDn}",
            LdapConnection.ScopeSub,
            "(objectClass=inetOrgPerson)",
            null,
            false
        );

        var rolesEntries = await GetRolesAsLdapEntriesAsync(connection);
        var users = new List<User>();
        while (await searchResult.HasMoreAsync()) {
            var userEntry = await searchResult.NextAsync();
            
            users.Add(new User {
                Id = int.Parse(userEntry.Get("uidNumber").StringValue),
                UserName = userEntry.Get("cn").StringValue,
                Role = rolesEntries.GetRoleName(userEntry.Get("gidNumber").StringValue)
            });
        }

        return users;
    }

    public async Task<User?> GetUserAsync(int userId) {
        using var connection = new LdapConnection();
        await connection.ConnectAsync(_connectionParams.Host, _connectionParams.Port);
        await connection.BindAsync("cn=admin," + _connectionParams.BaseDn, _connectionParams.Password);

        var searchResult = await connection.SearchAsync(
            $"ou={_connectionParams.UsersOU},{_connectionParams.BaseDn}",
            LdapConnection.ScopeSub,
            $"(uidNumber={userId})",
            null,
            false
        );

        if (!await searchResult.HasMoreAsync()) {
            return null;
        }

        var rolesEntries = await GetRolesAsLdapEntriesAsync(connection);
        var userEntry = await searchResult.NextAsync();
            
        return new User {
            Id = int.Parse(userEntry.Get("uidNumber").StringValue),
            UserName = userEntry.Get("cn").StringValue,
            Role = rolesEntries.GetRoleName(userEntry.Get("gidNumber").StringValue)
        };
    }

    public async Task<User?> AuthenticateUserAsync(string username, string password) {
        using var connection = new LdapConnection();
        await connection.ConnectAsync(_connectionParams.Host, _connectionParams.Port);
        await connection.BindAsync("cn=admin," + _connectionParams.BaseDn, _connectionParams.Password);
        
        var searchResult = await connection.SearchAsync(
            $"ou={_connectionParams.UsersOU},{_connectionParams.BaseDn}",
            LdapConnection.ScopeSub,
            $"(cn={username})",
            null,
            false
        );

        if (!await searchResult.HasMoreAsync()) {
            return null;
        }

        var userEntry = await searchResult.NextAsync();

        try {
            await connection.BindAsync(userEntry.Dn, password);
            await connection.BindAsync("cn=admin," + _connectionParams.BaseDn, _connectionParams.Password);
        }
        catch (LdapException ex) {
            return null;
        }

        var rolesEntries = await GetRolesAsLdapEntriesAsync(connection);
            
        return new User {
            Id = int.Parse(userEntry.Get("uidNumber").StringValue),
            UserName = userEntry.Get("cn").StringValue,
            Role = rolesEntries.GetRoleName(userEntry.Get("gidNumber").StringValue)
        };
    }

    private async Task<List<LdapEntry>> GetRolesAsLdapEntriesAsync(LdapConnection connection) {
        var rolesEntries = new List<LdapEntry>();

        var searchResult = await connection.SearchAsync(
            $"ou={_connectionParams.RolesOU},{_connectionParams.BaseDn}",
            LdapConnection.ScopeSub,
            "(objectClass=posixGroup)",
            null,
            false
        );

        while (await searchResult.HasMoreAsync()) {
            rolesEntries.Add(await searchResult.NextAsync());
        }

        return rolesEntries;
    }
}
