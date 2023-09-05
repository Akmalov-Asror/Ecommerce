using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Views;

namespace ShopOnline.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopOnlineDbContext shopOnlineDbContext;

    public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
    {
        this.shopOnlineDbContext = shopOnlineDbContext;
    }
    public async Task<IEnumerable<ProductCategory>> GetCategories() => await shopOnlineDbContext.ProductCategories.ToListAsync();

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<Product> GetItem(int id)
    {
        var product = await shopOnlineDbContext.Products
            .Include(p => p.ProductCategory)
            .SingleOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<IEnumerable<Product>> GetItems()
    {
        var products = await this.shopOnlineDbContext.Products
            .Include(p => p.ProductCategory).ToListAsync();

        return products;
        
    }

    public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
    {
        var products = await this.shopOnlineDbContext.Products
            .Include(p => p.ProductCategory)
            .Where(p => p.CategoryId == id).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetAll()
    {
        return await shopOnlineDbContext.Products.Include(c => c.ProductCategory).ToListAsync();
    }

    public async Task<Product> AddProduct(ProductView productView)
    {
        var newProduct = new Product();
        
        newProduct.Name = productView.Name;
        newProduct.Description = productView.Description;
        newProduct.ImageURL = productView.ImageURL;
        newProduct.Price = productView.Price;
        newProduct.Qty = productView.Qty;
        var findCategory = await shopOnlineDbContext.ProductCategories.FindAsync(productView.CategoryId);
        if (findCategory != null) newProduct.CategoryId = findCategory.Id;

        

        shopOnlineDbContext.Products.Add(newProduct);
        await shopOnlineDbContext.SaveChangesAsync();
        return newProduct;
    }

    public async Task<Product> UpdateProduct(string name, ProductView product)
    {
        var findProduct = await shopOnlineDbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
        findProduct.Qty = product.Qty;
        findProduct.Price = product.Price;
        findProduct.ImageURL = product.ImageURL;
        findProduct.Name = product.Name;
        findProduct.Description = product.Description;
        var findProductCategory = await shopOnlineDbContext.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == product.CategoryId);
        if (findProductCategory is not null ) findProduct.CategoryId = product.CategoryId;

        shopOnlineDbContext.Entry(findProduct).State = EntityState.Modified;
        await shopOnlineDbContext.SaveChangesAsync();
        return findProduct;
    }

    public async Task DeleteProduct(string name)
    {
        var selectProduct = await shopOnlineDbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
        shopOnlineDbContext.Products.Remove(selectProduct);
        await shopOnlineDbContext.SaveChangesAsync();
    }
}