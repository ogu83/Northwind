using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol.Server;
using NorthwindApi.Models;

namespace NorthwindMcpServer.Tools;

[McpServerToolType]
public static class OrderTool
{
    private const string entity = "order";
    private const string entityPlural = "orders";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [McpServerTool, Description($"Gets all the {entityPlural} from the API for a given customer and return back to the client as List of {entity}.")]
    public static ValueTask<List<Order>> GetAll_Orders_ByCustomer(
        HttpClient httpClient,
        [Description($"id of the customer")] int id,
        CancellationToken cancellationToken)
    => ToolHelpers.GetAll<Order>(httpClient, $"{entity}/Customer/{id}", JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Gets all the {entityPlural} from the API and return back to the client as List of {entity}.")]
    public static ValueTask<List<Order>> GetAll_Orders(
        HttpClient httpClient,
        CancellationToken cancellationToken)
    => ToolHelpers.GetAll<Order>(httpClient, entity, JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Gets the paginated {entityPlural} from the API and return back to the client as PagedList of {entity}.")]
    public static ValueTask<PagedList<Order>?> GetPaged_Orders(
        HttpClient httpClient,
        [Description($"The number of {entityPlural} to skip for pagination.")] int skip,
        [Description($"The number of {entityPlural} to take for pagination.")] int take,
        [Description($"The field to order the results by. It can only be null or empty for OrderId, can only be CustomerId, OrderDate, RequiredDate, ShippedDate")] string orderBy,
        [Description($"The field to order the results Ascending or Descending, true for Ascending")] bool isAscending,
        [Description($"An optional contains filter to apply to the results")] string? filter,
        CancellationToken cancellationToken)
    => ToolHelpers.GetPaged<Order>(httpClient, entity, JsonSerializerOptions, skip, take, orderBy, isAscending, filter, cancellationToken);

    [McpServerTool, Description($"Gets the {entity} details from the API with given id and return back to the client as {entity} details.  If the {entity} does not exist, it will return null.")]
    public static ValueTask<Order?> GetById_Order(
            HttpClient httpClient,
            [Description($"id of the {entity}")] int id,
            CancellationToken cancellationToken)
    => ToolHelpers.GetById<Order, int>(httpClient, entity, JsonSerializerOptions, id, cancellationToken);

    [McpServerTool, Description($"Saves the {entity} to the API and return back to the client as {entity} details. Only the properties that are not null will be saved. If the {entity} does not exist, it will be created.")]
    public static ValueTask<Order> Save_Order(
        HttpClient httpClient,
        [Description($"The {entity} to save.")] Order e,
        CancellationToken cancellationToken)
    => ToolHelpers.Save(httpClient, entity, e, JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Deletes the {entity} with given id from the API and return back to the client as success or failure message.")]
    public static ValueTask Delete_Order(
        HttpClient httpClient,
        [Description($"id of the {entity} to delete.")] int id,
        CancellationToken cancellationToken)
    => ToolHelpers.Delete(httpClient, entity, id, cancellationToken);
}