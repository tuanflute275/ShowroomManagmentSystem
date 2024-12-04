namespace ShowroomManagmentSystem.Models
{
    [Table("SalesOrders")]
    public class SalesOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderId { get; set; }

        // Foreign Key to Customers
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public DateTime? OrderDate { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
    }
}
