namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        try
        {
            string diretorioLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(diretorioLog))
            {
                Directory.CreateDirectory(diretorioLog);
            }

            string caminhoArquivoLog = Path.Combine(diretorioLog, "api_log.txt");
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}");
            }
        }
        catch (Exception ex)
        {
            // Se não conseguir escrever no arquivo, pelo menos tentamos registrar no console
            Console.WriteLine($"Erro ao escrever no arquivo de log: {ex.Message}");
            Console.WriteLine($"Mensagem original: {mensagem}");
        }
    }
}
