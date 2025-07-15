namespace Domain.Entities;

public class StoredFile : Entity {
    public string Path { set; get; }
    public string Name { set; get; }
    public string MimeType { set; get; }
}
