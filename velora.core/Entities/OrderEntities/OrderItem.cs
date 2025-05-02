using velora.core.Data;

namespace velora.core.Entities.OrderEntities
{
    public class OrderItem : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}