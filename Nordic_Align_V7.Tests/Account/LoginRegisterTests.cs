
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Controllers;
using Nordic_Align_V7.Models;
using Nordic_Align_V7.ViewModels;

using Microsoft.AspNetCore.Http;

namespace Nordic_Align_V7.Tests;
public class AccountControllerTests
{
    private readonly Mock<UserManager<UserModel>> _userManagerMock;
    private readonly Mock<SignInManager<UserModel>> _signInManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null!, null!, null!, null!, null!, null!, null!, null!);

        _signInManagerMock = new Mock<SignInManager<UserModel>>(
            _userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<UserModel>>(), null!, null!, null!, null!);

        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null!, null!, null!, null!);

        _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _roleManagerMock.Object);

        var httpContext = new DefaultHttpContext();
        var sessionMock = new Mock<ISession>();
        httpContext.Session = sessionMock.Object;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };


    }

    [Fact]
    public async Task Register_SuccessfulRegistration_RedirectsToHome()
    {
        var model = new RegisterViewModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _userManagerMock.Setup(x => x.Users).Returns(new List<UserModel>().AsQueryable());
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<UserModel>(), false, null))
            .Returns(Task.CompletedTask);

        var result = await _controller.Register(model);

        var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToAction.ActionName);
        Assert.Equal("Home", redirectToAction.ControllerName);
    }

    [Fact]
    public async Task Register_InvalidModel_ReturnsViewWithModel()
    {
        var model = new RegisterViewModel();
        _controller.ModelState.AddModelError("Email", "Email is required");

        var result = await _controller.Register(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
    }

    [Fact]
    public async Task Login_SuccessfulLogin_RedirectsToHome()
    {
        var model = new LoginViewModel { Email = "johndoe@example.com", Password = "Password123!" };

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(model.Email, model.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email))
            .ReturnsAsync(new UserModel { Email = model.Email });
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<UserModel>()))
            .ReturnsAsync(new List<string> { "User" });

        var result = await _controller.Login(model, model.Email, model.Password);

        var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToAction.ActionName);
        Assert.Equal("Home", redirectToAction.ControllerName);
    }

    [Fact]
    public async Task Login_InvalidLogin_ReturnsViewWithError()
    {
        var model = new LoginViewModel { Email = "johndoe@example.com", Password = "WrongPassword" };

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(model.Email, model.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        var result = await _controller.Login(model, model.Email, model.Password);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True(_controller.ModelState.ContainsKey(""));
    }

}
