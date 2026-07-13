namespace SGI.Application.Interfaces
{
    public interface ILoggerService
    {
        void LogInfo(string mensaje);
        void LogWarning(string mensaje);
        void LogError(Exception ex, string mensaje);
    }
}