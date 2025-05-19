using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelWebsite.Models;
using TravelWebsite.ViewModels;

namespace TravelWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Xóa các thông báo lỗi cũ
            ModelState.Clear();

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập email.");
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu.");
                return View(model);
            }

            // Kiểm tra xem người dùng có tồn tại không
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                return View(model);
            }

            // Thử đăng nhập bằng username thay vì email
            if (user.UserName != model.Email)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }

            // Nếu không thành công, thử đăng nhập bằng email
            var emailResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (emailResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (emailResult.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }
            else
            {
                // Kiểm tra mật khẩu
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isPasswordCorrect)
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không chính xác.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin.");
                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }
    }
} 