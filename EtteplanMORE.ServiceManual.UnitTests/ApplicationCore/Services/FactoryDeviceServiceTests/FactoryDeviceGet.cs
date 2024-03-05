using System;
using System.Linq;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Xunit;

namespace EtteplanMORE.ServiceManual.UnitTests.ApplicationCore.Services.FactoryDeviceServiceTests
{
    public class FactoryDeviceGet
    {
        [Fact]
        public async void AllCars()
        {
            IFactoryDeviceService factoryDeviceService = new FactoryDeviceService();

            var fds = (await factoryDeviceService.GetAll()).ToList();

            Assert.NotNull(fds);
            Assert.NotEmpty(fds);
            Assert.Equal(3, fds.Count);
        }

        [Fact]
        public async void ExistingCardWithId()
        {
            IFactoryDeviceService FactoryDeviceService = new FactoryDeviceService();
            int fdId = 1;

            var fd = await FactoryDeviceService.Get(fdId);

            Assert.NotNull(fd);
            Assert.Equal(fdId, fd.Id);
        }

        [Fact]
        public async void NonExistingCardWithId()
        {
            IFactoryDeviceService FactoryDeviceService = new FactoryDeviceService();
            int fdId = 4;

            var fd = await FactoryDeviceService.Get(fdId);

            Assert.Null(fd);
        }
    }
}