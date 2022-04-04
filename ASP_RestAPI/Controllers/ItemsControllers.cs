using ASP_RestAPI.Repositories;
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
        public IEnumerable<Item> GetItems()
        {
            return repository.GetItems();           
        }

        //Get /items/id
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            
            if(item is null)
                return NotFound();

            return item;
        }
    }
}