using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using NorthwindApi.Models;

namespace NorthwindMcpServer.Tools;

[McpServerToolType]
public static class OrderDetailsTool
{
    private const string entity = "orderDetail";
    private const string entityPlural = "orderDetails";

    private static JsonSerializerOptions JsonSerializerOptions => ToolHelpers.JsonSerializerOptions;

    [McpServerTool, Description($"Gets all the {entityPlural} from the API for a given order and return back to the client as List of {entity}.")]
    public static ValueTask<List<OrderDetail>> GetAll_OrderDetails_ByOrder(
        HttpClient httpClient,
        [Description($"id of the order")] int id,
        CancellationToken cancellationToken)
    => ToolHelpers.GetAll<OrderDetail>(httpClient, $"{entityPlural}/Order/{id}", JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Gets all the {entityPlural} from the API and return back to the client as List of {entity}.")]
    public static ValueTask<List<OrderDetail>> GetAll_OrderDetails(
        HttpClient httpClient,
        CancellationToken cancellationToken)
    => ToolHelpers.GetAll<OrderDetail>(httpClient, entityPlural, JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Gets the paginated {entityPlural} from the API and return back to the client as PagedList of {entity}.")]
    public static ValueTask<PagedList<OrderDetail>?> GetPaged_OrderDetails(
        HttpClient httpClient,
        [Description($"The number of {entityPlural} to skip for pagination.")] int skip,
        [Description($"The number of {entityPlural} to take for pagination.")] int take,
        [Description($"The field to order the results by. It can only be null or empty for OrderDetailsId")] string orderBy,
        [Description($"The field to order the results Ascending or Descending, true for Ascending")] bool isAscending,
        [Description($"An optional contains filter to apply to the results")] string? filter,
        CancellationToken cancellationToken)
    => ToolHelpers.GetPaged<OrderDetail>(httpClient, entityPlural, JsonSerializerOptions, skip, take, orderBy, isAscending, filter, cancellationToken);

    [McpServerTool, Description($"Gets the {entity} details from the API with given id and return back to the client as {entity} details.  If the {entity} does not exist, it will return null.")]
    public static ValueTask<OrderDetail?> GetById_OrderDetail(
            HttpClient httpClient,
            [Description("orderId")] int orderId,
            [Description("productId")] int productId,
            CancellationToken cancellationToken)
    => ToolHelpers.GetById<OrderDetail, Tuple<int, int>>(httpClient, entityPlural, JsonSerializerOptions, new Tuple<int, int>(orderId, productId), cancellationToken);

    [McpServerTool, Description($"Saves the {entity} to the API and return back to the client as {entity} details. Only the properties that are not null will be saved. If the {entity} does not exist, it will be created.")]
    public static ValueTask<OrderDetail> Save_OrderDetail(
        HttpClient httpClient,
        [Description($"The {entity} to save.")] OrderDetail e,
        CancellationToken cancellationToken)
    => ToolHelpers.Save(httpClient, entityPlural, e, JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Deletes the {entity} with given id from the API and return back to the client as success or failure message.")]
    public static ValueTask Delete_OrderDetail(
        HttpClient httpClient,
        [Description("orderId")] int orderId,
        [Description("productId")] int productId,
        CancellationToken cancellationToken)
    => ToolHelpers.Delete(httpClient, entityPlural, new Tuple<int, int>(orderId, productId), cancellationToken);
}
