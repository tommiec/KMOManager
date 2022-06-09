namespace KMOManager
{

    public class Product
    {
        public int ProductId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Colour { get; set; }

        public int AudienceId { get; set; }
        public Audience Audience { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int Stock { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return Brand + " " + Name + " (" + Colour + ", " + Size + ")";
        }

    }
}
