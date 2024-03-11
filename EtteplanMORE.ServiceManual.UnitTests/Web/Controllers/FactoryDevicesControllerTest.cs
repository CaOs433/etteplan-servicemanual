using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EtteplanMORE.ServiceManual.UnitTests.Web.Controllers.FactoryDevicesControllerTests
{
    public class FactoryDevicesControllerTests
    {
        private readonly Mock<IFactoryDeviceService> _mockFactoryDeviceService;
        private readonly FactoryDevicesController _controller;

        public FactoryDevicesControllerTests()
        {
            _mockFactoryDeviceService = new Mock<IFactoryDeviceService>();
            _controller = new FactoryDevicesController(_mockFactoryDeviceService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithFactoryDevices()
        {
            // Arrange
            var devices = new List<FactoryDevice>
            {
                new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" },
                new FactoryDevice { Id = 2, Name = "Device 2", Year = 2022, Type = "Type 2" }
            };
            _mockFactoryDeviceService.Setup(service => service.GetAll()).ReturnsAsync(devices);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var factoryDevices = Assert.IsAssignableFrom<IEnumerable<FactoryDeviceDto>>(okResult.Value);
            Assert.Equal(devices.Count, factoryDevices.Count());
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithFactoryDevice()
        {
            // Arrange
            var device = new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" };
            _mockFactoryDeviceService.Setup(service => service.Get(device.Id)).ReturnsAsync(device);

            // Act
            var result = await _controller.Get(device.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var factoryDevice = Assert.IsType<FactoryDeviceDto>(okResult.Value);
            Assert.Equal(device.Id, factoryDevice.Id);
            Assert.Equal(device.Name, factoryDevice.Name);
            Assert.Equal(device.Year, factoryDevice.Year);
            Assert.Equal(device.Type, factoryDevice.Type);
        }

        // ...

    }
}
