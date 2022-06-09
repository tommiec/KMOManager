using System;
using System.ComponentModel.DataAnnotations;

namespace KMOManager
{

    public enum Role
    {
        Admin,
        Warehouse,
        Sales
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Role Role { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime LastSeen { get; set; }
        public bool IsActive { get; set; }
    }
}
