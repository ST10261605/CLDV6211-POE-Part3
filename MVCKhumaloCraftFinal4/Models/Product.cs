using System.ComponentModel.DataAnnotations;

namespace MVCKhumaloCraftFinal4.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }

        public string productName { get; set; }

        public string description { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }

        public bool stockAvailable { get; set; }

        public string ImageUrl { get; set; }
    }
}
