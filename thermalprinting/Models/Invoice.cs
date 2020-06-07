namespace thermalprinting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Invoice
    {
        public Company Company { get; set; }
        public SaleOrder Sales { get; set; }
    }
}