using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Models;
using Nordic_Align_V7.ViewModels;

namespace Nordic_Align_V7.Controllers;

public class AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, RoleManager<IdentityRole> roleManager) : Controller
{
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly SignInManager<UserModel> _signInManager = signInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Register(RegisterViewModel model)
    {

        Console.WriteLine("Register method called."); // Debug

        if (!ModelState.IsValid)
        {
            ViewBag.RegisterFail = "Registration failed. Please check your inputs.";
            Console.WriteLine("Model state is invalid."); // Debug
            return View(model);
        }

        bool isFirstUser = !_userManager.Users.Any();

        var user = new UserModel
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true

        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            Console.WriteLine($"User {user.Email} created successfully."); // Debug


            if (isFirstUser)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Error: {error.Description}"); // Debug
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user!);

            HttpContext.Session.SetString("UserRole", roles.FirstOrDefault() ?? "User");
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Remove("UserRole");
        return RedirectToAction("Login", "Account");
    }

}
