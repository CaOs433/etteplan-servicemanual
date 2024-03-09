using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class FactoryDeviceService : IFactoryDeviceService
    {
        private readonly FactoryDeviceDbContext _dbContext;

        public FactoryDeviceService() { }

        public FactoryDeviceService(FactoryDeviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FactoryDevice>> GetAll()
        {
            return await _dbContext.FactoryDevices.ToListAsync();
        }

        public async Task<FactoryDevice> Get(int id)
        {
            return await _dbContext.FactoryDevices.FindAsync(id);
        }

        public async Task<FactoryDevice> Create(FactoryDevice factoryDevice)
        {
            _dbContext.FactoryDevices.Add(factoryDevice);
            await _dbContext.SaveChangesAsync();

            return factoryDevice;
        }

        public async Task<FactoryDevice> Update(FactoryDevice factoryDevice)
        {
            bool deviceExists = await _dbContext.FactoryDevices.AnyAsync(
                _factoryDevice => _factoryDevice.Id == factoryDevice.Id);

            if (!deviceExists)
            {
                return null;
            }

            _dbContext.FactoryDevices.Update(factoryDevice);
            await _dbContext.SaveChangesAsync();

            return factoryDevice;
        }

        public async Task<bool> Delete(int id)
        {
            FactoryDevice device = await _dbContext.FactoryDevices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            _dbContext.FactoryDevices.Remove(device);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<FactoryDevice>> FilterDevicesByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return await _dbContext.FactoryDevices.ToListAsync();
            }

            return await _dbContext.FactoryDevices
                .Where(d => EF.Functions.Like(d.Type, $"%{type}%"))
                .ToListAsync();
        }
    }
}
