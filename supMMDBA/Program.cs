/*
================================================================================================
ARQUIVO: Program.cs
FUNÇÃO:  Configuração principal da aplicação ASP.NET Core 6+, incluindo:
         1. Carregamento de Configurações (incluindo segredos)
         2. Registro de Serviços na Injeção de Dependência (Banco de Dados, Identity, SignalR, etc.)
         3. Configuração do Pipeline de Middlewares HTTP (requisições)
         4. Mapeamento de Rotas e Endpoints (MVC, Razor Pages, Hubs)
================================================================================================
*/

#region NAMESPACES
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mmdba.Data;
using mmdba.Hubs;
using mmdba.Models;
using System;
using System.Globalization;
using System.IO;
#endregion

// ================================================================================================
// PASSO 1: CRIAR O CONSTRUTOR DA APLICAÇÃO (Application Builder)
// ================================================================================================
var builder = WebApplication.CreateBuilder(args);


// ================================================================================================
// PASSO 2: CARREGAR CONFIGURAÇÕES ADICIONAIS
// Esta é a correção principal que fizemos.
// Garante que, em ambiente de desenvolvimento, as configurações do 'secrets.json' sejam
// carregadas na memória. Deve ser uma das primeiras coisas a serem feitas.
// ================================================================================================
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}


// ================================================================================================
// PASSO 3: REGISTRAR SERVIÇOS NA INJEÇÃO DE DEPENDÊNCIA (Dependency Injection)
// Aqui, informamos ao .NET quais serviços e classes nossa aplicação irá usar.
// ================================================================================================

#region Registro de Serviços

// --- Infraestrutura: Data Protection ---
// Configura a proteção de dados para persistir chaves de criptografia (usadas para cookies, tokens, etc.)
// em uma pasta específica, garantindo que elas não se percam quando a aplicação reiniciar.
var dataProtectionPath = Path.Combine(builder.Environment.ContentRootPath, ".dp_keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath));

// --- Banco de Dados e Autenticação (Identity) ---
// Lê a string de conexão. Em desenvolvimento, virá do 'secrets.json'; em produção, de variáveis de ambiente.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

// Configura o Entity Framework Core para usar o PostgreSQL (Npgsql) como provedor de banco de dados.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configura o sistema de usuários e autenticação (ASP.NET Core Identity).
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<IdentityErrorDescriberPtBr>(); // Adiciona descrições de erro em português.

// --- Serviços de Negócio e Opções de Configuração ---
// Registra nossa implementação customizada de IEmailSender para envio de e-mails.
builder.Services.AddTransient<IEmailSender, mmdba.Services.EmailSender>();

// Usa o "Options Pattern": vincula a seção "AuthMessageSenderOptions" do appsettings.json
// a uma classe C# (AuthMessageSenderOptions), facilitando o acesso a essas configurações de forma tipada.
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("AuthMessageSenderOptions"));

// --- Camada de Apresentação (MVC, Razor Pages, Localização) ---
// Habilita os recursos para aplicações MVC e Razor Pages.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(); // Suporte para tradução nas Views.

builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(); // Suporte para tradução em anotações de validação (ex: [Required]).

// Habilita a compilação em tempo de execução das Views em ambiente de desenvolvimento.
// Isso permite alterar um arquivo .cshtml e ver o resultado no navegador sem precisar reiniciar a aplicação.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}

// --- Serviços de Comunicação em Tempo Real e Sessão ---
builder.Services.AddSignalR(); // Habilita o SignalR para comunicação real-time (WebSockets).
builder.Services.AddDistributedMemoryCache(); // Cache em memória necessário para o serviço de Sessão.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

// --- Serviços em Background (Ex: MQTT) ---
// Registra o serviço de MQTT para rodar em segundo plano. Mantido comentado como no original.
//builder.Services.AddHostedService<MqttSubscriberService>();

// --- Serviço de Localização (Tradução) ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

#endregion


// ================================================================================================
// PASSO 4: CONSTRUIR A APLICAÇÃO
// A variável 'app' representa a aplicação web configurada, pronta para receber requisições.
// ================================================================================================
var app = builder.Build();


// ================================================================================================
// PASSO 5: TAREFAS DE INICIALIZAÇÃO PÓS-CONSTRUÇÃO
// Este bloco executa tarefas assim que a aplicação é construída, mas antes de começar a
// aceitar requisições. Útil para, por exemplo, aplicar migrações do banco de dados.
// ================================================================================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


// ================================================================================================
// PASSO 6: CONFIGURAR O PIPELINE DE MIDDLEWARES HTTP
// Middlewares são componentes que processam uma requisição HTTP.
// A ORDEM DELES É EXTREMAMENTE IMPORTANTE. A requisição passa por eles em sequência.
// ================================================================================================

#region Configuração do Pipeline (Middlewares)

// --- Localização (Cultura) ---
var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// --- Tratamento de Erros ---
// Em ambiente de produção, usa uma página de erro amigável. Em DEV, mostra a página de exceção detalhada.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Força o uso de HTTPS.
}

// --- Middlewares Essenciais ---
app.UseHttpsRedirection(); // Redireciona requisições HTTP para HTTPS.
app.UseStaticFiles(); // Habilita o serviço de arquivos estáticos (CSS, JS, imagens).
app.UseRouting(); // Habilita o sistema de roteamento do ASP.NET Core.
app.UseSession(); // Habilita o middleware de sessão.

// --- Autenticação e Autorização ---
// IMPORTANTE: UseAuthentication DEVE vir antes de UseAuthorization.
app.UseAuthentication(); // Verifica a identidade do usuário.
app.UseAuthorization(); // Verifica se o usuário autenticado tem permissão para acessar um recurso.

#endregion


// ================================================================================================
// PASSO 7: MAPEAMENTO DE ROTAS E ENDPOINTS
// Define quais URLs correspondem a quais partes da aplicação (Controllers, Razor Pages, Hubs).
// ================================================================================================

#region Rotas e Endpoints

// Mapeia os endpoints necessários para as páginas do Identity (Login, Registro, etc.).
app.MapRazorPages();

// Mapeia a rota padrão para o padrão MVC: {controller}/{action}/{id?}.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapeia os Hubs do SignalR para seus respectivos endpoints.
app.MapHub<AlarmesHub>("/alarmesHub");
app.MapHub<IOsHub>("/iosHub");
app.MapHub<AvisosHub>("/avisosHub");
app.MapHub<DadosHub>("/dadosHub");
app.MapHub<StatusHub>("/statusHub");
app.MapHub<VelocidadeHub>("/velocidadeHub");
app.MapHub<ContagemHub>("/contagemHub");

#endregion


// ================================================================================================
// PASSO 8: EXECUTAR A APLICAÇÃO
// Inicia o servidor web e começa a escutar por requisições HTTP.
// ================================================================================================
app.Run();