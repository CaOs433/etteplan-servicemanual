using System.Collections.Generic;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IMaintenanceTaskService
    {
        Task<IEnumerable<MaintenanceTask>> GetAll();
        Task<MaintenanceTask> Get(int id);
        Task<IEnumerable<MaintenanceTask>> GetByDevice(int deviceId);
        Task<MaintenanceTask> Create(MaintenanceTask maintenanceTask);
        Task<MaintenanceTask> Update(MaintenanceTask maintenanceTask);
        Task<bool> Delete(int id);
    }
}
