using ShopOnline.Api.Entities;
using ShopOnline.Api.Views;

namespace ShopOnline.Api.Repositories.Contracts;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetItems();
    Task<IEnumerable<ProductCategory>> GetCategories();
    Task<Product> GetItem(int id);
    Task<ProductCategory> GetCategory(int id);
    Task<IEnumerable<Product>> GetItemsByCategory(int id);
    Task<List<Product>> GetAll();
    Task<Product> AddProduct(ProductView productView);
    Task<Product> UpdateProduct(string name, ProductView product);
    Task DeleteProduct(string name);


}