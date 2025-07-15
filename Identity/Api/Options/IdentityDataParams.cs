using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Options;

public class IdentityDataParams {
    public static readonly string SectionName = "IdentityData";

    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ApiScope { get; set; }

    [Required]
    public string Secret { get; set; }
}
