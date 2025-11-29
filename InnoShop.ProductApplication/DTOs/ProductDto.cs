using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public Guid UserId {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
