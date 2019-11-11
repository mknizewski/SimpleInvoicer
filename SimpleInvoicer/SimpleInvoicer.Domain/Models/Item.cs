namespace SimpleInvoicer.Domain.Models
{
    public class Item
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public Units Unit { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public ItemTax Tax { get; set; }
        public decimal TaxAmmount { get; set; }
        public decimal ValueGross { get; set; }
    }
}
