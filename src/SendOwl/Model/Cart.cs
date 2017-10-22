using System;
using System.Collections.Generic;

namespace SendOwl.Model
{
    public class Cart
    {
        public string State { get; set; }
        public DateTime Started_checkout_at { get; set; }
        public DateTime Completed_checkout_at { get; set; }
        public int? Discount_code_id { get; set; }
        public string Discount_code { get; set; }
        public List<CartItem> Cart_items { get; set; }
    }
}
