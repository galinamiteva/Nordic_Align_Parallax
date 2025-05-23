﻿using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Models;
using Nordic_Align_V7.Services;

namespace Nordic_Align_V7.Controllers;

public class ContactController : Controller
{
    private readonly IMailService _mailService;

    public ContactController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ContactFormModel formData)
    {
        if (string.IsNullOrWhiteSpace(formData.Name) ||
        string.IsNullOrWhiteSpace(formData.Email) ||
        string.IsNullOrWhiteSpace(formData.Message))
        {
            return BadRequest(@Nordic_Align_V7.Resources.Resource.Allafältärobligatoriska);
        }

        string subject = $"Nytt meddelande från {formData.Name}";
        string body = $"<p><strong>Name:</strong> {formData.Name}</p><p><strong>Email:</strong>" +
          $" {formData.Email}</p><p><strong>Meddelande:</strong> {formData.Message}</p>";

        try
        {
            await _mailService.SendEmailAsync("galinamiteva69@gmail.com", subject, body);
            return Ok(@Nordic_Align_V7.Resources.Resource.Tackfördittmeddelande);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Fel: {ex.Message}");
        }
    }
}