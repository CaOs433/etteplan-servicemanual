using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class MaintenanceTaskService : IMaintenanceTaskService
    {
        private readonly FactoryDeviceDbContext _dbContext;

        public MaintenanceTaskService(FactoryDeviceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<MaintenanceTask>> GetAll()
        {
            return await _dbContext.MaintenanceTasks
                .Include(maintenanceTask => maintenanceTask.FactoryDevice)
                .OrderBy(maintenanceTask => maintenanceTask.Severity)
                .ThenBy(maintenanceTask => maintenanceTask.RegistrationTime)
                .ToListAsync();
        }

        public async Task<MaintenanceTask> Get(int id)
        {
            return await _dbContext.MaintenanceTasks
                .Include(maintenanceTask => maintenanceTask.FactoryDevice)
                .FirstOrDefaultAsync(maintenanceTask => maintenanceTask.Id == id);
        }

        public async Task<IEnumerable<MaintenanceTask>> GetByDevice(int deviceId)
        {
            return await _dbContext.MaintenanceTasks
                .Where(maintenanceTask => maintenanceTask.FactoryDeviceId == deviceId)
                .OrderBy(maintenanceTask => maintenanceTask.Severity)
                .ThenBy(maintenanceTask => maintenanceTask.RegistrationTime)
                .ToListAsync();
        }

        public async Task<MaintenanceTask> Create(MaintenanceTask maintenanceTask)
        {
            _dbContext.MaintenanceTasks.Add(maintenanceTask);
            await _dbContext.SaveChangesAsync();

            return await this.Get(maintenanceTask.Id);
        }

        public async Task<MaintenanceTask> Update(MaintenanceTask maintenanceTask)
        {
            bool taskExists = await _dbContext.MaintenanceTasks.AnyAsync(
                _maintenanceTask => _maintenanceTask.Id == maintenanceTask.Id);

            if (!taskExists)
            {
                return null;
            }

            _dbContext.MaintenanceTasks.Update(maintenanceTask);
            await _dbContext.SaveChangesAsync();

            return maintenanceTask;
        }

        public async Task<bool> Delete(int id)
        {
            MaintenanceTask maintenanceTask = await _dbContext.MaintenanceTasks.FindAsync(id);

            if (maintenanceTask == null)
            {
                return false;
            }

            _dbContext.MaintenanceTasks.Remove(maintenanceTask);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
