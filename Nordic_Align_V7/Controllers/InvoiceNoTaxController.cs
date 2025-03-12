using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;
using System.Net.Mail;
using Nordic_Align_V7.Models;
using Nordic_Align_V7;

namespace Nordic_Align_V6.Controllers;

public class InvoiceNoTaxController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly InvoiceDBContext _dbContext;

    public InvoiceNoTaxController(IConfiguration configuration, InvoiceDBContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult CreateInvoiceInternational()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateInvoiceInternational(string email, string invoiceNumber, DateTime issueDate,
        string companyName, string street, string city, string country, string postalCode, string? state,
        string? phone, string? comment, List<string> itemNames, List<decimal> prices, List<int> quantities)
    {
        try
        {
            DateTime dueDate = issueDate.AddDays(30);

            var customerAddress = new AddressModel
            {
                CompanyName = companyName,
                Street = street,
                City = city,
                Country = country,
                PostalCode = postalCode,
                State = state,
                Email = email,
                Phone = phone


            };

            var invoice = new InvoiceModel
            {
                InvoiceNumber = invoiceNumber,
                IssueDate = issueDate,
                DueDate = dueDate,
                CustomerAddress = customerAddress,
                Comment = comment

            };
            using var transaction = _dbContext.Database.BeginTransaction();

            _dbContext.Invoices.Add(invoice);
            _dbContext.SaveChanges();

            if (itemNames == null || prices == null || quantities == null || itemNames.Count != prices.Count || itemNames.Count != quantities.Count)
            {
                return Content("Error: Invalid item data. Please check the input.");
            }

            var invoiceItems = new List<InvoiceItemModel>();

            for (int i = 0; i < itemNames.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(itemNames[i]) && prices[i] > 0 && quantities[i] > 0)
                {
                    invoiceItems.Add(new InvoiceItemModel
                    {
                        Item = itemNames[i],
                        Price = prices[i],
                        Quantity = quantities[i],
                        InvoiceId = invoice.Id,
                        Invoice = invoice
                    });
                }
            }
            invoice.Items = invoiceItems;
            _dbContext.InvoiceItems.AddRange(invoiceItems);
            _dbContext.SaveChanges();
            transaction.Commit();

            var pdfBytes = GenerateInvoicePdf(invoiceNumber, issueDate, dueDate, companyName, street, city, country, postalCode, state!, email, phone!, comment!, itemNames, prices, quantities);
            bool result = SendEmailWithAttachment(email, "Invoice PDF", "Please find your invoice attached.", pdfBytes);

            return result ? Content("Invoice email has been sent successfully.") : Content("An error occurred while sending the email.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return Content($"A processing error occurred: {ex.Message}");
        }
    }

    private byte[] GenerateInvoicePdf(string invoiceNumber, DateTime issueDate, DateTime dueDate,
    string companyName, string street, string city, string country, string postalCode, string state,
    string email, string phone, string? comment, List<string> itemNames, List<decimal> prices, List<int> quantities)
    {
        decimal total = prices.Zip(quantities, (p, q) => p * q).Sum();

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Image("wwwroot/images/Logo_Nordic.png").FitWidth();
                        col.Item().PaddingBottom(40);
                        col.Item().Text("Nordic Align Supply Chain Consulting AB").Bold();
                        col.Item().Text("Hagaesplanaden 73");
                        col.Item().Text("113 68 Stockholm");
                        col.Item().Text("Sweden");
                        col.Item().Text("nordicalign@gmail.com");
                        col.Item().PaddingBottom(13);
                        col.Item().PaddingBottom(3).Text("Bill To:").FontColor(Colors.Grey.Lighten1);
                        col.Item().Text(companyName).Bold();
                        col.Item().Text(street);
                        col.Item().Text($"{postalCode} {city}");
                        col.Item().Text(country);
                        col.Item().Text(email);

                    });
                    row.Spacing(70);
                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text("INVOICE").FontSize(26).Bold().AlignEnd();
                        col.Item().Text($"# {invoiceNumber}").FontSize(14).FontColor(Colors.Grey.Lighten1).AlignEnd();
                        col.Item().PaddingBottom(70).PaddingLeft(20);

                        col.Item().Row(innerRow =>
                        {

                            innerRow.ConstantItem(80).Column(leftCol =>
                            {
                                leftCol.Item().PaddingBottom(7).Text("Date:").FontColor(Colors.Grey.Lighten1);
                                leftCol.Item().PaddingBottom(7).Text("PaymentTerms:").FontColor(Colors.Grey.Lighten1);
                                leftCol.Item().PaddingBottom(12).Text("Due Date:").FontColor(Colors.Grey.Lighten1);
                                leftCol.Item().Background(Colors.Grey.Lighten4).PaddingVertical(2).PaddingBottom(7).Text("Balance Due:").Bold();

                            });


                            innerRow.RelativeItem().Column(rightCol =>
                            {
                                rightCol.Item().PaddingBottom(7).Text(issueDate.ToString("MMM dd, yyyy")).AlignEnd();
                                rightCol.Item().PaddingBottom(7).Text("30 days").AlignEnd();
                                rightCol.Item().PaddingBottom(12).Text(dueDate.ToString("MMM dd, yyyy")).AlignEnd();
                                rightCol.Item().Background(Colors.Grey.Lighten4).PaddingVertical(2).PaddingBottom(7).Text($"{total:C}").Bold().AlignEnd();

                            });
                        });
                    });

                });






                page.Content().Column(column =>
                {
                    column.Item().PaddingTop(30).Element(container => ComposeStyledTable(container, itemNames, prices, quantities));
                    column.Item().PaddingTop(10).AlignRight().Text($"Total: {total:C}").FontSize(14);

                    if (!string.IsNullOrWhiteSpace(comment))
                    {
                        column.Item().PaddingTop(30).AlignLeft().Text($" {comment}");
                    }

                });

                page.Footer().PaddingBottom(70).Column(column =>
                {
                    column.Item().PaddingBottom(7).Text("Notes:").FontColor(Colors.Grey.Lighten1);
                    column.Item().Text("Nordic Align Supply Chain Consulting");
                    column.Item().Text("Kontonr 814 655 698-3");
                    column.Item().Text("Clearingnr 8169-5");
                    column.Item().PaddingTop(10).Text("International payments");
                    column.Item().Text("IBAN SE22 8000 0816 9581 4655 6983");
                    column.Item().Text("BIC SWEDSES");
                });
            });
        }).GeneratePdf();
    }



    private void ComposeStyledTable(IContainer container, List<string> itemNames, List<decimal> prices, List<int> quantities)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Background(Colors.Black).PaddingVertical(5).Text("#").FontColor(Colors.White).Bold();
                header.Cell().Background(Colors.Black).PaddingVertical(5).Text("Item").FontColor(Colors.White).Bold();
                header.Cell().Background(Colors.Black).PaddingVertical(5).AlignRight().Text("Quantity").FontColor(Colors.White).Bold();
                header.Cell().Background(Colors.Black).PaddingVertical(5).AlignRight().Text("Rate").FontColor(Colors.White).Bold();
                header.Cell().Background(Colors.Black).PaddingVertical(5).AlignRight().Text("Amount").FontColor(Colors.White).Bold();
                header.Cell().PaddingBottom(10);
            });

            for (int i = 0; i < itemNames.Count; i++)
            {
                decimal total = prices[i] * quantities[i];

                table.Cell().PaddingBottom(7).Text((i + 1).ToString());
                table.Cell().PaddingBottom(7).Text(itemNames[i]);
                table.Cell().PaddingBottom(7).AlignRight().Text(quantities[i].ToString());
                table.Cell().PaddingBottom(7).AlignRight().Text($"{prices[i]:C}");
                table.Cell().PaddingBottom(7).AlignRight().Text($"{total:C}").ParagraphSpacing(5);



            }
        });
    }


    private bool SendEmailWithAttachment(string recipientEmail, string subject, string body, byte[] attachmentBytes)
    {
        try
        {
            var smtpSettings = _configuration.GetSection("Smtp");
            var fromEmail = smtpSettings["Username"];
            var fromName = smtpSettings["FromName"];
            var fromPassword = smtpSettings["Password"];
            var smtpHost = smtpSettings["Host"];
            var smtpPort = int.Parse(smtpSettings["Port"]!);
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"]!);

            var fromAddress = new MailAddress(fromEmail!, fromName);
            var toAddress = new MailAddress(recipientEmail);
            var replyToAddress = new MailAddress("galinamiteva69@gmail.com");

            using var smtp = new SmtpClient
            {
                Host = smtpHost!,
                Port = smtpPort,
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            message.ReplyToList.Add(replyToAddress);
            message.Attachments.Add(new Attachment(new MemoryStream(attachmentBytes), "Invoice.pdf"));

            smtp.Send(message);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
//AddressComponent class remains the same
public class AddressComponent : IComponent
{
    private readonly string _title;
    private readonly string _name;
    private readonly string _street;
    private readonly string _city;
    private readonly string _country;
    private readonly string _postalCode;
    private readonly string _email;
    private readonly string? _phone;


    public AddressComponent(string title, string name, string street, string city, string country,
                             string postalCode, string email, string? phone)
    {
        _title = title;
        _name = name;
        _street = street;
        _city = city;
        _country = country;
        _postalCode = postalCode;
        _email = email;
        _phone = phone; // This is now nullable
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().Text(_title).SemiBold();
            column.Item().PaddingBottom(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            column.Item().Text(_name);
            column.Item().Text(_street);
            column.Item().Text($"{_postalCode}, {_city}");
            column.Item().Text(_country);
            column.Item().Text($"Email: {_email}");
            if (_phone != null)
            {
                column.Item().Text($"Phone: {_phone}");
            }
            column.Item().PaddingBottom(20);

        });
    }
}
