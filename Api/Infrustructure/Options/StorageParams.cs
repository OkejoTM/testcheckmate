using System.ComponentModel.DataAnnotations;

namespace Infrustructure.Options;

public class StorageServiceParams {
    public static readonly string SectionName = "Storage";

    [Required]
    public string RootPath { get; set; }
}
