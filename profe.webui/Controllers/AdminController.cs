 using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using profe.webui.Data.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using profe.webui.Entities;
using profe.webui.Models;
using profe.webui.Data.Services;
using Microsoft.EntityFrameworkCore;
using profe.webui.Extensions;
using System.IO;
using Azure.Storage.Blobs;
using Microsoft.Win32.SafeHandles;
using profe.webui.Exceptions.ProductExceptions;

namespace profe.webui.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartService _cartService;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminController(IProductRepository productRepository, ICategoryRepository categoryRepository, ICartService cartService,
                              UserManager<User>  userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
		{
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _roleManager = roleManager;
		}

        //PRODUCT PAGES

        public IActionResult ProductList()
        {

            try
            {
                var adminProducts = new ProductListViewModel()
                {
                    Products = _productRepository.GetAll()
                };

                return View(adminProducts);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ürünler listelenirken beklenmeyen bir hata oluştu.");
                ModelState.AddModelError("", "Ürünler listelenirken beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");

            }
            return View();

        }

        [HttpGet]
        public IActionResult CreateProduct()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model, IFormFile file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var entity = new Product()
                    {
                        productName = model.Name,
                        Price = model.Price,
                        Description = model.Description,
                        Stock = model.Stock,
                        //ImgUrl = model.ImgUrl,
                        IsApproved = model.IsApproved,
                    };




                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    entity.ImgUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    _productRepository.AddAsync(entity);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Create Product",
                        Message = $"{entity.productName} isimli ürün eklendi.",
                        AlertType = "success"

                    });

                    return RedirectToAction("ProductList");
                }
            }
            catch(ProductTitleMustNotBeSameException ex)
            {
                _logger.LogError("Aynı isimde başka bir ürün eklenmeye çalışıldı",ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün oluşturma sırasında beklenmeyen bir hata oluştu.");
                ModelState.AddModelError("", "Ürün oluşturma işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productRepository.GetProductDetails((int)id);

            if (id == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = product.Id,
                Name = product.productName,
                Price = product.Price,
                Description = product.Description,
                //ImgUrl = product.ImgUrl,

                SelectedCategories = product.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryRepository.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    var product = _productRepository.GetProductDetails(model.ProductId);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    product.productName = model.Name;
                    product.Price = model.Price;
                    product.Description = model.Description;
                    product.Stock = model.Stock;
                    product.IsApproved = model.IsApproved;

                    if (file != null)
                    {
                        var extention = Path.GetExtension(file.FileName);
                        var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                        product.ImgUrl = randomName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName);
                        //eski projede wwwroot\\img şeklindeydi, öyle ur kısmında hata verdi

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    _productRepository.Update(product, categoryIds);


                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Update Product",
                        Message = $"{model.Name} isimli ürün güncellendi.",
                        AlertType = "success"

                    });

                    return RedirectToAction("ProductList");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün düzenleme sırasında beklenmeyen bir hata oluştu.");
                //kullanıcı tarayıcı üzerinden hangi alanları eksik girdiğini daha estekik görsün diyehata fırlatmadım
                //ModelState.AddModelError("", "Ürün düzenleme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
                return View(model);

            }

            ViewBag.Categories = _categoryRepository.GetAll();
            //burada bir linq sorgusuyla boş selectedcategories çek, döngüyü null referenceden kurtar
            return View(model);

        }

        //[HttpPost]
        //public async IActionResult deleteproduct(int productId)
        //{

        //    Product product = await _productRepository.GetById(productId);

        //    if (product != null)
        //    {
        //        _productRepository.Delete(product);
        //    }


        //    TempData.Put("message", new AlertMessage()
        //    {
        //        Title = "Delet Product",
        //        Message = $"{product.productName} adlı ürün silindi.",
        //        AlertType = "danger"

        //    });


        //    return RedirectToAction("ProductList");
        //}

        [HttpPost]
        public async Task<IActionResult> deleteproduct(int productId)
        {
            try
            {
                Product product = await _productRepository.GetById(productId);

                if (product != null)
                {
                    _productRepository.Delete(product);
                }


                TempData.Put("message", new AlertMessage()
                {
                    Title = "Delet Product",
                    Message = $"{product.productName} adlı ürün silindi.",
                    AlertType = "danger"

                });


                return RedirectToAction("ProductList");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ürün silme sırasında beklenmeyen bir hata oluştu.");
                ModelState.AddModelError("", "Ürün silme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");

            }
            return RedirectToAction("ProductList");


        }

        //USER PAGES
        public IActionResult UserCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };



            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Admin olarak  kullanıcı eklediğinde daha sonrasında el ile role eklei checkbox yüzünden hata veriyor
               // _cartService.InitializeCart(user.Id);

                return RedirectToAction("UserList");
            }

            ModelState.AddModelError("", "Bir hata oluştu, lütfen tekrar deneyin");
            _cartService.InitializeCart(user.Id);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UserDelete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return RedirectToAction("RoleList");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        return RedirectToAction("RoleList");
                    }
                }
                return RedirectToAction("RoleList");
            }

            return View(model);
        }


        public IActionResult UserList()
        {

            return View(_userManager.Users);
        }


        //ROLE PAGES
        [HttpPost]
        public async Task<IActionResult> RoleDelete(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("RoleList");

        }





        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(); // Rol bulunamazsa hata dön
            }

            var allUsers = await _userManager.Users.ToListAsync(); // Tüm kullanıcıları önceden çek
            var members = new List<User>();
            var nonMembers = new List<User>();

            foreach (var user in allUsers)
            {
                // Kullanıcının rolüne göre doğru listeye ekleyin
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    members.Add(user);
                }
                else
                {
                    nonMembers.Add(user);
                }
            }

            var model = new RoleDetails
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                //ADD TO ROLE
                //foreach içindeki son satır , eğer dizi nullsa boş bir dizi tanımlaması yapmak için
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                //REMOVE FROM ROLE
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }


            }
            return Redirect("/admin/RoleEdit/" + model.RoleId);
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        //tempData alternative 
                    }
                }

            }
            return View(model);
        }




    }
}

