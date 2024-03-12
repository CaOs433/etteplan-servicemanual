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

namespace EtteplanMORE.ServiceManual.UnitTests.Web.Controllers.MaintenanceTasksControllerTests
{
    public class MaintenanceTasksControllerTests
    {
        private readonly Mock<IMaintenanceTaskService> _mockMaintenanceTaskService;
        private readonly MaintenanceTasksController _controller;

        private readonly List<FactoryDevice> _factoryDevices;
        private readonly List<MaintenanceTask> _maintenanceTasks;

        public MaintenanceTasksControllerTests()
        {
            _mockMaintenanceTaskService = new Mock<IMaintenanceTaskService>();
            _controller = new MaintenanceTasksController(_mockMaintenanceTaskService.Object);

            _factoryDevices =
            [
                new FactoryDevice { Id = 0, Name = "Device 1", Year = 2007, Type = "Type 1" },
                new FactoryDevice { Id = 1, Name = "Device 2", Year = 2000, Type = "Type 2" },
                new FactoryDevice { Id = 2, Name = "Device 3", Year = 1998, Type = "Type 3" }
            ];

            _maintenanceTasks = [];
            _factoryDevices.ForEach(device =>
                _maintenanceTasks.Add(new MaintenanceTask()
                {
                    Id = device.Id,
                    FactoryDevice = device,
                    RegistrationTime = DateTime.Now,
                    Description = $"Task {device.Id}",
                    Severity = Severity.critical,
                    Status = Status.open
                }));
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithMaintenanceTasks()
        {
            // Arrange
            List<MaintenanceTask> tasks = _maintenanceTasks;
            _mockMaintenanceTaskService.Setup(service => service.GetAll()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<MaintenanceTaskDto> maintenanceTasks = Assert.IsAssignableFrom<IEnumerable<MaintenanceTaskDto>>(okResult.Value);
            Assert.Equal(tasks.Count, maintenanceTasks.Count());
            maintenanceTasks.ToList().ForEach(maintenanceTask =>
            {
                var task = tasks.Find(t => t.Id == maintenanceTask.Id);
                Assert.Equal(task.Id, maintenanceTask.Id);
                Assert.Equal(task.FactoryDevice, maintenanceTask.FactoryDevice);
                Assert.Equal(task.RegistrationTime, maintenanceTask.RegistrationTime);
                Assert.Equal(task.Description, maintenanceTask.Description);
                Assert.Equal(task.Severity, maintenanceTask.Severity);
                Assert.Equal(task.Status, maintenanceTask.Status);
            });
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithMaintenanceTask()
        {
            // Arrange
            MaintenanceTask task = _maintenanceTasks[0];
            _mockMaintenanceTaskService.Setup(service => service.Get(task.Id)).ReturnsAsync(task);

            // Act
            ActionResult<MaintenanceTaskDto> result = await _controller.Get(task.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            MaintenanceTaskDto maintenanceTaskDto = Assert.IsType<MaintenanceTaskDto>(okResult.Value);
            Assert.Equal(task.Id, maintenanceTaskDto.Id);
            Assert.Equal(task.FactoryDevice, maintenanceTaskDto.FactoryDevice);
            Assert.Equal(task.RegistrationTime, maintenanceTaskDto.RegistrationTime);
            Assert.Equal(task.Description, maintenanceTaskDto.Description);
            Assert.Equal(task.Severity, maintenanceTaskDto.Severity);
            Assert.Equal(task.Status, maintenanceTaskDto.Status);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtAction()
        {
            MaintenanceTask task = _maintenanceTasks[0];
            MaintenanceTaskDto taskDto = new MaintenanceTaskDto()
            {
                FactoryDeviceId = task.FactoryDevice.Id,
                RegistrationTime = task.RegistrationTime,
                Description = task.Description,
                Severity = task.Severity,
                Status = task.Status
            };
            _mockMaintenanceTaskService.Setup(service => service.Create(It.IsAny<MaintenanceTask>())).ReturnsAsync(task);

            // Act
            var result = await _controller.Post(taskDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(MaintenanceTasksController.Get), createdAtActionResult.ActionName);
            Assert.Equal(taskDto.Id, createdAtActionResult.RouteValues["id"]);

            MaintenanceTaskDto createdMaintenanceTaskDto = Assert.IsType<MaintenanceTaskDto>(createdAtActionResult.Value);
            Assert.Equal(taskDto.Id, createdMaintenanceTaskDto.Id);
            Assert.Equal(taskDto.FactoryDeviceId, createdMaintenanceTaskDto.FactoryDevice.Id);
            Assert.Equal(taskDto.RegistrationTime, createdMaintenanceTaskDto.RegistrationTime);
            Assert.Equal(taskDto.Description, createdMaintenanceTaskDto.Description);
            Assert.Equal(taskDto.Severity, createdMaintenanceTaskDto.Severity);
            Assert.Equal(taskDto.Status, createdMaintenanceTaskDto.Status);
        }

        [Fact]
        public async Task Put_ReturnsOkResultWithUpdatedMaintenanceTask()
        {
            // Arrange
            MaintenanceTask task = _maintenanceTasks[0];
            MaintenanceTaskDto taskDto = new MaintenanceTaskDto()
            {
                Id = task.Id,
                FactoryDeviceId = task.FactoryDevice.Id,
                RegistrationTime = task.RegistrationTime,
                Description = "Updated Maintenance Task",
                Severity = task.Severity,
                Status = task.Status
            };
            _mockMaintenanceTaskService.Setup(service => service.Get(task.Id)).ReturnsAsync(task);
            _mockMaintenanceTaskService.Setup(service => service.Update(It.IsAny<MaintenanceTask>())).ReturnsAsync(task);

            // Act
            var result = await _controller.Put(task.Id, taskDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            MaintenanceTaskDto updatedMaintenanceTaskDto = Assert.IsType<MaintenanceTaskDto>(okResult.Value);
            Assert.Equal(taskDto.Id, updatedMaintenanceTaskDto.Id);
            Assert.Equal(taskDto.FactoryDeviceId, updatedMaintenanceTaskDto.FactoryDevice.Id);
            Assert.Equal(taskDto.RegistrationTime, updatedMaintenanceTaskDto.RegistrationTime);
            Assert.Equal(taskDto.Description, updatedMaintenanceTaskDto.Description);
            Assert.Equal(taskDto.Severity, updatedMaintenanceTaskDto.Severity);
            Assert.Equal(taskDto.Status, updatedMaintenanceTaskDto.Status);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            MaintenanceTask task = _maintenanceTasks[0];
            _mockMaintenanceTaskService.Setup(service => service.Get(task.Id)).ReturnsAsync(task);

            // Act
            var result = await _controller.Delete(task.Id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

    }
}
