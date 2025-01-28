namespace AssetsMonitor.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendAlertEmailAsync(string subject, string symbol, decimal price, string action);
    }
}
