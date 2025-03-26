
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7.Models;
using Microsoft.AspNetCore.Identity;


namespace Nordic_Align_V7.Tests;
public class InvoiceControllerTests
{
    private readonly InvoiceController _controller;
    private readonly InvoiceDBContext _context;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<UserManager<UserModel>> _mockUserManager;

    public InvoiceControllerTests()
    {
        // Създаване на моки за IConfiguration и UserManager
        _mockConfiguration = new Mock<IConfiguration>();
        _mockUserManager = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        // Създаване на опции за DbContext с InMemoryDatabase
        var options = new DbContextOptionsBuilder<InvoiceDBContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

        // Инициализиране на InvoiceDBContext с два параметъра
        _context = new InvoiceDBContext(options, _mockConfiguration.Object);

        // Инициализиране на контролера
        _controller = new InvoiceController(_mockConfiguration.Object, _context);
    }

    

    


    [Fact]
    public void CreateInvoice_InvalidItemData_ReturnsErrorMessage()
    {
        // Arrange
        string email = "test@example.com";
        string invoiceNumber = "INV12345";
        DateTime issueDate = DateTime.UtcNow;
        string companyName = "Test Company";
        string street = "Test Street 123";
        string city = "Test City";
        string country = "Test Country";
        string postalCode = "12345";
        string state = "Test State";
        string phone = "123456789";
        string comment = "Test Comment";
        List<string> itemNames = new List<string> { "Item1" };
        List<decimal> prices = new List<decimal> { -10.0m };
        List<int> quantities = new List<int> { 1 };

        // Act
        var result = _controller.CreateInvoice(email, invoiceNumber, issueDate, companyName, street, city, country, postalCode, state, phone, comment, itemNames, prices, quantities);
        var contentResult = result as ContentResult;

        Console.WriteLine(contentResult?.Content); // Печатаме съдържанието на резултата


        // Assert
        Assert.NotNull(contentResult);
        Assert.Contains("A processing error occurred", contentResult.Content);
    }

    [Fact]
    public void CreateInvoice_ExceptionOccurs_ReturnsErrorMessage()
    {
        // Arrange
        string email = "test@example.com";
        string invoiceNumber = "INV12345";
        DateTime issueDate = DateTime.UtcNow;
        List<string> itemNames = null!; // Подаване на невалидни данни (null)
        List<decimal> prices = new List<decimal> { 10.0m };
        List<int> quantities = new List<int> { 1 };

        // Act
        var result = _controller.CreateInvoice(email, invoiceNumber, issueDate, "Company", "Street", "City", "Country", "12345", "State", "123456789", "Comment", itemNames, prices, quantities);
        var contentResult = result as ContentResult;

        Console.WriteLine(contentResult?.Content); // Печатаме съдържанието на резултата

        // Assert
        Assert.NotNull(contentResult);
        Assert.Contains("A processing error occurred", contentResult.Content);
    }
}

