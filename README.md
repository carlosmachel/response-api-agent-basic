# response-api-agent-basic

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Azure AI](https://img.shields.io/badge/Azure-AI%20Projects-0078D4?logo=microsoft-azure)](https://azure.microsoft.com/en-us/products/ai-services)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

API .NET minimal para demonstrar criação de agentes com Azure AI Projects (Foundry) e respostas via OpenAI Responses.

## 📋 Visão Geral

Este projeto é uma API minimalista desenvolvida em .NET 10 que demonstra a integração com Azure AI Projects (anteriormente conhecido como Azure AI Foundry). A aplicação permite:

- ✨ Criar versões de agentes de IA com instruções personalizadas
- 💬 Abrir e gerenciar conversas no projeto Azure AI
- 🤖 Enviar mensagens e obter respostas contextualizadas dos agentes
- 📚 Documentação automática com OpenAPI e interface Scalar

## 🎯 Características

- **Arquitetura Minimal API**: Utiliza o padrão Minimal API do ASP.NET Core para endpoints enxutos e performáticos
- **Azure AI Projects**: Integração nativa com Azure AI Projects para gerenciamento de agentes
- **OpenAI Responses**: Utiliza o formato de respostas do OpenAI para comunicação estruturada
- **Autenticação Azure**: Suporte a DefaultAzureCredential para múltiplos métodos de autenticação
- **Documentação Interativa**: Interface Scalar para exploração e teste dos endpoints

## 🔧 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior
- Uma instância do [Azure AI Projects](https://azure.microsoft.com/en-us/products/ai-services) com acesso habilitado
- Credenciais Azure configuradas via `DefaultAzureCredential`:
  - [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) (recomendado para desenvolvimento local)
  - Visual Studio Code com extensão Azure
  - Managed Identity (para ambientes de produção)
  - Service Principal com variáveis de ambiente

## ⚙️ Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/carlosmachel/response-api-agent-basic.git
cd response-api-agent-basic
```

### 2. Configure as credenciais Azure

Faça login no Azure CLI:

```bash
az login
```

### 3. Configure o arquivo appsettings.json

Edite `src/AgentsBasic/appsettings.json` ou crie `appsettings.Development.json` com suas configurações:

```json
{
  "AzureAiSettings": {
    "Model": "gpt-4o",
    "Uri": "https://seu-projeto.openai.azure.com/"
  }
}
```

**Onde encontrar esses valores:**
- **Model**: Nome do modelo de IA disponível no seu projeto Azure AI (ex: `gpt-4o`, `gpt-4`, `gpt-35-turbo`)
- **Uri**: Endpoint do seu projeto Azure AI Projects, disponível no portal Azure

## 🚀 Como Executar

### Restaurar dependências

Na raiz do repositório:

```bash
dotnet restore
```

### Executar a aplicação

```bash
dotnet run --project src/AgentsBasic/AgentsBasic.csproj
```

### Acessar a documentação

Em ambiente de desenvolvimento, acesse:

- **Scalar UI**: http://localhost:5001/scalar/v1 (interface interativa)
- **OpenAPI JSON**: http://localhost:5001/openapi/v1.json (especificação OpenAPI)

## 📚 Endpoints da API

Todos os endpoints estão definidos em `src/AgentsBasic/Application/Module.cs`.

### 1. Criar Agente

Cria uma nova versão de agente com instruções personalizadas.

**Endpoint:** `POST /ai-agent`

**Parâmetros de Query:**
- `name` (string, obrigatório): Nome único do agente
- `instructions` (string, obrigatório): Instruções que definem o comportamento do agente

**Exemplo de Requisição:**

```bash
curl -X POST "http://localhost:5001/ai-agent?name=AssistenteVirtual&instructions=Voc%C3%AA%20%C3%A9%20um%20assistente%20virtual%20prestativo%20e%20amig%C3%A1vel"
```

**Resposta de Sucesso (200 OK):**

```json
{
  "agentName": "AssistenteVirtual",
  "version": "1.0.0"
}
```

### 2. Criar Conversa

Cria uma nova conversa no projeto.

**Endpoint:** `GET /ai-agent/create-conversation`

**Exemplo de Requisição:**

```bash
curl -X GET "http://localhost:5001/ai-agent/create-conversation"
```

**Resposta de Sucesso (200 OK):**

```json
"conv_abc123xyz456"
```

### 3. Obter Resposta do Agente

Envia uma mensagem do usuário e obtém a resposta do agente.

**Endpoint:** `GET /ai-agent/response`

**Parâmetros de Query:**
- `agentName` (string, obrigatório): Nome do agente criado anteriormente
- `conversationId` (string, obrigatório): ID da conversa criada anteriormente
- `userInput` (string, obrigatório): Mensagem do usuário
- `previousResponseId` (string, opcional): ID da resposta anterior para continuar o contexto

**Exemplo de Requisição:**

```bash
curl -X GET "http://localhost:5001/ai-agent/response?agentName=AssistenteVirtual&conversationId=conv_abc123xyz456&userInput=Ol%C3%A1%2C%20como%20voc%C3%AA%20pode%20me%20ajudar%3F"
```

**Resposta de Sucesso (200 OK):**

```json
{
  "outputText": "Olá! Sou seu assistente virtual. Posso ajudar você com diversas tarefas...",
  "id": "resp_def789ghi012"
}
```

## 📁 Estrutura do Projeto

```
response-api-agent-basic/
├── src/
│   └── AgentsBasic/
│       ├── Application/
│       │   ├── AzureAiSettings.cs    # Configurações do Azure AI
│       │   ├── Module.cs              # Definição dos endpoints
│       │   └── Service.cs             # Lógica de negócio e integração Azure
│       ├── AgentsBasic.csproj         # Arquivo de projeto .NET
│       ├── appsettings.json           # Configurações da aplicação
│       └── Program.cs                 # Ponto de entrada da aplicação
├── .gitignore
├── LICENSE
├── README.md
└── response-api-agent-basic.sln       # Solução Visual Studio
```

## 🔐 Autenticação e Segurança

A aplicação utiliza `DefaultAzureCredential` do Azure Identity, que tenta autenticar na seguinte ordem:

1. **EnvironmentCredential**: Variáveis de ambiente
2. **WorkloadIdentityCredential**: Identidade de carga de trabalho do Kubernetes
3. **ManagedIdentityCredential**: Identidade gerenciada do Azure
4. **SharedTokenCacheCredential**: Cache de token compartilhado
5. **VisualStudioCredential**: Visual Studio
6. **VisualStudioCodeCredential**: Visual Studio Code
7. **AzureCliCredential**: Azure CLI
8. **AzurePowerShellCredential**: Azure PowerShell
9. **AzureDeveloperCliCredential**: Azure Developer CLI
10. **InteractiveBrowserCredential**: Navegador interativo

Para desenvolvimento local, recomenda-se usar o Azure CLI:

```bash
az login
az account set --subscription "sua-subscription-id"
```

## 🐛 Solução de Problemas

### Erro: "Unauthorized" ou "Authentication failed"

**Solução**: Verifique se suas credenciais Azure estão configuradas corretamente:

```bash
az login
az account show
```

### Erro: "Model not found"

**Solução**: Confirme se o modelo especificado em `appsettings.json` está disponível no seu projeto Azure AI.

### Erro: "Unable to connect to Azure AI Projects"

**Solução**: Verifique se o URI está correto e se você tem permissões adequadas no projeto Azure AI.

### A documentação Scalar não está disponível

**Solução**: Certifique-se de estar executando em ambiente de desenvolvimento. A documentação só é habilitada automaticamente no modo Development.

## 🔄 Fluxo de Uso Completo

1. **Criar um agente**:
   ```bash
   POST /ai-agent?name=MeuAgente&instructions=Suas%20instru%C3%A7%C3%B5es
   ```

2. **Criar uma conversa**:
   ```bash
   GET /ai-agent/create-conversation
   ```

3. **Enviar mensagens e obter respostas**:
   ```bash
   GET /ai-agent/response?agentName=MeuAgente&conversationId=SEU_ID&userInput=Sua%20mensagem
   ```

4. **Continuar a conversa** (usando o ID da resposta anterior):
   ```bash
   GET /ai-agent/response?agentName=MeuAgente&conversationId=SEU_ID&userInput=Pr%C3%B3xima%20mensagem&previousResponseId=ID_ANTERIOR
   ```

## 📦 Dependências Principais

- **Azure.AI.Projects** (1.2.0-beta.5): SDK para Azure AI Projects
- **Azure.AI.Projects.OpenAI** (1.0.0-beta.5): Extensões OpenAI para Azure AI Projects
- **Microsoft.AspNetCore.OpenApi** (10.0.0): Suporte a OpenAPI
- **Scalar.AspNetCore** (2.0.15): Interface de documentação interativa

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE) - veja o arquivo LICENSE para mais detalhes.

## 👤 Autor

**Carlos Machel**

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:

1. Fazer um fork do projeto
2. Criar uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abrir um Pull Request

## 📝 Notas Adicionais

- O acesso ao Azure AI Projects usa `DefaultAzureCredential`, portanto, configure suas credenciais antes de executar
- Ajuste o modelo em `AzureAiSettings.Model` conforme a disponibilidade do seu projeto Azure AI
- Os pacotes Azure.AI.Projects estão em versão beta - verifique a documentação oficial para possíveis breaking changes
- Em produção, use Managed Identity ou Service Principal para autenticação

## 🔗 Links Úteis

- [Documentação Azure AI Projects](https://learn.microsoft.com/azure/ai-services/)
- [.NET 10 Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core Minimal APIs](https://docs.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Azure Identity](https://docs.microsoft.com/dotnet/api/overview/azure/identity-readme)
