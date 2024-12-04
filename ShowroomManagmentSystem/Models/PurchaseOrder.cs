namespace ShowroomManagmentSystem.Models
{
    [Table("PurchaseOrders")]
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderId { get; set; }

        // Foreign Key to Suppliers
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? TotalAmount { get; set; } = 0;

        [StringLength(100)]
        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(100)]
        public string? DeleteBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        [StringLength(1)]
        public string? DeleteFlag { get; set; }
    }
}
