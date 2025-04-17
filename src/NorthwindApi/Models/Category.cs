namespace NorthwindApi.Models;

public class Category : BaseModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }
}
