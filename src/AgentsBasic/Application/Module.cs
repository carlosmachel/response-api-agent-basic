using Microsoft.AspNetCore.Mvc;

namespace AgentsBasic.Application;

public static class Module
{
    public static void Register(this IEndpointRouteBuilder app)
    {
        app.MapPost("/ai-agent", async (
                [FromServices] Service service,
                [FromQuery] string name, [FromQuery] string instructions) =>
            {
                var agentId = await service.CreateAgentAsync(
                    name, 
                    instructions);
                return Results.Ok(agentId);
            })
            .WithTags("Ai Agents");
        
        
        app.MapGet("/ai-agent/create-conversation", async (
                [FromServices] Service service) =>
            {
                var agentId = await service.CreateConversationAsync();
                return Results.Ok(agentId);
            })
            .WithTags("Ai Agents");
        
        
        app.MapGet("/ai-agent/response", async (
                [FromServices] Service service,
                [FromQuery] string agentName,
                [FromQuery] string conversationId,
                [FromQuery] string? previousResponseId,
                [FromQuery] string userInput) =>
            {
                var result = await service.ResponseAsync(agentName, conversationId, userInput, previousResponseId);
                return Results.Ok(result);
            })
            .WithTags("Ai Agents");
    }
}