
namespace velora.services.Services.OrderService.Dto
{
    public class OrderItemDto
    {
        public Guid OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public  int ProductId  { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }


    }
}
