using ASP_RestAPI.Dtos;
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
        public IEnumerable<ItemDto> GetItems()
        {
            return repository.GetItems().Select<Item, ItemDto>(item => item.AsDto());
        }

        //Get /items/id
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id).AsDto();

            if (item is null)
                return NotFound();

            return item;
        }


        //POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }

        //PUT /items
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(id);

            if (existingItem is null)
                return NotFound();

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }

        //DELETE /items
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            Item item = repository.GetItem(id);
            
            if (item is not null)
            {
                repository.DeleteItem(id);
                return NoContent();
            }
            return NotFound();

        }
    }

}