using System.ComponentModel.DataAnnotations;

namespace Shared.Options;

public class LdapConnectionParams {
    public static readonly string SectionName = "LdapConnection";

    [Required]
    public string Host { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string BaseDn { get; set; }

    [Required]
    public string UsersOU { get; set; }

    [Required]
    public string RolesOU { get; set; }
}
