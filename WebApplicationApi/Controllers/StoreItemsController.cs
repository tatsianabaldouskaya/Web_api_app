using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationApi.Authentication;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.StoreItem;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Role.Manager))]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class StoreItemsController : ControllerBase
    {
        private IRepository<StoreItemModel> _repository;

        public StoreItemsController(IRepository<StoreItemModel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreItemModel>>> GetStoreItems()
        {
            var entities = await _repository.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreItemModel>> GetStoreItem(int id)
        {
            var storeItemModel = await _repository.GetByIdAsync(id);

            if (storeItemModel == null)
            {
                return NotFound(new { message = "Store item was not found" });
            }

            return Ok(storeItemModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStoreItem(int id, StoreItemDto storeItemDto)
        {
            var storeItem = new StoreItemModel()
            {
                AvailableQuantity = storeItemDto.AvailableQuantity,
                BookedQuantity = storeItemDto.BookedQuantity,
                SoldQuantity = storeItemDto.SoldQuantity
            };

            var result = await _repository.UpdateAsync(id, storeItem);

            if (result == null)
            {
                return NotFound(new { message = "Store item was not found" });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<StoreItemModel>> CreateStoreItem(StoreItemDto storeItemDto)
        {
            var storeItem = new StoreItemModel()
            {
                ProductId = storeItemDto.Product.Id,
                AvailableQuantity = storeItemDto.AvailableQuantity,
                BookedQuantity = storeItemDto.BookedQuantity,
                SoldQuantity = storeItemDto.SoldQuantity,
            };

            var createdItem = await _repository.AddAsync(storeItem);

            return CreatedAtAction(nameof(CreateStoreItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreItem(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(new { message = "Store item was not found" });
            }

            return Ok();
        }
    }
}
