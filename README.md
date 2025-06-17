# poc-datadog-traces-calculator
Calculadora de Tabuada com Instrumentação Datadog
📋 Descrição
Uma aplicação de console .NET que calcula tabuadas e demonstra instrumentação avançada com o Datadog APM (Application Performance Monitoring). Este projeto é especialmente notável por funcionar com a compilação AOT (Ahead of Time), que normalmente seria incompatível com bibliotecas de instrumentação.

✨ Características
Calcula a tabuada completa (1 a 10) para qualquer número informado
Instrumenta cada parte do processo com spans do Datadog
Mede e reporta o tempo de execução de cada operação
Registra valores de entrada/saída e estatísticas
Funciona com compilação AOT (Ahead of Time)
🔍 Como funciona a instrumentação
O código implementa uma estratégia de rastreamento em múltiplos níveis:

Span raiz (tabuada.calculator): Envolve toda a operação, desde a entrada do usuário até a exibição dos resultados
Span de análise (parse_input): Rastreia a conversão da entrada do usuário para um número
Tags: input, parsed_number, error (quando aplicável)
Span de cálculo (calculate_tabuada): Monitora o processo completo de geração da tabuada
Tags: number, total_result, calculation_time_ms
Spans individuais (multiply): Um span para cada operação de multiplicação na tabuada
Tags: multiplier, multiplicand, result
Cada span fornece contexto detalhado sobre a operação, permitindo analisar o desempenho e comportamento da aplicação.

🚀 Compilação e execução
Requisitos
.NET 8 SDK
Datadog Agent configurado e rodando (localmente ou em container)
Comandos

# Compilação normal
dotnet build

# Execução direta
dotnet run

# Publicação com AOT (Ahead of Time)
dotnet publish -r win-x64 -c Release --self-contained

📦 Estrutura da aplicação
A aplicação segue um fluxo simples:

Solicita um número ao usuário
Valida a entrada (verifica se é um número válido)
Calcula a tabuada de 1 a 10 para o número informado
Exibe os resultados com estatísticas de tempo
Repete até que o usuário digite "sair"
🔧 Configuração do Datadog
O projeto usa a biblioteca oficial Datadog.Trace e o pacote Datadog.Trace.Trimming para compatibilidade com AOT.

<ItemGroup>
  <PackageReference Include="Datadog.Trace" Version="2.40.0" />
  <PackageReference Include="Datadog.Trace.Trimming" Version="2.40.0-prerelease" />
</ItemGroup>


📊 Visualização no Datadog
Após a execução, os dados podem ser visualizados no Datadog:

Acesse o painel do Datadog
Navegue até APM > Traces
Procure pelos traces com nome de serviço padrão ou filtre por operação tabuada.calculator
Explore a hierarquia dos spans para analisar o desempenho de cada etapa
💡 Detalhes técnicos importantes
A aplicação usa Thread.Sleep(50) para simular processamento em cada operação
Ao final, aguarda 5 segundos para garantir o envio de telemetria pendente
Usa spans aninhados para representar a estrutura hierárquica das operações
Cada span registra metadados relevantes como tags
⚙️ Compatibilidade com AOT
Um dos destaques deste projeto é sua compatibilidade com compilação AOT (Ahead of Time), que:

Reduz significativamente o tempo de inicialização da aplicação
Diminui o uso de memória
Permite a distribuição como um único arquivo executável nativo
Normalmente seria incompatível com bibliotecas que utilizam reflexão (como a maioria das bibliotecas de APM)
Este projeto resolve esse problema utilizando o pacote experimental Datadog.Trace.Trimming.

🔨 Resolução de problemas
Se os traces não aparecerem no Datadog, verifique se o Datadog Agent está rodando
Certifique-se de que a porta 8126 (padrão do APM) está acessível
Aumente o tempo de espera ao final da aplicação se necessário (atualmente 5 segundos)
Para diagnóstico, verifique os logs do Agent do Datadog
Este projeto serve como exemplo prático de como implementar monitoramento detalhado em uma aplicação .NET, mesmo com restrições de compilação como AOT.# poc-traces-calculator
