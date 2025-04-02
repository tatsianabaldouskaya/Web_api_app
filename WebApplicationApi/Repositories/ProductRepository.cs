using Microsoft.EntityFrameworkCore;

using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Repositories;

public class ProductRepository : IRepository<ProductModel>
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<ProductModel> GetByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);
    }

    public async Task<ProductModel> AddAsync(ProductModel product)
    {
        var entity = (await _dbContext.Products.AddAsync(product)).Entity;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<ProductModel> UpdateAsync(int id, ProductModel product)
    {
        var existingProduct = await _dbContext.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Author = product.Author;
        existingProduct.Price = product.Price;
        existingProduct.ImagePath = product.ImagePath;

        await _dbContext.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
