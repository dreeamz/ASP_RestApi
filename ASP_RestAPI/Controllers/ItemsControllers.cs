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

        //Get / items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            return repository.GetItems().Select<Item,ItemDto>(item =>item.AsDto());
        }
        
        //Get /items/id
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id).AsDto();
            
            if(item is null)
                return NotFound();

            return item;
        }
    }
}