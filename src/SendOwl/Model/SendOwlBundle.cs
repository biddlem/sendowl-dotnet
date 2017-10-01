using System;

namespace SendOwl.Model
{
    public class SendOwlBundle
    {
        public int Id { get; set; }
        public Components Components { get; set; }
        public string Name { get; set; }
        public DateTime Created_at { get; set; }
        public string Price { get; set; }
        public string Currency_code { get; set; }
        public DateTime Updated_at { get; set; }
        public string Instant_buy_url { get; set; }
        public bool Price_is_minimum { get; set; }
        public string Add_to_cart_url { get; set; }
    }
}
