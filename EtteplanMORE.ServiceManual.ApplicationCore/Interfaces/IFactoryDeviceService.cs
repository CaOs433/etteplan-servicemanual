using System.Collections.Generic;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IFactoryDeviceService
    {
        Task<IEnumerable<FactoryDevice>> GetAll();
        Task<FactoryDevice> Get(int id);
        Task<FactoryDevice> Create(FactoryDevice factoryDevice);
        Task<FactoryDevice> Update(FactoryDevice factoryDevice);
        Task<bool> Delete(int id);
        Task<IEnumerable<FactoryDevice>> FilterDevicesByType(string type);
    }
}
