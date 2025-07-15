using Novell.Directory.Ldap;

namespace Identity.Api.Extensions;

public static class RolesExtensions {
    public static string GetRoleName(this List<LdapEntry> roles, string gidNumber) {
        return roles
                .Where(e => e.Get("gidNumber").StringValue == gidNumber)
                .FirstOrDefault()
                .Get("cn")
                .StringValue;
    }
}
