using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;

namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
         
        private ICategoryService _categoryService; 

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
 

        public IViewComponentResult Invoke(){
          if(RouteData.Values["category"]!=null){ // id bilgisi list den geliyorsa 
               ViewBag.SelectedCategory = RouteData?.Values["category"]; // id bilgisi geledebilir gelmeyede bilir 
          }
          
          return View(_categoryService.GetAll());
         
        }
    }
}