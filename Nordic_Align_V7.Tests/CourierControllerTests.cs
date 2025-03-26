using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Nordic_Align_V7.Controllers;
using Nordic_Align_V7.Models;

namespace Nordic_Align_V7.Tests;



public class CourierControllerTest
{
    private readonly NordicAlignDBContext _dbContext;
    private readonly CourierController _controller;

    public CourierControllerTest()
    {
        // Използвайте InMemoryDatabase за тестовете
        var options = new DbContextOptionsBuilder<NordicAlignDBContext>()
                      .UseInMemoryDatabase(databaseName: "TestDatabase")
                      .Options;

        _dbContext = new NordicAlignDBContext(options, new Mock<IConfiguration>().Object);
        _controller = new CourierController(_dbContext);
        _dbContext.Couriers.RemoveRange(_dbContext.Couriers);
        _dbContext.SaveChanges();
    }

    [Fact]
    public void Index_ReturnsViewWithListOfCouriers()
    {
        // Arrange
        var couriers = new List<CourierModel>
            {
                new CourierModel {Id = 1, FullName = "John Doe", Personnummer = 1234567890, LivingPlace = "Stockholm", Phone = "123456789"},
                new CourierModel {Id = 2, FullName = "Jane Smith", Personnummer = 9876543210, LivingPlace = "Stockholm", Phone = "123456789"}
            };

        _dbContext.Couriers.AddRange(couriers);
        _dbContext.SaveChanges();

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CourierModel>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void Create_Post_ReturnsRedirectToAction()
    {
        // Arrange
        var courier = new CourierModel
        {
            Id = 1,
            FullName = "John Doe",
            Personnummer = 1234567890,
            EmploymentDate = new DateTime(2021, 1, 1),
            StartWorkTime = new TimeSpan(8, 0, 0),
            EndWorkTime = new TimeSpan(17, 0, 0),
            LivingPlace = "Stockholm",
            Phone = "123456789"
        };

        // Act
        var result = _controller.Create(courier, "17:00:00", "08:00:00", "2021-01-01");

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Single(_dbContext.Couriers);
    }

    [Fact]
    public void Edit_Post_ReturnsRedirectToAction()
    {
        // Arrange
        var courier = new CourierModel
        {
            Id = 1,
            FullName = "John Doe",
            Personnummer = 1234567890,
            EmploymentDate = new DateTime(2021, 1, 1),
            StartWorkTime = new TimeSpan(8, 0, 0),
            EndWorkTime = new TimeSpan(17, 0, 0),
            LivingPlace = "Stockholm",
            Phone = "123456789"
        };

        _dbContext.Couriers.Add(courier);
        _dbContext.SaveChanges();

        // Act
        courier.FullName = "Updated Name";
        var result = _controller.Edit(courier, "17:00:00", "08:00:00", "2021-01-01");

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Updated Name", _dbContext.Couriers.First().FullName);
    }

    [Fact]
    public void Delete_ReturnsRedirectToAction()
    {
        // Arrange
        var courier = new CourierModel { Id = 1, FullName = "John Doe", Personnummer = 1234567890,LivingPlace = "Stockholm",Phone = "123456789"
        };

        _dbContext.Couriers.Add(courier);
        _dbContext.SaveChanges();

        // Act
        var result = _controller.Delete(courier.Id);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Empty(_dbContext.Couriers);
    }

    [Fact]
    public void Search_ReturnsViewWithMatchingCouriers()
    {
        // Arrange
        var searchString = "John";
        var couriers = new List<CourierModel>
            {
                new CourierModel { Id = 1, FullName = "John Doe", Personnummer = 1234567890, LivingPlace = "Stockholm",Phone = "123456789" },
                new CourierModel { Id = 2, FullName = "Jane Smith", Personnummer = 9876543210 , LivingPlace = "Stockholm",Phone = "123456789"}
            };

        _dbContext.Couriers.AddRange(couriers);
        _dbContext.SaveChanges();

        // Act
        var result = _controller.Search(searchString);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CourierModel>>(viewResult.Model);
        Assert.Single(model);
    }
}

