namespace ShowroomManagmentSystem.Models
{
    [Table("VehicleDetails")]
    public class VehicleDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleDetailId { get; set; }

        // Foreign Key to Vehicles
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        [StringLength(50)]
        public string EngineNumber { get; set; }

        [StringLength(50)]
        public string ChassisNumber { get; set; }

        [StringLength(30)]
        public string Color { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string? FuelType { get; set; }

        public int? ManufactureYear { get; set; }

        [StringLength(20)]
        public string? TransmissionType { get; set; }

        public int? Mileage { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Weight { get; set; }

        [StringLength(50)]
        public string? Dimensions { get; set; }

        [StringLength(10)]
        public string? ColorCode { get; set; }

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
