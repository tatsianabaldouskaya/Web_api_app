using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Authentication;
using WebApplicationApi.Data;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Booking;
using WebApplicationApi.Repositories;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class BookingsController : ControllerBase
    {
        private readonly IRepository<BookingModel> _repository;

        public BookingsController(IRepository<BookingModel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingModel>>> GetBookings()
        {
            var bookings = await _repository.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingModel>> GetBooking(int id)
        {
            var bookingModel = await _repository.GetByIdAsync(id);

            if (bookingModel == null)
            {
                return NotFound(new { message = "Booking was not found" });
            }

            return Ok(bookingModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBooking(int id, BookingDto bookingDto)
        {
            var booking = new BookingModel()
            {
                Date = bookingDto.Date,
                Time = bookingDto.Time,
                Quantity = bookingDto.Quantity
            };

            var result = await _repository.UpdateAsync(id, booking);

            if (result == null)
            {
                return NotFound(new { message = "Booking was not found" });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookingModel>> CreateBooking(BookingDto bookingDto)
        {
            var booking = new BookingModel()
            {
                UserId = bookingDto.User.Id,
                ProductId = bookingDto.Product.Id,
                DeliveryAddress = bookingDto.User.Address,
                Date = DateTime.Now.Date,
                Time = DateTime.Now,
                StatusId = (int)bookingDto.Status,
                Quantity = bookingDto.Quantity
            };

            var createdBooking = await _repository.AddAsync(booking);

            return CreatedAtAction(nameof(CreateBooking), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound( new { message = "Booking was not found" });
            }

            return Ok();
        }
    }
}
