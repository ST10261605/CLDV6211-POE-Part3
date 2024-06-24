using MVCKhumaloCraftFinal4.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCKhumaloCraftFinal4.Models
{
    public class Order
    {
        [Key]
        public int orderID { get; set; }

        public int userID { get; set; }

        public User User { get; set; }

        public int productID { get; set; }

        public Product Product { get; set; }

        public string deliveryCountry { get; set; }

        public string shippingMethod { get; set; }

        public string shippingAddress { get; set; }

        public string phoneNumber { get; set; }

        public string orderStatus { get; set; } = "Processing"; // Default value for new orders
    }
}
