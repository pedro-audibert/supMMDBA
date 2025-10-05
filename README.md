# Sistema Supervisório MMDBA

## 📄 Descrição do Projeto

O Sistema Supervisório é uma plataforma integrada de gestão e monitoramento de máquinas industriais, desenvolvida para atender às necessidades de pequenas e médias empresas (PMEs). O principal objetivo é fornecer uma solução acessível e adaptável que centraliza o acompanhamento das operações em tempo real e a implementação de planos de manutenção, liberando os gestores de cargas operacionais para focarem no crescimento estratégico do negócio.

A arquitetura do sistema é composta por um dashboard web conectado a um banco de dados PostgreSQL e ao Node-RED, que atua como um gateway IIoT para se comunicar com os Controladores Lógicos Programáveis (CLPs) no chão de fábrica, garantindo uma coleta de dados eficiente e contínua.

## ✨ Funcionalidades Principais

O sistema é estruturado em quatro pilares principais para facilitar a compreensão e o uso:

###  Pillar I: Acesso e Perfil do Usuário
- **Login e Segurança (RF1):** Módulo completo de autenticação construído sobre o ASP.NET Core Identity, com registro, login seguro, recuperação de senha, suporte a autenticação de dois fatores (2FA) e portal de gerenciamento de dados pessoais (LGPD).
- **Casos de Uso:** Autenticar Usuário (UC1), Redefinir Senha (UC2), Gerenciar perfil (UC3, UC4, UC5) e 2FA (UC6).

###  Pillar II: Supervisão de Operações e Diagnóstico
- **Supervisão de Produção em Tempo Real (RF5):** Dashboard principal que exibe KPIs essenciais como status da máquina, alarmes, velocidade e gráficos de tendência da última hora.
- **Painel de Diagnóstico de I/O (RF3):** Ferramenta de baixo nível para equipes de manutenção, exibindo em tempo real o estado de todas as entradas e saídas (I/Os) do CLP para rápida identificação de falhas.
- **Histórico e Rastreabilidade de Eventos (RF4):** Tela para análise de todos os eventos operacionais (Alarmes, Avisos, Status) com filtros avançados por tipo e período.

### Pillar III: Análise Gerencial de Performance
- **Análise de OEE (Overall Equipment Effectiveness) (RF8):** Calcula e visualiza os três pilares da eficiência industrial: Disponibilidade, Performance e Qualidade.
- **Emissão de Relatórios (RF6):** Consolida dados históricos em relatórios gerenciais, como ocorrência de alarmes, tempo de parada (downtime) e produtividade.

### Pillar IV: Administração e Configuração do Sistema
- **Gestão de Usuários e Permissões (RF9):** Painel administrativo para gerenciar usuários e atribuir papéis (Administrador, Operador, etc.).
- **Sistema de Notificações Proativas (RF7):** Envia alertas automáticos por e-mail ou outros canais para eventos críticos, garantindo uma resposta rápida.
- **Coleta de Dados (IIoT) (RF2):** Camada de aquisição de dados via Node-RED que se conecta aos CLPs, formata os dados em JSON e os envia de forma segura para a API.

## 🛠️ Pilha Tecnológica (Stack)

* **Backend:** C# com ASP.NET Core 8 (API RESTful e Razor Pages)
* **Frontend:** JavaScript (ES6+), Chart.js, Bootstrap
* **Banco de Dados:** PostgreSQL
* **ORM:** Entity Framework Core 8
* **Comunicação em Tempo Real:** SignalR
* **Gateway IIoT:** Node-RED
* **Segurança:** ASP.NET Core Identity

## 🚀 Como Começar

*(Esta seção é um espaço reservado para você adicionar instruções sobre como configurar e rodar o projeto localmente).*

1.  **Pré-requisitos:**
    * SDK do .NET 8
    * PostgreSQL
    * Node.js e Node-RED
2.  **Configuração do Backend:**
    * Clone o repositório: `git clone https://github.com/pedro-audibert/supMMDBA.git`
    * Configure os segredos do usuário (connection string, etc.) usando o Secret Manager.
    * Execute as migrações do Entity Framework.
3.  **Execução:**
    * Inicie a aplicação a partir do Visual Studio ou via linha de comando com `dotnet run`.
