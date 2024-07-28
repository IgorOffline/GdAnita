namespace AnitaBusiness.BusinessMain.BusinessLogging;

public class NoopLogger : ILogger
{
    public LogLevel LogLevel { get; set; }
    
    public void Print(string s)
    {
        // Noop
    }
}