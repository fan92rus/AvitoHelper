namespace AvitoHelper.Services
{
    public interface IEmailSender
    {
        void Execute(string login, string pass, string to, string subject, string text);
    }
}