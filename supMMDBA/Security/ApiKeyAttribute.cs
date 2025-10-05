/*
=========================================================================================================
ARQUIVO: Filters/ApiKeyAttribute.cs (Exemplo de localização)
FUNÇÃO:  Implementa um filtro de autorização customizado para validar chaves de API.
         Este atributo pode ser aplicado a um Controller inteiro ou a um método de API específico
         para protegê-lo contra acessos não autorizados.

COMO FUNCIONA:
1.  Intercepta a requisição antes que ela chegue ao método do controller (Action).
2.  Procura por um cabeçalho (header) HTTP chamado "X-API-Key".
3.  Compara o valor deste cabeçalho com a chave de API segura armazenada na configuração
    da aplicação (seja em 'appsettings.json', 'secrets.json' ou variáveis de ambiente).
4.  Se a chave for válida, a requisição prossegue para o controller.
5.  Se a chave estiver ausente ou for inválida, a requisição é bloqueada imediatamente com um
    status HTTP 401 Unauthorized.

COMO USAR:
Basta decorar a classe do Controller ou o método da Action com o atributo [ApiKey].
Exemplo:
[ApiController]
[ApiKey]
public class MeuApiController : ControllerBase
{
    // ...
}
=========================================================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

// Define que este atributo pode ser usado em classes de controller inteiras ou em métodos individuais.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    // Define o nome padrão do cabeçalho HTTP que conterá a chave da API.
    private const string ApiKeyHeaderName = "X-API-Key";

    /// <summary>
    /// Este método é executado automaticamente pelo ASP.NET Core antes de um método de ação do controller.
    /// </summary>
    /// <param name="context">O contexto da ação, contém informações sobre a requisição HTTP.</param>
    /// <param name="next">Um delegate que executa o próximo filtro ou o próprio método da ação.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // --- PASSO 1: Extrair a chave de API do cabeçalho da requisição ---
        // Tenta obter o valor do cabeçalho "X-API-Key".
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            // Se o cabeçalho não for encontrado, encerra a requisição imediatamente.
            context.Result = new UnauthorizedObjectResult("Chave de API ausente no cabeçalho (X-API-Key).");
            return;
        }

        // --- PASSO 2: Obter o serviço de configuração da aplicação ---
        // Pede ao provedor de serviços da aplicação uma instância do IConfiguration.
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        // --- PASSO 3: Obter a chave de API esperada a partir da configuração ---
        // Lê o valor da chave "Authentication:ApiKey" da configuração.
        // O .NET buscará essa chave em todas as suas fontes: appsettings.json, secrets.json, etc.
        var apiKey = configuration.GetValue<string>("Authentication:ApiKey");

        // --- PASSO 4: Validar a chave de API ---
        // Compara a chave fornecida na requisição com a chave esperada da configuração.
        // A verificação 'apiKey == null' é crucial para tratar o caso em que a chave
        // não está configurada no servidor, evitando erros de referência nula.
        if (apiKey == null || !apiKey.Equals(potentialApiKey.ToString()))
        {
            // Se a chave do servidor não existe ou se as chaves não coincidem, rejeita a requisição.
            context.Result = new UnauthorizedObjectResult("Chave de API inválida.");
            return;
        }

        // --- PASSO 5: Permitir a continuação da requisição ---
        // Se a chave de API for válida, chama o próximo passo no pipeline (outro filtro ou o método do controller).
        await next();
    }
}