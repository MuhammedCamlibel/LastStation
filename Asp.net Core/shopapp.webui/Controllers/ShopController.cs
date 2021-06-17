using System.Linq;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;


namespace shopapp.webui.Controllers
{
    public class ShopController:Controller
    {
        
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult List(string category, int page=1){ // page herhangi bir deger gelmezse 1 verilir 
            
            const int pageSize = 2;
            var productViewModel = new ProductListViewModel(){
                PageInfo =  new PageInfo() // sayfalama için 
                {
                   TotalItems = _productService.GetCountByCategory(category),
                   CurrentPage = page,
                   ItemsPerPage = pageSize,
                   CurrentCategory = category

                },
                Products = _productService.GetProductByCategory(category,page,pageSize)
            };

            return View(productViewModel);
        }

        public IActionResult Details(string url){

            if(url==null){
                return NotFound();
            }

            Product product = _productService.GetProductDetails(url);

            if(product == null){
                
                return NotFound();
            }

            return View(new ProductDetailModel{
                Product = product,
                Categories = product.ProductCategories.Select(i=>i.Category).ToList()
            });
        }


        public IActionResult Search(string  q,string fiyat){

            var productViewModel = new ProductListViewModel(){

                Products = _productService.GetSearchResult(q,fiyat)
            };

            return View(productViewModel);
        }

    }
}