using System;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTasksController : Controller
    {
        private readonly IMaintenanceTaskService _maintenanceTaskService;

        public MaintenanceTasksController(IMaintenanceTaskService maintenanceTaskService)
        {
            _maintenanceTaskService = maintenanceTaskService;
        }

        /// <summary>
        ///     HTTP GET: api/MaintenanceTasks/
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok((await _maintenanceTaskService.GetAll())
                    .Select(maintenanceTask =>
                        new MaintenanceTaskDto()
                        {
                            Id = maintenanceTask.Id,
                            FactoryDevice = maintenanceTask.FactoryDevice,
                            RegistrationTime = maintenanceTask.RegistrationTime,
                            Description = maintenanceTask.Description,
                            Severity = maintenanceTask.Severity,
                            Status = maintenanceTask.Status
                        }
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal server error occurred. See the server logs for more details.");
            }
        }

        /// <summary>
        ///     HTTP GET: api/MaintenanceTasks/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceTaskDto>> Get(int id)
        {
            try
            {
                var maintenanceTask = await _maintenanceTaskService.Get(id);

                if (maintenanceTask == null)
                {
                    return NotFound("No maintenance task found with the given ID.");
                }

                return Ok(new MaintenanceTaskDto()
                    {
                        Id = maintenanceTask.Id,
                        FactoryDevice = maintenanceTask.FactoryDevice,
                        RegistrationTime = maintenanceTask.RegistrationTime,
                        Description = maintenanceTask.Description,
                        Severity = maintenanceTask.Severity,
                        Status = maintenanceTask.Status
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal server error occurred. See the server logs for more details.");
            }
        }

        /// <summary>
        ///     HTTP POST: api/MaintenanceTasks/
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MaintenanceTaskDto maintenanceTaskDto)
        {
            try
            {
                if (maintenanceTaskDto.FactoryDeviceId == null)
                {
                    return BadRequest("FactoryDeviceId required.");
                }
                // Convert timezone to UTC
                DateTime registrationTimeUtc = maintenanceTaskDto.RegistrationTime.ToUniversalTime();

                MaintenanceTask maintenanceTask = new MaintenanceTask()
                {
                    FactoryDeviceId = (int)maintenanceTaskDto.FactoryDeviceId,
                    RegistrationTime = registrationTimeUtc,
                    Description = maintenanceTaskDto.Description,
                    Severity = maintenanceTaskDto.Severity,
                    Status = maintenanceTaskDto.Status
                };
                MaintenanceTask createdMaintenanceTask = await _maintenanceTaskService.Create(maintenanceTask);

                return CreatedAtAction(nameof(Get), new { id = maintenanceTask.Id }, new MaintenanceTaskDto()
                    {
                        Id = maintenanceTask.Id,
                        FactoryDevice = createdMaintenanceTask.FactoryDevice,
                        RegistrationTime = maintenanceTask.RegistrationTime,
                        Description = maintenanceTask.Description,
                        Severity = maintenanceTask.Severity,
                        Status = maintenanceTask.Status
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal server error occurred. See the server logs for more details.");
            }
        }

        /// <summary>
        ///     HTTP PUT: api/MaintenanceTasks/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MaintenanceTaskDto maintenanceTaskDto)
        {
            try
            {
                if (id != maintenanceTaskDto.Id)
                {
                    return BadRequest("ID in the URL doesn't match the ID in the request body.");
                }

                var maintenanceTask = await _maintenanceTaskService.Get(id);

                if (maintenanceTask == null)
                {
                    return NotFound("No maintenance task found with the given ID.");
                }

                // Convert timezone to UTC
                DateTime registrationTimeUtc = maintenanceTaskDto.RegistrationTime.ToUniversalTime();

                if (maintenanceTaskDto.FactoryDeviceId != null)
                {
                    maintenanceTask.FactoryDeviceId = (int)maintenanceTaskDto.FactoryDeviceId;
                }
                maintenanceTask.RegistrationTime = registrationTimeUtc;
                maintenanceTask.Description = maintenanceTaskDto.Description;
                maintenanceTask.Severity = maintenanceTaskDto.Severity;
                maintenanceTask.Status = maintenanceTaskDto.Status;

                MaintenanceTask updatedMaintenanceTask = await _maintenanceTaskService.Update(maintenanceTask);

                return Ok(new MaintenanceTaskDto()
                    {
                        Id = updatedMaintenanceTask.Id,
                        FactoryDevice = updatedMaintenanceTask.FactoryDevice,
                        RegistrationTime = updatedMaintenanceTask.RegistrationTime,
                        Description = updatedMaintenanceTask.Description,
                        Severity = updatedMaintenanceTask.Severity,
                        Status = updatedMaintenanceTask.Status
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal server error occurred. See the server logs for more details.");
            }
        }

        /// <summary>
        ///     HTTP DELETE: api/MaintenanceTasks/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var maintenanceTask = await _maintenanceTaskService.Get(id);

                if (maintenanceTask == null)
                {
                    return NotFound("No maintenance task found with the given ID.");
                }

                await _maintenanceTaskService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal server error occurred. See the server logs for more details.");
            }
        }

        /// <summary>
        ///     HTTP GET: api/MaintenanceTasks/byfactorydevice/{factoryDeviceId}
        /// </summary>
        [HttpGet("byfactorydevice/{factoryDeviceId}")]
        public async Task<IActionResult> GetByDevice(int factoryDeviceId)
        {
            try
            {
                var maintenanceTasks = await _maintenanceTaskService.GetByDevice(factoryDeviceId);

                if (maintenanceTasks == null || !maintenanceTasks.Any())
                {
                    return NotFound("No maintenance tasks found.");
                }

                return Ok(maintenanceTasks.Select(maintenanceTask => new MaintenanceTaskDto()
                    {
                        Id = maintenanceTask.Id,
                        RegistrationTime = maintenanceTask.RegistrationTime,
                        Description = maintenanceTask.Description,
                        Severity = maintenanceTask.Severity,
                        Status = maintenanceTask.Status
                    })
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal server error occurred. See the server logs for more details.");
            }
        }

    }
}
