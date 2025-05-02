namespace velora.repository.Cart.Models
{
    public class CartItem
    {
        public int ProductId { get; set; } 
        public string ProductName { get; set; } 
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }

    }
}