using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }

        //added to optimize performance
        //add a foreign key attribute, and it will declare which property in product class must ne used for joining product entity and product category entity
        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }

    }
}
