using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationApi.Authentication;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.Booking;
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
        [Authorize(Roles = $"{nameof(Role.Customer)},{nameof(Role.Manager)}")]
        public async Task<ActionResult<IEnumerable<BookingModel>>> GetBookings()
        {
            var bookings = await _repository.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Role.Customer)},{nameof(Role.Manager)}")]
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
        [Authorize(Roles = $"{nameof(Role.Customer)},{nameof(Role.Manager)}")]
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
        [Authorize(Roles = $"{nameof(Role.Customer)},{nameof(Role.Manager)}")]
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
        [Authorize(Roles = $"{nameof(Role.Customer)},{nameof(Role.Manager)}")]
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
