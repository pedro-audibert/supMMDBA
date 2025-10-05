\# Sistema Supervisório MMDBA



\## 📄 Descrição do Projeto



\[cite\_start]O Sistema Supervisório é uma plataforma integrada de gestão e monitoramento de máquinas industriais, desenvolvida para atender às necessidades de pequenas e médias empresas (PMEs)\[cite: 1]. \[cite\_start]O principal objetivo é fornecer uma solução acessível e adaptável que centraliza o acompanhamento das operações em tempo real e a implementação de planos de manutenção, liberando os gestores de cargas operacionais para focarem no crescimento estratégico do negócio\[cite: 1].



\[cite\_start]A arquitetura do sistema é composta por um dashboard web conectado a um banco de dados PostgreSQL e ao Node-RED, que atua como um gateway IIoT para se comunicar com os Controladores Lógicos Programáveis (CLPs) no chão de fábrica, garantindo uma coleta de dados eficiente e contínua\[cite: 1, 5].



\## ✨ Funcionalidades Principais



\[cite\_start]O sistema é estruturado em quatro pilares principais para facilitar a compreensão e o uso\[cite: 2, 3]:



\###  Pillar I: Acesso e Perfil do Usuário

\- \[cite\_start]\*\*Login e Segurança (RF1):\*\* Módulo completo de autenticação construído sobre o ASP.NET Core Identity, com registro, login seguro, recuperação de senha, suporte a autenticação de dois fatores (2FA) e portal de gerenciamento de dados pessoais (LGPD)\[cite: 2, 3].

\- \[cite\_start]\*\*Casos de Uso:\*\* Autenticar Usuário (UC1), Redefinir Senha (UC2), Gerenciar perfil (UC3, UC4, UC5) e 2FA (UC6)\[cite: 3, 4].



\###  Pillar II: Supervisão de Operações e Diagnóstico

\- \[cite\_start]\*\*Supervisão de Produção em Tempo Real (RF5):\*\* Dashboard principal que exibe KPIs essenciais como status da máquina, alarmes, velocidade e gráficos de tendência da última hora\[cite: 2].

\- \[cite\_start]\*\*Painel de Diagnóstico de I/O (RF3):\*\* Ferramenta de baixo nível para equipes de manutenção, exibindo em tempo real o estado de todas as entradas e saídas (I/Os) do CLP para rápida identificação de falhas\[cite: 2, 3].

\- \[cite\_start]\*\*Histórico e Rastreabilidade de Eventos (RF4):\*\* Tela para análise de todos os eventos operacionais (Alarmes, Avisos, Status) com filtros avançados por tipo e período\[cite: 2, 3].



\###  Pillar III: Análise Gerencial de Performance

\- \[cite\_start]\*\*Análise de OEE (Overall Equipment Effectiveness) (RF8):\*\* Calcula e visualiza os três pilares da eficiência industrial: Disponibilidade, Performance e Qualidade\[cite: 2, 3].

\- \[cite\_start]\*\*Emissão de Relatórios (RF6):\*\* Consolida dados históricos em relatórios gerenciais, como ocorrência de alarmes, tempo de parada (downtime) e produtividade\[cite: 2, 3].



\### Pillar IV: Administração e Configuração do Sistema

\- \[cite\_start]\*\*Gestão de Usuários e Permissões (RF9):\*\* Painel administrativo para gerenciar usuários e atribuir papéis (Administrador, Operador, etc.)\[cite: 2, 3].

\- \[cite\_start]\*\*Sistema de Notificações Proativas (RF7):\*\* Envia alertas automáticos por e-mail ou outros canais para eventos críticos, garantindo uma resposta rápida\[cite: 2, 3].

\- \[cite\_start]\*\*Coleta de Dados (IIoT) (RF2):\*\* Camada de aquisição de dados via Node-RED que se conecta aos CLPs, formata os dados em JSON e os envia de forma segura para a API\[cite: 2, 5].



\## 🛠️ Pilha Tecnológica (Stack)



\* \[cite\_start]\*\*Backend:\*\* C# com ASP.NET Core 8 (API RESTful e Razor Pages) \[cite: 5]

\* \[cite\_start]\*\*Frontend:\*\* JavaScript (ES6+), Chart.js, Bootstrap \[cite: 5]

\* \[cite\_start]\*\*Banco de Dados:\*\* PostgreSQL \[cite: 5]

\* \[cite\_start]\*\*ORM:\*\* Entity Framework Core 8 \[cite: 5]

\* \[cite\_start]\*\*Comunicação em Tempo Real:\*\* SignalR \[cite: 5]

\* \[cite\_start]\*\*Gateway IIoT:\*\* Node-RED \[cite: 5]

\* \[cite\_start]\*\*Segurança:\*\* ASP.NET Core Identity \[cite: 5]



\## 🚀 Como Começar



\*(Esta seção é um espaço reservado para você adicionar instruções sobre como configurar e rodar o projeto localmente).\*



1\.  \*\*Pré-requisitos:\*\*

&nbsp;   \* SDK do .NET 8

&nbsp;   \* PostgreSQL

&nbsp;   \* Node.js e Node-RED

2\.  \*\*Configuração do Backend:\*\*

&nbsp;   \* Clone o repositório: `git clone https://github.com/pedro-audibert/supMMDBA.git`

&nbsp;   \* Configure os segredos do usuário (connection string, etc.) usando o Secret Manager.

&nbsp;   \* Execute as migrações do Entity Framework.

3\.  \*\*Execução:\*\*

&nbsp;   \* Inicie a aplicação a partir do Visual Studio ou via linha de comando com `dotnet run`.

