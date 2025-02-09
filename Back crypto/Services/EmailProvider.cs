using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Crypto.Services
{
    public class EmailProvider
    {
        private readonly string _host = "http://localhost:5000"; // Exemple d'URL de ton API
        private readonly ILogger<EmailProvider> _logger;
        private readonly ExternalApiService _externalApiService;
        private readonly ITokenValidator _tokenValidator;
        private readonly DataContext _context;

        public EmailProvider(DataContext context, ILogger<EmailProvider> logger, ExternalApiService apiService,ITokenValidator tokenValid)
        {
            _context = context;
            _logger = logger;
            _externalApiService = apiService;
            _tokenValidator = tokenValid;
        }

        public async Task SendEmailAutorisationVente(string tokenTransac,string email)
        {
            var location = _host + $"/api/compte/validation?token={tokenTransac}";

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse("yoahndaniel37@gmail.com"));
            emailMessage.To.Add(MailboxAddress.Parse(email));
            emailMessage.Subject = "Validation de votre transaction";
            emailMessage.Body = new TextPart("plain")
            {
                Text = $"Veuillez cliquer sur ce lien afin de valider votre operation: {location}"
            };

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, false);
                await smtpClient.AuthenticateAsync("yoahndaniel37@gmail.com", "your-password"); // Utilise un mot de passe d'application
                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
