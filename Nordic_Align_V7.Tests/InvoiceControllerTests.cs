
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7.Models;
using Microsoft.AspNetCore.Identity;


namespace Nordic_Align_V7.Tests;
public class InvoiceControllerTests
{
    private readonly InvoiceDBContext _dbContext;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly InvoiceController _controller;

    public InvoiceControllerTests()
    {
        var options = new DbContextOptionsBuilder<InvoiceDBContext>()
            .UseInMemoryDatabase(databaseName: "TestInvoiceDB")
            .Options;

        _dbContext = new InvoiceDBContext(options, new ConfigurationBuilder().Build());
        _mockConfig = new Mock<IConfiguration>();
        _controller = new InvoiceController(_mockConfig.Object, _dbContext);
    }

    [Fact]
    public void CreateInvoice_Get_ReturnsViewResult()
    {
        var result = _controller.CreateInvoice();
        Assert.IsType<ViewResult>(result);
    }



    [Fact]
    public void CreateInvoice_Post_ReturnsErrorOnInvalidData()
    {
        // Arrange
        var result = _controller.CreateInvoice("test@example.com", "INV-002", DateTime.UtcNow, "Company", "Street", "City", "Country", "1000", "State", "12345", "123123", "Comment", new List<string> { "Item1" }, new List<decimal> { 0m }, new List<int> { 1 });

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Contains("A processing error occurred", viewResult.ViewData["RegisterFail"]!.ToString());
    }
}


