using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Repositories;

public class StoreItemRepository : IRepository<StoreItemModel>
{
    private readonly AppDbContext _dbContext;

    public StoreItemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<StoreItemModel>> GetAllAsync()
    {
        return await _dbContext.BookStore.ToListAsync();
    }

    public async Task<StoreItemModel> GetByIdAsync(int id)
    {
        return await _dbContext.BookStore.FindAsync(id);
    }

    public async Task<StoreItemModel> AddAsync(StoreItemModel model)
    {
        var entity = (await _dbContext.BookStore.AddAsync(model)).Entity;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<StoreItemModel> UpdateAsync(int id, StoreItemModel model)
    {
        var existingEntity = await _dbContext.BookStore.FindAsync(id);
        if (existingEntity == null)
        {
            return null;
        }

        existingEntity.ProductId = model.ProductId;
        existingEntity.AvailableQuantity = model.AvailableQuantity;
        existingEntity.BookedQuantity = model.BookedQuantity;
        existingEntity.SoldQuantity = model.SoldQuantity;

        await _dbContext.SaveChangesAsync();
        return existingEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbContext.BookStore.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbContext.BookStore.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
