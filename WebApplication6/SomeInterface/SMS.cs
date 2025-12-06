namespace WebApplication6.SomeInterface
{
    public interface ISMSService
    {
        Task SendSmsAsync(string to, string message);
    }

}
