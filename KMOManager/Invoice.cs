using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMOManager
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public double Total   { get; set; }
        public double VAT { get; set; }
        public double TotalVAT { get; set; }
    }
}
