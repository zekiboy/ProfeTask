 using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using profe.webui.Data.EmailService;
using profe.webui.Data.Repositories;
using profe.webui.Data.Services;
using profe.webui.Entities;
using profe.webui.Exceptions.AuthExceptions;
using profe.webui.Extensions;
using profe.webui.Models;

namespace profe.webui.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly UserManager<User> _userManager;
        private readonly ICartService _cartService;

        //cookie işlemlerini yönetecek
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController (UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender,
                                  ICartService cartService, ILogger<AccountController> logger)
        {
            _logger = logger;
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        public IActionResult Login(string ReturnUrl = null)
        {
            //kullanıcı başka bir sayfaya tıklayıp logine yönlendirildiğinde,giriş yaptıktan sonra, istediği sayfaya ReturnUrl ile ile geri göndereceğiz bu bilgiyi hidden input olarak tutacağız

            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce bir hesap oluşturulmamış");
                        return View(model);
                    }

                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Lütfen email hesabınıza gelen link ile üyeliğinizi onaylayın");
                        return View(model);
                    }


                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        return Redirect(model.ReturnUrl ?? "~/");
                    }

                    ModelState.AddModelError("", "Girilen kullanıcı adı veya parola yanlış");
                    return View(model);
                }
            }
            catch(EmailOrPasswordShouldNotBeInvalidException ex)
            {
                _logger.LogError(ex, "Login işlemi sırasında bir hata oluştu: {Message}", ex.Message);
                ModelState.AddModelError("", ex.Message);
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında beklenmeyen bir hata oluştu.");
                ModelState.AddModelError("", "Kayıt işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
                return View(model);

            }
            return View(model);


        }



        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user = new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email

                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        //generate token 
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var url = Url.Action("ConfirmEmail", "Account", new
                        {
                            userId = user.Id,
                            token = code,

                        });

                        // email
                        await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:7085{url}'>tıklayınız.</a>");
                        return RedirectToAction("Login", "Account");

                    }

                    ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
                    return View(model);

                }

            }
            catch (UserAlreadyExistException ex)
            {
                _logger.LogError(ex, "Kullanıcı kaydı sırasında bir hata oluştu: {Message}", ex.Message);
                ModelState.AddModelError("", ex.Message);
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında beklenmeyen bir hata oluştu.");
                ModelState.AddModelError("", "Kayıt işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
                return View(model);

            }


               return View(model);

        }




            public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            //her yapıya artık bu şekilde tempdata ekleyebiliriz, ekstra metodlara ihtiyacımız yok
            TempData.Put("message", new AlertMessage()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınız güvenli bir şekilde kapatıldı",
                AlertType = "warning"
            });

            return Redirect("~/");
        }




        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Geçersiz token.",
                    Message = "Geçersiz Token",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // cards objesini oluştur.
                    _cartService.InitializeCart(user.Id); 
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hesabınız onaylandı.",
                        Message = "Hesabınız onaylandı.",
                        AlertType = "success"
                    });
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Hesabınızı onaylanmadı.",
                Message = "Hesabınızı onaylanmadı.",
                AlertType = "warning"
            });
            return View();
        }


        public IActionResult ForgotPassword()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            //Generate Token
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code,

            });

            // Email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı yenilemek için linke <a href='https://localhost:7085{url}'>tıklayınız.</a>");


            return View();
        }




        public IActionResult ResetPassword(string userId, string token)
        {

            if (userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var model = new ResetPasswordModel { Token = token };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }
         
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}