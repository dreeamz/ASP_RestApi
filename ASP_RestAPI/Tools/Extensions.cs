using ASP_RestAPI.Dtos;
using ASP_RestAPI.Entities;

namespace ASP_RestAPI.Tools
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
             return new ItemDto 
             {
                 Id = item.Id,
                 Name = item.Name,
                 Price = item.Price,
                 CreatedDate = item.CreatedDate
             };
        }
    }
}