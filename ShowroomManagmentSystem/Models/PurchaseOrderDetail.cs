namespace ShowroomManagmentSystem.Models
{
    [Table("PurchaseOrderDetails")]
    public class PurchaseOrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderDetailId { get; set; }

        // Foreign Key to PurchaseOrders
        [ForeignKey("PurchaseOrder")]
        public int PurchaseOrderId { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        // Foreign Key to Vehicles
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public int? Quantity { get; set; } = 0;

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? UnitPrice { get; set; } = 0;

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
