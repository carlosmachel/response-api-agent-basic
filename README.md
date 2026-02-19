# response-api-agent-basic

API .NET minimal para demonstrar criacao de agentes com Azure AI Projects (Foundry) e respostas via OpenAI Responses.

## Visao geral
- Cria versoes de agentes com instrucoes personalizadas.
- Abre conversas no projeto.
- Envia mensagens e retorna a resposta do agente.

## Pre-requisitos
- .NET SDK (net10.0).
- Uma instancia do Azure AI Projects com acesso habilitado.
- Credenciais Azure disponiveis via `DefaultAzureCredential` (Azure CLI, VS Code, Managed Identity, etc.).

## Configuracao
Edite `src/AgentsBasic/appsettings.json` (ou `appsettings.Development.json`) com seus valores:

```json
{
  "AzureAiSettings": {
    "Model": "SEU_MODELO",
    "Uri": "https://SEU_ENDPOINT"
  }
}
```

## Como executar
Na raiz do repositorio:

```zsh
dotnet restore
```

```zsh
dotnet run --project src/AgentsBasic/AgentsBasic.csproj
```

Em ambiente de desenvolvimento, a documentacao OpenAPI e a UI do Scalar ficam disponiveis automaticamente.

## Endpoints
Baseados em `src/AgentsBasic/Application/Module.cs`.

### Criar agente
`POST /ai-agent?name=...&instructions=...`

Resposta: nome e versao do agente.

### Criar conversa
`GET /ai-agent/create-conversation`

Resposta: `conversationId`.

### Obter resposta do agente
`GET /ai-agent/response?agentName=...&conversationId=...&userInput=...&previousResponseId=...`

- `previousResponseId` e opcional.
- Retorna o texto de saida e o id da resposta.

## Notas
- O acesso usa `DefaultAzureCredential`, entao configure suas credenciais antes de executar.
- Ajuste o modelo em `AzureAiSettings.Model` conforme a disponibilidade do seu projeto.
