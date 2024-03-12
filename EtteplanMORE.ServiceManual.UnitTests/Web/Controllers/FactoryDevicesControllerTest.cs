using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EtteplanMORE.ServiceManual.UnitTests.Web.Controllers.FactoryDevicesControllerTests
{
    public class FactoryDevicesControllerTests
    {
        private readonly Mock<IFactoryDeviceService> _mockFactoryDeviceService;
        private readonly FactoryDevicesController _controller;

        private readonly List<FactoryDevice> _factoryDevices;

        public FactoryDevicesControllerTests()
        {
            _mockFactoryDeviceService = new Mock<IFactoryDeviceService>();
            _controller = new FactoryDevicesController(_mockFactoryDeviceService.Object);

            _factoryDevices =
            [
                new FactoryDevice { Id = 0, Name = "Device 1", Year = 2007, Type = "Type 1" },
                new FactoryDevice { Id = 1, Name = "Device 2", Year = 2000, Type = "Type 2" },
                new FactoryDevice { Id = 2, Name = "Device 3", Year = 1998, Type = "Type 3" }
            ];
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithFactoryDevices()
        {
            // Arrange
            List<FactoryDevice> devices = _factoryDevices;
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
            FactoryDevice device = _factoryDevices[0];
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

        [Fact]
        public async Task Post_ReturnsCreatedAtAction()
        {
            // Arrange
            FactoryDevice device = _factoryDevices[0];
            FactoryDeviceDto deviceDto = new FactoryDeviceDto { Name = device.Name, Year = device.Year, Type = device.Type };
            _mockFactoryDeviceService.Setup(service => service.Create(It.IsAny<FactoryDevice>())).ReturnsAsync(device);

            // Act
            var result = await _controller.Post(deviceDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FactoryDevicesController.Get), createdAtActionResult.ActionName);
            Assert.Equal(device.Id, createdAtActionResult.RouteValues["id"]);

            FactoryDeviceDto createdFactoryDevice = Assert.IsType<FactoryDeviceDto>(createdAtActionResult.Value);
            Assert.Equal(device.Id, createdFactoryDevice.Id);
            Assert.Equal(device.Name, createdFactoryDevice.Name);
            Assert.Equal(device.Year, createdFactoryDevice.Year);
            Assert.Equal(device.Type, createdFactoryDevice.Type);
        }

        [Fact]
        public async Task Put_ReturnsOkResultWithUpdatedFactoryDevice()
        {
            // Arrange
            FactoryDevice device = _factoryDevices[0];
            FactoryDeviceDto deviceDto = new FactoryDeviceDto { Id = device.Id, Name = "Updated Device", Year = 2022, Type = "Type 2" };
            _mockFactoryDeviceService.Setup(service => service.Get(device.Id)).ReturnsAsync(device);
            _mockFactoryDeviceService.Setup(service => service.Update(It.IsAny<FactoryDevice>())).ReturnsAsync(device);

            // Act
            var result = await _controller.Put(device.Id, deviceDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            FactoryDeviceDto updatedFactoryDevice = Assert.IsType<FactoryDeviceDto>(okResult.Value);
            Assert.Equal(device.Id, updatedFactoryDevice.Id);
            Assert.Equal(deviceDto.Name, updatedFactoryDevice.Name);
            Assert.Equal(deviceDto.Year, updatedFactoryDevice.Year);
            Assert.Equal(deviceDto.Type, updatedFactoryDevice.Type);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            FactoryDevice device = _factoryDevices[0];
            _mockFactoryDeviceService.Setup(service => service.Get(device.Id)).ReturnsAsync(device);

            // Act
            var result = await _controller.Delete(device.Id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

    }
}
