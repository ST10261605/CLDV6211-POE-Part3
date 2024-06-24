using MVCKhumaloCraftFinal4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCKhumaloCraftFinal4.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int userID { get; set; }
        public User User { get; set; }
        public int Quantity { get; set; }
    }
}
