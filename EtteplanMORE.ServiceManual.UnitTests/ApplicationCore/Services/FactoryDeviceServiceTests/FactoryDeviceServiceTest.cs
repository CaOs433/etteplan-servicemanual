using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EtteplanMORE.ServiceManual.UnitTests.ApplicationCore.Services.FactoryDeviceServiceTests
{
    public class FactoryDeviceServiceTests
    {
        private readonly Mock<FactoryDeviceDbContext> _mockDbContext;
        private readonly FactoryDeviceService _factoryDeviceService;

        public FactoryDeviceServiceTests()
        {
            _mockDbContext = new Mock<FactoryDeviceDbContext>();
            _factoryDeviceService = new FactoryDeviceService(_mockDbContext.Object);
        }

        /*[Fact]
        public async Task GetAll_ReturnsAllFactoryDevices()
        {
            // Arrange
            var devices = new List<FactoryDevice>
            {
                new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" },
                new FactoryDevice { Id = 2, Name = "Device 2", Year = 2022, Type = "Type 2" }
            };
            var mockDbSet = devices.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(db => db.FactoryDevices).Returns(mockDbSet.Object);

            // Act
            var result = await _factoryDeviceService.GetAll();

            // Assert
            Assert.Equal(devices.Count, result.Count());
            Assert.Equal(devices, result);
        }*/

        [Fact]
        public async Task Get_ReturnsFactoryDeviceById()
        {
            // Arrange
            var device = new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" };
            _mockDbContext.Setup(db => db.FactoryDevices.FindAsync(device.Id)).ReturnsAsync(device);

            // Act
            var result = await _factoryDeviceService.Get(device.Id);

            // Assert
            Assert.Equal(device, result);
        }

        /*[Fact]
        public async Task Create_ReturnsCreatedFactoryDevice()
        {
            // Arrange
            var device = new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" };
            _mockDbContext.Setup(db => db.FactoryDevices.Add(device));
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _factoryDeviceService.Create(device);

            // Assert
            Assert.Equal(device, result);
        }*/

        /*[Fact]
        public async Task Update_ReturnsUpdatedFactoryDevice()
        {
            // Arrange
            var device = new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" };
            _mockDbContext.Setup(db => db.FactoryDevices.AnyAsync(d => d.Id == device.Id)).ReturnsAsync(true);
            _mockDbContext.Setup(db => db.FactoryDevices.Update(device));
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _factoryDeviceService.Update(device);

            // Assert
            Assert.Equal(device, result);
        }*/

        /*[Fact]
        public async Task Update_ReturnsNullWhenDeviceDoesNotExist()
        {
            // Arrange
            var device = new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" };
            _mockDbContext.Setup(db => db.FactoryDevices.AnyAsync(d => d.Id == device.Id)).ReturnsAsync(false);

            // Act
            var result = await _factoryDeviceService.Update(device);

            // Assert
            Assert.Null(result);
        }*/

        /*[Fact]
        public async Task Delete_ReturnsTrueWhenDeviceIsDeleted()
        {
            // Arrange
            var device = new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" };
            _mockDbContext.Setup(db => db.FactoryDevices.FindAsync(device.Id)).ReturnsAsync(device);
            _mockDbContext.Setup(db => db.FactoryDevices.Remove(device));
            _mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _factoryDeviceService.Delete(device.Id);

            // Assert
            Assert.True(result);
        }*/

        [Fact]
        public async Task Delete_ReturnsFalseWhenDeviceDoesNotExist()
        {
            // Arrange
            var deviceId = 1;
            _mockDbContext.Setup(db => db.FactoryDevices.FindAsync(deviceId)).ReturnsAsync((FactoryDevice)null);

            // Act
            var result = await _factoryDeviceService.Delete(deviceId);

            // Assert
            Assert.False(result);
        }

        /*[Fact]
        public async Task FilterDevicesByType_ReturnsAllDevicesWhenTypeIsNull()
        {
            // Arrange
            var devices = new List<FactoryDevice>
            {
                new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" },
                new FactoryDevice { Id = 2, Name = "Device 2", Year = 2022, Type = "Type 2" }
            };
            var mockDbSet = devices.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(db => db.FactoryDevices).Returns(mockDbSet.Object);

            // Act
            var result = await _factoryDeviceService.FilterDevicesByType(null);

            // Assert
            Assert.Equal(devices.Count, result.Count());
            Assert.Equal(devices, result);
        }*/

        /*[Fact]
        public async Task FilterDevicesByType_ReturnsFilteredDevicesByType()
        {
            // Arrange
            var devices = new List<FactoryDevice>
            {
                new FactoryDevice { Id = 1, Name = "Device 1", Year = 2021, Type = "Type 1" },
                new FactoryDevice { Id = 2, Name = "Device 2", Year = 2022, Type = "Type 2" },
                new FactoryDevice { Id = 3, Name = "Device 3", Year = 2023, Type = "Type 3" }
            };
            var mockDbSet = devices.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(db => db.FactoryDevices).Returns(mockDbSet.Object);

            // Act
            var result = await _factoryDeviceService.FilterDevicesByType("Type 2");

            // Assert
            Assert.Single(result);
            Assert.Equal(devices[1], result.First());
        }*/
    }
}
