using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol.Server;
using NorthwindApi.Models;

namespace NorthwindMcpServer.Tools;

[McpServerToolType]
public static class CustomerTool
{
    private const string entity = "customer";
    private const string entityPlural = "customers";

    private static JsonSerializerOptions JsonSerializerOptions => ToolHelpers.JsonSerializerOptions;

    [McpServerTool, Description($"Gets all the {entityPlural} from the API and return back to the client as List of {entity}.")]
    public static ValueTask<List<Customer>> GetAll_Customers(
        HttpClient httpClient,
        CancellationToken cancellationToken)
    => ToolHelpers.GetAll<Customer>(httpClient, entity, JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Gets the paginated {entityPlural} from the API and return back to the client as PagedList of {entity}.")]
    public static ValueTask<PagedList<Customer>?> GetPaged_Customers(
        HttpClient httpClient,
        [Description($"The number of {entityPlural} to skip for pagination.")] int skip,
        [Description($"The number of {entityPlural} to take for pagination.")] int take,
        [Description($"The field to order the results by. It can only be null or empty for customerId, can only be companyName, contactName, contactTitle, address, city, region, postalCode, country")] string orderBy,
        [Description($"The field to order the results Ascending or Descending, true for Ascending")] bool isAscending,
        [Description($"An optional contains filter to apply to the results")] string? filter,
        CancellationToken cancellationToken)
    => ToolHelpers.GetPaged<Customer>(httpClient, entity, JsonSerializerOptions, skip, take, orderBy, isAscending, filter, cancellationToken);

    [McpServerTool, Description($"Gets the {entity} details from the API with given id and return back to the client as {entity} details.  If the {entity} does not exist, it will return null.")]
    public static ValueTask<Customer?> GetById_Customer(
            HttpClient httpClient,
            [Description($"id of the {entity}")] string id,
            CancellationToken cancellationToken)
    => ToolHelpers.GetById<Customer, string>(httpClient, entity, JsonSerializerOptions, id, cancellationToken);

    [McpServerTool, Description($"Saves the {entity} to the API and return back to the client as {entity} details. Only the properties that are not null will be saved. If the {entity} does not exist, it will be created. customerId is optional and can be null or 0. If customerId is not provided, a new {entity} will be created with a new id. CompanyName is required and cannot be null or empty. ContactName, ContactTitle, Address, City, Region, PostalCode, Country are optional and can be null.")]
    public static ValueTask<Customer> Save_Customer(
        HttpClient httpClient,
        [Description($"The {entity} to save.")] Customer e,
        CancellationToken cancellationToken)
    => ToolHelpers.Save(httpClient, entity, e, JsonSerializerOptions, cancellationToken);

    [McpServerTool, Description($"Deletes the {entity} with given id from the API and return back to the client as success or failure message.")]
    public static ValueTask Delete_Customer(
        HttpClient httpClient,
        [Description($"id of the {entity} to delete.")] int id,
        CancellationToken cancellationToken)
    => ToolHelpers.Delete(httpClient, entity, id, cancellationToken);
}
