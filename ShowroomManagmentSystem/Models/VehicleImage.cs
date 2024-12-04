namespace ShowroomManagmentSystem.Models
{
    [Table("VehicleImages")]
    public class VehicleImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleImageId { get; set; }

        [Required]
        [StringLength(200)]
        public string Path { get; set; }

        // Foreign Key to Vehicles
        [ForeignKey("VehicleDetail")]
        public int VehicleDetailId { get; set; }
        public virtual VehicleDetail VehicleDetail { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
