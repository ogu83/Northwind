using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol.Server;
using NorthwindApi.Models;

[McpServerToolType]
public static class CategoryTool
{
    private const string address = "category";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [McpServerTool, Description("Gets all the categories from the API and return back to the client as List of Category.")]
    public static async ValueTask<List<Category>> GetAll(
            HttpClient httpClient,
            CancellationToken cancellationToken)
    {
        try
        {
            var doc = await httpClient.ReadJsonDocumentAsync($"/{address}");
            var categories = JsonSerializer.Deserialize<List<Category>>(doc.RootElement,JsonSerializerOptions);
            return categories ?? [];
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching categories: {ex.Message}");
            return [];
        }
    }

    // // Implement query handler
    // public override async Task<IEnumerable<object>> QueryAsync(ModelQuery query, CancellationToken cancellationToken)
    // {
    //     // Call your .NET 9.0 API endpoint to get Categories
    //     var categories = await _httpClient.GetFromJsonAsync<List<Category>>($"/{address}", cancellationToken);
    //     return categories;
    // }

    // // Implement get by id handler
    // public override async Task<object> GetAsync(object id, CancellationToken cancellationToken)
    // {
    //     var category = await _httpClient.GetFromJsonAsync<Category>($"/Category/{id}", cancellationToken);
    //     return category;
    // }

    // // Implement save handler
    // public override async Task<object> SaveAsync(object entity, CancellationToken cancellationToken)
    // {
    //     var response = await _httpClient.PutAsJsonAsync("/Category", entity, cancellationToken);
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<Category>(cancellationToken: cancellationToken);
    // }

    // // Implement delete handler
    // public override async Task DeleteAsync(object id, CancellationToken cancellationToken)
    // {
    //     var response = await _httpClient.DeleteAsync($"/Category?id={id}", cancellationToken);
    //     response.EnsureSuccessStatusCode();
    // }
}