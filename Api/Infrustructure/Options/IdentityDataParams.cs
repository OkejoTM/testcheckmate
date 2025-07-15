using System.ComponentModel.DataAnnotations;

namespace Infrustructure.Options;

public class IdentityDataParams {
    public static readonly string SectionName = "IdentityData";

    [Required]
    public string ServerAddress { get; set; }
}
