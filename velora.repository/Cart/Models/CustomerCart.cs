namespace velora.repository.Cart.Models
{
    public class CustomerCart
    {
        public string? Id { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        //public string? PaymentIntentId { get; set; }
        //public string? ClientSecret { get; set; }
    }
}