using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IProductService : IValidator<Product>
    {
         Product GetById(int id);

         Product GetByIdWithCategories(int id);

         List<Product> GetAll();

         List<Product> GetHomePageProducts();
         List<Product> GetSearchResult(string searchString,string fiyat);

         Product GetProductDetails(string url);
         List<Product> GetProductByCategory(string name , int page, int pageSize);

         bool Create(Product entity);

         void Update(Product entity);

         void Delete(Product entity);

        int GetCountByCategory(string category);
        bool Update(Product entity, int[] categoryIds);
    }
}