using Azure.AI.Projects;
using Azure.AI.Projects.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Options;
using OpenAI.Responses;

#pragma warning disable OPENAI001
#pragma warning disable CA2252

namespace AgentsBasic.Application;

public record AgentResult(string AgentName, string Version);

public record ResponseResult(string OutputText, string Id);

public class Service(IOptions<AzureAiSettings> settings)
{
    private AIProjectClient GetProjectClient()
    {
        return new AIProjectClient(
            new Uri(settings.Value.Uri),  
            new DefaultAzureCredential());  
    }

    public async Task<AgentResult> CreateAgentAsync(
        string agentName,
        string instructions)
    {
        var client = GetProjectClient();
        var creationOptions = new AgentVersionCreationOptions(
            new PromptAgentDefinition(model: settings.Value.Model)
            {
                Instructions = instructions
            });
        AgentVersion agent = await client.Agents.CreateAgentVersionAsync(agentName: agentName, creationOptions);
        return new AgentResult(agent.Name, agent.Version);
    }

    public async Task<string> CreateConversationAsync()
    {
        var client = GetProjectClient();
        var options = new ProjectConversationCreationOptions();
        var conversation = await client
            .OpenAI
            .Conversations
            .CreateProjectConversationAsync(options);
        return conversation.Value.Id;
    }
    
    public async Task<ResponseResult> ResponseAsync(
        string agentName,
        string conversationId,
        string userInput,
        string? previousResponseId = null)
    {
        var client = GetProjectClient();
        AgentRecord record = await client.Agents.GetAgentAsync(agentName);
        var responseClient = client.OpenAI.GetProjectResponsesClientForAgent(record, conversationId);
        var options = new CreateResponseOptions(
            [ResponseItem.CreateUserMessageItem(userInput)],
            settings.Value.Model)
        {
            //ConversationOptions = new ResponseConversationOptions(conversationId),
            //PreviousResponseId = previousResponseId
        };

        var teste = client.OpenAI.Conversations.GetProjectConversationsAsync(record);
        await foreach(var t in teste)
        {
            Console.WriteLine(t);
        }
        var result = await responseClient.CreateResponseAsync(options);
        return new ResponseResult(result.Value.GetOutputText(), result.Value.Id);
    }
}