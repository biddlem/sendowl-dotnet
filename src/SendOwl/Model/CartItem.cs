using System;

namespace SendOwl.Model
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public DateTime? Valid_until { get; set; }
        public int? Download_attempts { get; set; }
        public long Product_id { get; set; }
        public string Price_at_checkout { get; set; }
    }
}
