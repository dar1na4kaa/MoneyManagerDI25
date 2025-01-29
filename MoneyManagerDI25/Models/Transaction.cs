namespace MoneyManagerX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Transaction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }

        public virtual Account Account { get; set; }

        public virtual Category Category { get; set; }
    }
}
