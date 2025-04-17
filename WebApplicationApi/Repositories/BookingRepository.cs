using Microsoft.EntityFrameworkCore;

using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Repositories;

public class BookingRepository : IRepository<BookingModel>
{
    private readonly AppDbContext _dbContext;

    public BookingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BookingModel>> GetAllAsync()
    {
        return await _dbContext.Bookings.ToListAsync();
    }

    public async Task<BookingModel> GetByIdAsync(int id)
    {
        return await _dbContext.Bookings.FindAsync(id);
    }

    public async Task<BookingModel> AddAsync(BookingModel model)
    {
        var entity = (await _dbContext.Bookings.AddAsync(model)).Entity;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<BookingModel> UpdateAsync(int id, BookingModel model)
    {
        var entity = await _dbContext.Bookings.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Date = model.Date;
        entity.DeliveryAddress = model.DeliveryAddress;
        entity.ProductId = model.ProductId;
        entity.StatusId = model.StatusId;
        entity.Time = model.Time;
        entity.UserId = model.UserId;

        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbContext.Bookings.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbContext.Bookings.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
