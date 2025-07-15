namespace Dtos;

public class TableDto<T> {
    public int ItemsPerPage { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }

    public T[] Items { get; set; }
}
