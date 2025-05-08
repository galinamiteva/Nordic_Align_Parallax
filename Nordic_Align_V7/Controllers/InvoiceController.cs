using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Models;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Nordic_Align_V7;


public class InvoiceController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly InvoiceDBContext _dbContext;

    public InvoiceController(IConfiguration configuration, InvoiceDBContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }




    [HttpGet]
    public IActionResult CreateInvoice()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateInvoice(string email, string invoiceNumber, DateTime issueDate,
        string companyName, string street, string city, string country, string postalCode, string? state,
        string? phone, string? vatNumber, string? comment, List<string> itemNames, List<decimal> prices, List<int> quantities)
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
                Comment = comment,
                VatNumber = vatNumber


            };


            using var transaction = _dbContext.Database.BeginTransaction();



            _dbContext.Invoices.Add(invoice);
            _dbContext.SaveChanges();
            Console.WriteLine($"Invoice saved with ID: {invoice.Id}");

            if (itemNames == null || prices == null || quantities == null || itemNames.Count != prices.Count || itemNames.Count != quantities.Count)
            {
                ViewBag.RegisterFail = "Error: Invalid item data. Please check the input.";
                return View();
            }

            if (prices.Any(p => p <= 0))
            {
                ViewBag.RegisterFail = "Error: Invalid item data";
                return View();
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


            Console.WriteLine($"Received {itemNames.Count} items, {prices.Count} prices, {quantities.Count} quantities.");
            for (int i = 0; i < itemNames.Count; i++)
            {
                Console.WriteLine($"Item {i}: {itemNames[i]}, Price: {prices[i]}, Quantity: {quantities[i]}");
            }

            _dbContext.SaveChanges();

            transaction.Commit();

            var pdfBytes = GenerateInvoicePdf(invoiceNumber, issueDate, dueDate, companyName, vatNumber!, street, city, country, postalCode, state!, email, phone!, comment!, itemNames, prices, quantities);
            bool result = SendEmailWithAttachment(email, "Invoice PDF", "Please find your invoice attached.", pdfBytes);

            if (result)
            {
                ViewBag.RegisterFail = "E-postfaktura har skickats.";
            }
            else
            {
                ViewBag.RegisterFail = "Fel: Det gick inte att skicka e-post.";
            }

            ModelState.Clear();

            return View();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            ViewBag.RegisterFail = "A processing error occurred";
            return View();
        }
    }

    private byte[] GenerateInvoicePdf(string invoiceNumber, DateTime issueDate, DateTime dueDate,
        string companyName, string? vatNumber, string street, string city, string country, string postalCode, string state,
        string email, string phone, string? comment, List<string> itemNames, List<decimal> prices, List<int> quantities)
    {
        decimal subtotal = prices.Zip(quantities, (p, q) => p * q).Sum();
        decimal tax = subtotal * 0.25m;
        decimal total = subtotal + tax;

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
                        col.Item().Text($"VAT: {vatNumber} ");
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
                                leftCol.Item().PaddingBottom(7).Text("Date:").FontColor(Colors.Grey.Lighten1).AlignEnd(); ;
                                leftCol.Item().PaddingBottom(7).Text("PaymentTerms:").FontColor(Colors.Grey.Lighten1).AlignEnd(); ;
                                leftCol.Item().PaddingBottom(12).Text("Due Date:").FontColor(Colors.Grey.Lighten1).AlignEnd(); ;
                                leftCol.Item().Background(Colors.Grey.Lighten4).PaddingVertical(2).PaddingBottom(7).Text("Balance Due:").AlignEnd().Bold();

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


                    column.Item().PaddingTop(30).Element(container => ComposeTable(container, itemNames, prices, quantities));
                    column.Spacing(5);  //  vertical spacing
                    column.Item().PaddingTop(7).AlignRight().Text($"Subtotal: {subtotal:C}").SemiBold();
                    column.Item().PaddingTop(7).AlignRight().Text($"Tax (25%): {tax:C}");
                    column.Item().PaddingTop(7).AlignRight().Text($"Total: {total:C}").SemiBold();



                });

                // Add footer at the end of the page
                page.Footer().PaddingBottom(90).Column(column =>
                {
                    column.Item().PaddingBottom(7).Text("Notes:").FontColor(Colors.Grey.Lighten1);
                    column.Item().Text("Nordic Align Supply Chain Consulting");
                    column.Item().PaddingBottom(7).Text("VAT: SE559480618301");
                    column.Item().Text("Kontonr 814 655 698-3");
                    column.Item().PaddingBottom(7).Text("Clearingnr 8169-5");
                    column.Item().Text("International payments");
                    column.Item().Text("IBAN SE22 8000 0816 9581 4655 6983");
                    column.Item().PaddingBottom(15).Text("BIC SWEDSES");

                    if (!string.IsNullOrWhiteSpace(comment))
                    {
                        column.Item().PaddingBottom(7).Text("Terms:").FontColor(Colors.Grey.Lighten1);
                        column.Item().AlignLeft().Text($" {comment}");
                    }
                });
            });
        }).GeneratePdf();
    }

    private void ComposeTable(IContainer container, List<string> itemNames, List<decimal> prices, List<int> quantities)
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
                table.Cell().PaddingBottom(7).AlignRight().Text($"{prices[i]:C}");
                table.Cell().PaddingBottom(7).AlignRight().Text(quantities[i].ToString());
                table.Cell().PaddingBottom(7).AlignRight().Text($"{total:C}").ParagraphSpacing(5);

            }
        });
    }


    public bool SendEmailWithAttachment(string recipientEmail, string subject, string body, byte[] attachmentBytes)
    {
        try
        {
            Console.WriteLine($"Attempting to send email to: {recipientEmail}");

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
            Console.WriteLine("Email sent successfully.");

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