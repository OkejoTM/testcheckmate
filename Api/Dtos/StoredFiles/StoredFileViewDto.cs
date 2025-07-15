namespace Dtos.StoredFiles;

public class StoredFileViewDto {
    public Guid Id { set; get; }
    public string Path { set; get; }
    public string Name { set; get; }
    public string MimeType { set; get; }
}
