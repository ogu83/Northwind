using System.Text.Json;
using NorthwindApi.Models;
using NorthwindMcpServer.Helpers;

namespace NorthwindMcpServer.Tools;

public static class ToolHelpers
{
    public static async ValueTask<List<T>> GetAll<T>(
        HttpClient httpClient,
        string entityPath,
        JsonSerializerOptions options,
        CancellationToken cancellationToken) where T : BaseModel
    {
        try
        {
            var doc = await httpClient.ReadJsonDocumentAsync($"/{entityPath}");
            var items = doc.RootElement.Deserialize<List<T>>(options);
            return items ?? [];
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching entities: {ex.Message}");
            return [];
        }
    }

    public static async ValueTask<PagedList<T>?> GetPaged<T>(
        HttpClient httpClient,
        string entityPath,
        JsonSerializerOptions options,
        int skip,
        int take,
        string orderBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken) where T : BaseModel
    {
        try
        {
            var doc = await httpClient.ReadJsonDocumentAsync($"/{entityPath}/skip/{skip}/take/{take}/orderBy/{orderBy}/asc/{isAscending}/filter/{Uri.EscapeDataString(filter ?? string.Empty)}");
            var items = doc.RootElement.Deserialize<PagedList<T>>(options);
            return items;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching paginated entities: {ex.Message}");
            return null;
        }
    }

    public static async ValueTask<T?> GetById<T,IDT>(
        HttpClient httpClient,
        string entityPath,
        JsonSerializerOptions options,
        IDT id,
        CancellationToken cancellationToken) where T : BaseModel
    {
        try
        {
            var doc = await httpClient.ReadJsonDocumentAsync($"/{entityPath}/{id}");
            var e = doc.RootElement.Deserialize<T>(options);
            return e;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching {entityPath} by ID {id}: {ex.Message}");
            return null;
        }
    }

    public static async ValueTask<T> Save<T>(
        HttpClient httpClient,
        string entityPath,
        T entity,
        JsonSerializerOptions options,
        CancellationToken cancellationToken) where T : BaseModel
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            if (string.IsNullOrWhiteSpace(entityPath))
            {
                throw new ArgumentException("Entity path cannot be null or empty", nameof(entityPath));
            }

            if (entity is not BaseModel)
            {
                throw new ArgumentException($"Entity must be of type {nameof(BaseModel)}", nameof(entity));
            }

            var response = await httpClient.PutAsJsonAsync($"/{entityPath}", entity, options, cancellationToken);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(options, cancellationToken) ?? entity;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error saving {entityPath}: {ex.Message}");
            throw;
        }
    }
    
    public static async ValueTask Delete(
        HttpClient httpClient,
        string entityPath,
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"/{entityPath}?id={id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting {entityPath} with ID {id}: {ex.Message}");
            throw;
        }
    }
}
