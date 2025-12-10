namespace ShoppingCart.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo  { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Item() { }

        public Item(ProductCart productCart) 
        {
            Id = productCart.Id;
            Name = productCart.Name;
            Photo = productCart.Photo;
            Price = productCart.Price;
        }
    }
}
