using System;
using Microsoft.AspNetCore.Mvc;
using profe.webui.Data.Repositories;

namespace profe.webui.ViewComponents
{
	public class CategoriesViewComponent : ViewComponent
    {

        private ICategoryRepository _categoryRepository ;

        public CategoriesViewComponent(ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }


        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["id"];
            return View(_categoryRepository.GetAll());
        }

    }
}

