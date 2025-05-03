namespace NorthwindApi.Models;

public class PagedList<T> : BaseModel where T: BaseModel
{
    public required List<T> Items { get; set; }

    public int ItemCount => Items.Count; 

    public required int TotalCount { get; set; }

    public required int StartIndex { get; set; }

    public int EndIndex => StartIndex + ItemCount;

    public int PageCount => TotalCount / ItemCount;

    public int PageIndex => StartIndex / ItemCount;

    public bool IsLastPage => EndIndex >= TotalCount;
}
