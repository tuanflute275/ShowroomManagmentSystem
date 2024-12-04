namespace ShowroomManagmentSystem.Models
{
    [Table("Vehicles")]
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }

        [StringLength(50)]
        public string ModelNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Image { get; set; }

        [StringLength(20)]
        public string? Status { get; set; } = "Active";

        public DateTime? DateAdded { get; set; } = DateTime.Now;

        public string? Description { get; set; }

        // Foreign Key to Suppliers
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        // Foreign Key to Branches
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

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
