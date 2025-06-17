using Datadog.Trace;
using System;
using System.Diagnostics;
using System.Threading;

// Loop principal
while (true)
{
    // Usar um span para o programa inteiro
    using var rootScope = Tracer.Instance.StartActive("tabuada.calculator");

    Console.Write("\nDigite um número para calcular a tabuada (ou 'sair' para encerrar): ");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input) || input.ToLower() == "sair")
    {
        break;
    }

    // Tentar converter a entrada
    using var parseScope = Tracer.Instance.StartActive("parse_input");
    parseScope.Span.SetTag("input", input);

    if (!int.TryParse(input, out int number))
    {
        Console.WriteLine("Entrada inválida! Por favor, digite um número.");
        parseScope.Span.SetTag("error", "true");
        parseScope.Span.SetTag("error.msg", "Entrada inválida");
        continue;
    }

    parseScope.Span.SetTag("parsed_number", number);
    parseScope.Close();

    // Medir o tempo de cálculo
    var sw = Stopwatch.StartNew();

    // Calcular a tabuada
    using var calcScope = Tracer.Instance.StartActive("calculate_tabuada");
    calcScope.Span.SetTag("number", number);

    Console.WriteLine($"\nTabuada do {number}:");
    Console.WriteLine("------------------");

    int totalResult = 0; // Para adicionar alguma estatística

    for (int i = 1; i <= 10; i++)
    {
        // Criar um span para cada multiplicação
        using var multiplyScope = Tracer.Instance.StartActive("multiply");
        multiplyScope.Span.SetTag("multiplier", i);
        multiplyScope.Span.SetTag("multiplicand", number);

        int result = number * i;
        totalResult += result;

        multiplyScope.Span.SetTag("result", result);

        // Simular algum processamento
        Thread.Sleep(50);

        Console.WriteLine($"{number} x {i} = {result}");
    }

    sw.Stop();

    calcScope.Span.SetTag("total_result", totalResult);
    calcScope.Span.SetTag("calculation_time_ms", sw.ElapsedMilliseconds);

    Console.WriteLine("------------------");
    Console.WriteLine($"Cálculo concluído em {sw.ElapsedMilliseconds}ms");
    Console.WriteLine($"Soma de todos os resultados: {totalResult}");

    // Adicionar uma estatística customizada ao span raiz
    rootScope.Span.SetTag("operation", "tabuada");
    rootScope.Span.SetTag("input_number", number);
    rootScope.Span.SetTag("execution_time_ms", sw.ElapsedMilliseconds);
}

Console.WriteLine("\nPrograma encerrado, enviando dados ao Datadog...");

// IMPORTANTE: Dar tempo para o tracer enviar os dados antes de encerrar
try
{
    // Apenas aguardar para garantir que os dados sejam enviados
    Console.WriteLine("Aguardando envio de telemetria (5 segundos)...");
    Thread.Sleep(5000);
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao finalizar tracer: {ex.Message}");
}

Console.WriteLine("Pressione qualquer tecla para sair.");
Console.ReadKey();