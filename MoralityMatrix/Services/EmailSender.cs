using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace MoralityMatrix.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        // Set the API Keys using the AuthMessageSenderOptions Object and .NET Secret Manager
        /// <summary>
        /// On Windows, Secret Manager stores keys/value pairs in a secrets.json file in the %APPDATA%/Microsoft/UserSecrets/<WebAppName-userSecretsId> directory.
        /// </summary>
        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Check if any of the API Keys are NULL and if so throw an exception
            if (string.IsNullOrEmpty(Options.MailJetPublicKey))
            {
                throw new Exception("Null MailJetPublicKey");
            }
            if (string.IsNullOrEmpty(Options.MailJetPrivateKey))
            {
                throw new Exception("Null MailJetPrivateKey");
            }
            await Execute(Options.MailJetPrivateKey, subject, message, toEmail);
        }

        public async Task Execute(string secretKey, string subject, string message, string toEmail)
        {
            /// <summary>
            /// This project is running Mailjet.Api version 3.0.0
            /// 
            /// MailJet Documentation
            /// https://dev.mailjet.com/email/guides/send-api-V3/
            /// 
            /// Microsoft Documentation
            /// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-8.0&tabs=visual-studio
            /// </summary>
            MailjetClient client = new MailjetClient(Options.MailJetPublicKey, secretKey);


            /// Change the sender information according to your MailJet account setup
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "morality.matrix.2024@gmail.com")
            .Property(Send.FromName, "Morality Matrix")
            .Property(Send.Subject, subject)
            .Property(Send.TextPart, message)
            .Property(Send.HtmlPart, message)
            .Property(Send.Recipients, new JArray {
            new JObject {
                {"Email", toEmail}
                }
            });
            MailjetResponse response = await client.PostAsync(request);

            // Return the SuccessStatusCode in the Output Console for debugging
            _logger.LogInformation(response.IsSuccessStatusCode
                ? $"Email to {toEmail} queued successfully!"
                : $"Failure Email to {toEmail}");
        }
    }
}