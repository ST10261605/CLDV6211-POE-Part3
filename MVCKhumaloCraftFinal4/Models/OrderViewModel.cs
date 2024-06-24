using System.ComponentModel.DataAnnotations;

namespace MVCKhumaloCraftFinal4.Models
{
    public class OrderViewModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public string deliveryCountry { get; set; }
        public string shippingMethod { get; set; }
        public string shippingAddress { get; set; }
        public string phoneNumber { get; set; }
    }

}
