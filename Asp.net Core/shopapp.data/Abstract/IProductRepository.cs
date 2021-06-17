using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductDetails(string url); 

        List<Product> GetProductsByCategory(string name , int page, int pageSize);

        List<Product> GetSearchResult(string searchString,string fiyat);

        List<Product> GetTop5Products();

        List<Product> GetHomePageProducts();

        int GetCountByCategory(string category);

        Product GetByIdWithCategories(int id);

        void Update(Product entity, int[] categoryIds);
    }
}