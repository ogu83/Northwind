using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

namespace NorthwindAgentApi.Controllers;

public class ChatController : ApiControllerBase
{
    private readonly IMcpClient _mcpClient;
    private readonly IChatClient _chatClient;
    private readonly Dictionary<Guid, ChatSession> _chatSessions;

    public ChatController(
        ILoggerFactory loggerFactory,
        IMcpClient mcpClient,
        IChatClient chatClient,
        Dictionary<Guid, ChatSession> chatSessions)
        : base(loggerFactory)
    {
        _mcpClient = mcpClient ?? throw new ArgumentNullException(nameof(mcpClient), "mcpClient cannot be null.");
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient), "chatClient cannot be null.");
        _chatSessions = chatSessions ?? throw new ArgumentNullException(nameof(chatSessions), "chatSessions must be a valid dictionary.");
    }

    [HttpGet("McpTools")]
    public async Task<ActionResult<List<McpClientTool>>> GetMcpToolsAsync()
    {
        var mcpTools = await _mcpClient.ListToolsAsync();
        return Ok(mcpTools);
    }

    [HttpGet("McpMethods")]
    public ActionResult<List<McpMethod>> GetMcpMethods()
    {
        List<McpMethod> methods = [];
        foreach (var method in _mcpClient.GetType().GetMethods())
        {
            if (method.Name.Contains("Tool") || method.Name.Contains("Function"))
            {
                var m = new McpMethod(method.Name);
                methods.Add(m);
                m.Parameters.AddRange(from parameter in method.GetParameters()
                                      select new McpParameter(parameter.Name ?? "", parameter.ParameterType.Name));
            }
        }
        return Ok(methods);
    }

    [HttpGet("CreateSession")]
    public async Task<ActionResult<Guid>> CreateSession()
    {
        var mcpTools = await _mcpClient.ListToolsAsync();
        var sessionId = Guid.NewGuid();
        _chatSessions[sessionId] = new ChatSession();
        _chatSessions[sessionId].Options.Tools = [.. mcpTools];
        return Ok(new { SessionId = sessionId });
    }

    [HttpGet("GetSessions")]
    public ActionResult<List<Guid>> GetActiveSessions()
    {
        var activeSessions = _chatSessions.Keys.ToList();
        return Ok(activeSessions);
    }

    [HttpGet("GetSession/{sessionId}")]
    public ActionResult<ChatSession> GetSession(Guid sessionId)
    {
        if (!_chatSessions.ContainsKey(sessionId))
        {
            return NotFound($"Session {sessionId} not found.");
        }

        var session = _chatSessions[sessionId];
        if (session == null)
        {
            return NotFound($"Session {sessionId} not found.");
        }
        return Ok(session);
    }

    [HttpPost("Prompt/{sessionId}")]
    public async Task<ActionResult<ChatMessage>> Prompt(Guid sessionId, [FromBody] string userPrompt)
    {
        if (!_chatSessions.ContainsKey(sessionId))
        {
            return NotFound($"Session {sessionId} not found.");
        }

        var session = _chatSessions[sessionId];
        var chatHistory = session.ChatHistory;
        var chatOptions = session.Options;

        chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

        string finalResponse = "";
        UsageDetails? usageDetails = null;

        // Get the entire response in one go (no loop)
        var allFunctionCalls = new List<FunctionCallContent>();
        string currentResponse = "";

        await foreach (var item in _chatClient.GetStreamingResponseAsync(chatHistory, chatOptions))
        {
            // Collect any function calls we find
            if (item.Contents.FirstOrDefault(c => c is FunctionCallContent) is FunctionCallContent functionCallContent)
            {
                allFunctionCalls.Add(functionCallContent);
                _logger.LogInformation($"Function call found: {functionCallContent.Name} (Collecting, not executing yet)");
            }
            else
            {
                // Regular text content
                currentResponse += item.Text;
            }

            var usage = item.Contents.OfType<UsageContent>().FirstOrDefault()?.Details;
            if (usage != null) usageDetails = usage;
        }

        // Store the current assistant response even if we found function calls
        finalResponse = currentResponse;

        // If we found function calls, process them and return their responses only, without another AI call
        if (allFunctionCalls.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.LogInformation($"Processing {allFunctionCalls.Count} function call(s) without adding to chat history yet");

            // Process each function call sequentially
            foreach (var functionCallContent in allFunctionCalls)
            {
                _logger.LogInformation($"Executing MCP tool: {functionCallContent.Name}");

                try
                {
                    // Get the arguments from the function call
                    var arguments = functionCallContent.Arguments as Dictionary<string, object>;

                    // Display the arguments
                    if (arguments != null)
                    {
                        _logger.LogInformation($"Arguments: {string.Join(", ", arguments.Select(a => $"{a.Key}: {a.Value}"))}");
                    }

                    // Call the MCP tool
                    var toolResult = await _mcpClient.CallToolAsync(
                        functionCallContent.Name,
                        arguments!
                    );

                    // Extract the text content from the result
                    var toolResponseText = string.Join("\n",
                        toolResult.Content
                            .Where(c => c.Type == "text")
                            .Select(c => c.Text));

                    _logger.LogInformation($"Tool response: {toolResponseText}");

                    // Do NOT add to chat history yet
                    _logger.LogInformation($"Tool response from {functionCallContent.Name} will be returned to the user directly");
                    // Append tool response to final response
                    // finalResponse += $"\n\n**Results from {functionCallContent.Name}:**\n{toolResponseText}";
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error executing tool {functionCallContent.Name}: {ex.Message}");
                    // Append error to final response
                    // finalResponse += $"\n\n**Error from {functionCallContent.Name}:**\n{ex.Message}";
                }
            }
        }

        // Add only one final assistant response that includes both AI text and tool results
        var assistantMessage = new ChatMessage(ChatRole.Assistant, finalResponse);
        chatHistory.Add(assistantMessage);
        _logger.LogInformation($"AI Response completed at {DateTime.Now}:");
        ShowUsageDetails(usageDetails);

        _logger.LogDebug("\n=== CHAT HISTORY AFTER PROCESSING ===");
        for (int i = 0; i < chatHistory.Count; i++)
        {
            // Since ChatMessage doesn't have a Content property, use the standard ToString method
            _logger.LogDebug($"[{i}] {chatHistory[i].Role}: {TruncateString(chatHistory[i].ToString(), 50)}");
        }

        return Ok(assistantMessage);
    }

    private static string TruncateString(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return input.Length <= maxLength ? input : string.Concat(input.AsSpan(0, maxLength), "...");
    }

    private void ShowUsageDetails(UsageDetails? usage)
    {
        if (usage != null)
        {
            _logger.LogInformation($"InputTokenCount: {usage.InputTokenCount}");
            _logger.LogInformation($"OutputTokenCount: {usage.OutputTokenCount}");
            _logger.LogInformation($"TotalTokenCount: {usage.TotalTokenCount}");

            if (usage.AdditionalCounts != null)
                foreach (var additionalCount in usage.AdditionalCounts)
                    _logger.LogInformation($"{additionalCount.Key}: {additionalCount.Value}");

        }
    }
}