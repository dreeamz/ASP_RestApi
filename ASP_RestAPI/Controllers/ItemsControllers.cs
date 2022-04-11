using ASP_RestAPI.Dtos;
using ASP_RestAPI.Entities;
using ASP_RestAPI.Repositories;
using ASP_RestAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ASP_RestAPI
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;
        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        //Get /items
        [HttpGet]
        public  IEnumerable<ItemDto> GetItems()
        {
            return  repository.GetItems().Select<Item, ItemDto>(item => item.AsDto());
        }

        //Get /items/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item =await repository.GetItemAsync(id);

            if (item is null)
                return NotFound();

            return item.AsDto();
        }


        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        //PUT /items
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem =await repository.GetItemAsync(id);

            if (existingItem is null)
                return NotFound();

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        //DELETE /items
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            Item item = await repository.GetItemAsync(id);
            
            if (item is not null)
            {
                await repository.DeleteItemAsync(id);
                return NoContent();
            }
            return NotFound();

        }
    }

}