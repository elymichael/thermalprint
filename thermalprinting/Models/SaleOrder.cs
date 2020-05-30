namespace thermalprinting
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SaleOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleOrderID { get; set; }

        [Required]
        public int SalesOrderControlID { get; set; }

        [Required]
        public int CompanyID { get; set; }

        public int? PatientID { get; set; }

        [Required]
        [StringLength(1)]
        public string StatusId { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public JObject Data { get; set; }

        public DateTime CreationDate { get; set; }

        public int UserID { get; set; }

        public int? CustomerID { get; set; }
    }
}