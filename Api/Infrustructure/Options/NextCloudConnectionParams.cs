using System.ComponentModel.DataAnnotations;

namespace Infrustructure.Options;

public class NextCloudConnectionParams {
    public static readonly string SectionName = "NextCloudConnection";

    [Required]
    public string BaseUrl { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string BasePath { get; set; }
}
