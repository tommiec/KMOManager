using System.Collections.Generic;

namespace KMOManager
{
    public class Audience
    {
        public int AudienceId { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Product { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
