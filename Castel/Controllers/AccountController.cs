using AutoMapper;
using Castel.Core.Unit;
using Castel.DTO;
using Castel.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Castel.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly SignInManager<StoreUser> signInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly RoleManager<StoreRole> roleManager;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper,
                                SignInManager<StoreUser> signInManager,
                                UserManager<StoreUser> userManager,
                                RoleManager<StoreRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await unitOfWork.AccountRepository.Get(u => u.Email == model.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "بيانات الدخول غير صحيحة");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied() => View("AccessDenied");
        [AllowAnonymous]

        public IActionResult Signup() => View();

        [AllowAnonymous]

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return View(model); // عرض الصفحة مع الأخطاء

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "كلمة المرور وتأكيدها غير متطابقين");
                return View(model);
            }
            var newUser = new StoreUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Status = AccountStatus.Active,
                AccountType = AccountType.User,
                IsApproved = true
            };
            await newUser.Validiate(unitOfWork);
            newUser.PasswordHash = userManager.PasswordHasher.HashPassword(newUser, model.Password);
            await unitOfWork.AccountRepository.Insert(newUser);
            await unitOfWork.Save();
            // تأكد من وجود دور "User"
            var userRoleExists = await roleManager.RoleExistsAsync("User");
            if (!userRoleExists)
            {
                await roleManager.CreateAsync(new StoreRole { Name = "User" });
            }
            if (string.IsNullOrEmpty(newUser.SecurityStamp))
            {
                // يضيف SecurityStamp إذا لم يتم توليده
                await userManager.UpdateSecurityStampAsync(newUser);
            }
            // إضافة الدور للمستخدم
            await userManager.AddToRoleAsync(newUser, "User");

            return RedirectToAction(nameof(Login));
        }

    }
}
