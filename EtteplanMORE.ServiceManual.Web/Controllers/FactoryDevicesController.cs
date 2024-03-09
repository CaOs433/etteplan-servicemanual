using System;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoryDevicesController : Controller
    {
        private readonly IFactoryDeviceService _factoryDeviceService;

        public FactoryDevicesController(IFactoryDeviceService factoryDeviceService)
        {
            _factoryDeviceService = factoryDeviceService;
        }

        /// <summary>
        ///     HTTP GET: api/factorydevices/
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok((await _factoryDeviceService.GetAll())
                    .Select(device =>
                        new FactoryDeviceDto()
                        {
                            Id = device.Id,
                            Name = device.Name,
                            Year = device.Year,
                            Type = device.Type
                        }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP GET: api/factorydevices/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                FactoryDevice device = await _factoryDeviceService.Get(id);

                if (device == null)
                {
                    return NotFound("No factory device found with the given ID.");
                }

                return Ok(new FactoryDeviceDto()
                    {
                        Id = device.Id,
                        Name = device.Name,
                        Year = device.Year,
                        Type = device.Type
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP POST: api/factorydevices
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FactoryDeviceDto deviceDto)
        {
            try
            {
                FactoryDevice device = new FactoryDevice()
                {
                    Name = deviceDto.Name,
                    Year = deviceDto.Year,
                    Type = deviceDto.Type
                };
                await _factoryDeviceService.Create(device);

                return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP PUT: api/factorydevices/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FactoryDeviceDto deviceDto)
        {
            try
            {
                if (id != deviceDto.Id)
                {
                    return BadRequest("ID in the URL doesn't match the ID in the request body.");
                }

                var device = await _factoryDeviceService.Get(id);

                if (device == null)
                {
                    return NotFound("No factory device found with the given ID.");
                }

                device.Name = deviceDto.Name;
                device.Year = deviceDto.Year;
                device.Type = deviceDto.Type;

                FactoryDevice updatedDevice = await _factoryDeviceService.Update(device);

                return Ok(new FactoryDeviceDto()
                    {
                        Id = updatedDevice.Id,
                        Name = updatedDevice.Name,
                        Year = updatedDevice.Year,
                        Type = updatedDevice.Type
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///     HTTP DELETE: api/factorydevices/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var device = await _factoryDeviceService.Get(id);

                if (device == null)
                {
                    return NotFound("No factory device found with the given ID.");
                }

                await _factoryDeviceService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
