using System.Text.RegularExpressions;

namespace KMOManager
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }


        public override string ToString()
        {
            return FirstName + " " + LastName + " " + Name;
        }

    }

}