namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class MaintenanceTaskDto
    {
        public int Id { get; set; }
        public FactoryDevice? FactoryDevice { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string? Description { get; set; }
        public Severity Severity { get; set; }
        public Status Status { get; set; }

    }
}
