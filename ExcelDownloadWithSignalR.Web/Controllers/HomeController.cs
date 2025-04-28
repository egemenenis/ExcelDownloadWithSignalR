using System.Diagnostics;
using ExcelDownloadWithSignalR.Web.Models;
using ExcelDownloadWithSignalR.Web.Models.ViewModels;
using ExcelDownloadWithSignalR.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExcelDownloadWithSignalR.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        AppDbContext context, FileService fileService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userToCreate = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(userToCreate, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hasUser = await userManager.FindByEmailAsync(model.Email);

            if (hasUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            }

            var result = await signInManager.PasswordSignInAsync(hasUser, model.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProductList()
        {
            var user = await userManager.FindByEmailAsync("test@gmail.com");

            if (context.Products.Any(x => x.UserId == user!.Id))
            {
                var products = context.Products.Where(x => x.UserId == user!.Id).ToList();

                return View(products);
            }

            var productList = new List<Product>()
            {
                new Product() { Name = "Pen 1", Description = "Desc 1", Price = 100, UserId = user!.Id },
                new Product() { Name = "Pen 2", Description = "Desc 2", Price = 200, UserId = user!.Id },
                new Product() { Name = "Pen 3", Description = "Desc 3", Price = 300, UserId = user!.Id },
                new Product() { Name = "Pen 4", Description = "Desc 4", Price = 400, UserId = user!.Id },
                new Product() { Name = "Pen 5", Description = "Desc 5", Price = 500, UserId = user!.Id }
            };

            context.Products.AddRange(productList);

            await context.SaveChangesAsync();

            return View(productList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateExcel()
        {
            var response = new
            {
                Status = await fileService.AddMessageToQueue()
            };

            return Json(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
