using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using profe.webui.Data.Repositories;
using profe.webui.Entities;
using profe.webui.Models;

namespace profe.webui.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository  _repository;
    private readonly IGenericRepository<Product> _genericRepository;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IProductRepository repository, IGenericRepository<Product> genericRepository)
    {
        _logger = logger;
        _repository = repository;
        _genericRepository = genericRepository;
    }

    public IActionResult Index()
    {
        //Listeleme için kullan
        return View();
    }


    public IActionResult list(int? id, string q)
    {
        //include ile category ve subcategory bilgilerini çek


        if (id != null) 
        {
            var model = new ProductListViewModel()
            {
                Products =_repository.GetProductByCategoryId((int)id)
            };

            return View(model);
        }
        else if (!string.IsNullOrEmpty(q))
        {
            var model = new ProductListViewModel()
            {
                Products =  _repository.GetProductsByQSearch(q)
            };
            return View(model);
        }
        else
        {
            var model = new ProductListViewModel()
            {
                Products = _genericRepository.GetAll()
            };
            return View(model);
        }
    }

    public  IActionResult detail(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Product product =  _repository.GetProductDetails((int)id);

        if (product == null)
        {
            return NotFound();
        }

        var model = new ProductDetailModel
        {
            Product = product,
            Categories = product.ProductCategories.Select(i => i.Category).ToList()
        };

        return View(model);
    }





    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

