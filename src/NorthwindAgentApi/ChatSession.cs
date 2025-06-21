using Microsoft.Extensions.AI;

namespace NorthwindAgentApi;

public class ChatSession
{
    public ChatOptions Options { get; private set; } = new();
    public List<ChatMessage> ChatHistory { get; private set; } = [];
}