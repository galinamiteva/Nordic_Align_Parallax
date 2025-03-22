namespace Nordic_Align_V7.Services;

public interface IMailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
