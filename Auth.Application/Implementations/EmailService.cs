using Auth.Domain.Interfaces.Services;
using Auth.Service.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Service.Implementations;

public class EmailService : IEmailService
{
    private readonly SecretManager _secretManager;
    public EmailService(SecretManager secretManager)
    {
        _secretManager = secretManager;
    }

    public async Task SendEmailAsync(string destino, string titulo, string mensagem)
        => await SendEmailAsync(new List<string> { destino }, titulo, mensagem);

    public async Task SendEmailAsync(List<string> destinos, string titulo, string mensagemHtml)
    {
        var client = new SendGridClient(_secretManager.EmailAPI.Key);
        var from = new EmailAddress("contato@coopertrasmig.com", "Coopertrasmig");

        var tos = destinos.Select(email => new EmailAddress(email)).ToList();

        var plainTextContent = System.Text.RegularExpressions.Regex.Replace(mensagemHtml, "<.*?>", string.Empty);

        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
            from,
            tos,
            titulo,
            plainTextContent,
            mensagemHtml
        );

        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

        if ((int)response.StatusCode >= 400)
        {
            var errorBody = await response.Body.ReadAsStringAsync().ConfigureAwait(false);
            throw new Exception($"Erro ao enviar email: {response.StatusCode} - {errorBody}");
        }
    }

}