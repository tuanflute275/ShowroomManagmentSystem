namespace ShowroomManagmentSystem.Models
{
    [Table("ServiceOrders")]
    public class ServiceOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceOrderId { get; set; }

        // Foreign Key to Vehicles
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public DateTime? ServiceDate { get; set; }

        [Column(TypeName = "text")]
        public string ServiceDetails { get; set; }

        // Foreign Key to Employees (Technician)
        [ForeignKey("Technician")]
        public int TechnicianId { get; set; }
        public virtual Account Technician { get; set; }
    }
}
