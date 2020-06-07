namespace thermalprinting.Models
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Company
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyID { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        public JObject Data { get; set; }

        public bool Active { get; set; }

        public int EntityID { get; set; }

        public DateTime CreationDate { get; set; }
    }
}