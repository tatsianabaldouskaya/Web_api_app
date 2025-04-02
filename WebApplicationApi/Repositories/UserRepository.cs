using Microsoft.EntityFrameworkCore;

using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Repositories;

public class UserRepository : IRepository<UserModel>
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<UserModel> GetByIdAsync(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<UserModel> AddAsync(UserModel model)
    {
        var entity = (await _dbContext.Users.AddAsync(model)).Entity;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<UserModel> UpdateAsync(int id, UserModel model)
    {
        var existingEntity = await _dbContext.Users.FindAsync(id);
        if (existingEntity == null)
        {
            return null;
        }

        existingEntity.Name = model.Name;
        existingEntity.Email = model.Email;
        existingEntity.Password = model.Password;
        existingEntity.Address = model.Address;
        existingEntity.Phone = model.Phone;
        existingEntity.RoleId = model.RoleId;
        
        await _dbContext.SaveChangesAsync();
        return existingEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbContext.Users.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbContext.Users.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
