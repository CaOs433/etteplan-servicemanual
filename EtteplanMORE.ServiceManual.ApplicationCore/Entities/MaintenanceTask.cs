using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public enum Severity
    {
        critical,
        important,
        unimportant
    }

    public enum Status
    {
        open,
        closed
    }

    public class MaintenanceTask
    {
        public int Id { get; set; }

        [ForeignKey("FactoryDevice")]
        public int FactoryDeviceId { get; set; }
        public FactoryDevice FactoryDevice { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string Description { get; set; }
        public Severity Severity { get; set; }
        public Status Status { get; set; }
    }
}
