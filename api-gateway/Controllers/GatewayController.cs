using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/gateway")]
    public class GatewayController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly ILogger<GatewayController> _logger;

        public GatewayController(IShipmentService shipmentService, ILogger<GatewayController> logger)
        {
            _shipmentService = shipmentService;
            _logger = logger;
        }

        [HttpGet("shipments")]
        public async Task<IActionResult> GetAllShipments()
        {
            _logger.LogInformation("Gateway receiving GET all shipments request");
            var result = await _shipmentService.GetAllShipmentsAsync();
            return Ok(result);
        }

        [HttpGet("shipments/{id}")]
        public async Task<IActionResult> GetShipmentById(long id)
        {
            _logger.LogInformation("Gateway receiving GET shipment {Id}", id);
            var result = await _shipmentService.GetShipmentByIdAsync(id);
            if (result == null) return NotFound(new { message = "Shipment not found in core service" });
            return Ok(result);
        }

        [HttpPost("shipments")]
        public async Task<IActionResult> CreateShipment([FromBody] Shipment shipment)
        {
            _logger.LogInformation("Gateway receiving POST shipment request");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _shipmentService.CreateShipmentAsync(shipment);
                return Created($"/api/gateway/shipments/{created?.Id}", created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating shipment in core service");
                return StatusCode(500, new { message = "Error communicating with core service" });
            }
        }

        [HttpPut("shipments/{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateStatusRequest request)
        {
            _logger.LogInformation("Gateway receiving PUT shipment status request");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _shipmentService.UpdateShipmentStatusAsync(id, request.Status);
            if (updated == null) return NotFound(new { message = "Shipment not found in core service" });

            return Ok(updated);
        }

        [HttpPut("shipments/{id}/destination")]
        public async Task<IActionResult> UpdateDestination(long id, [FromBody] UpdateDestinationRequest request)
        {
            _logger.LogInformation("Gateway receiving PUT shipment destination request");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _shipmentService.UpdateShipmentDestinationAsync(id, request.Destination);
            if (updated == null) return NotFound(new { message = "Shipment not found in core service" });

            return Ok(updated);
        }

        [HttpPut("shipments/{id}")]
        public async Task<IActionResult> UpdateShipment(long id, [FromBody] UpdateShipmentRequest request)
        {
            _logger.LogInformation("Gateway receiving full PUT shipment request");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _shipmentService.UpdateShipmentAsync(id, request);
            if (updated == null) return NotFound(new { message = "Shipment not found in core service" });

            return Ok(updated);
        }

        [HttpPatch("shipments/{id}")]
        public async Task<IActionResult> PatchShipment(long id, [FromBody] System.Text.Json.JsonElement patch)
        {
            _logger.LogInformation("Gateway receiving PATCH shipment request");

            var updated = await _shipmentService.PatchShipmentAsync(id, patch);
            if (updated == null) return NotFound(new { message = "Shipment not found in core service" });

            return Ok(updated);
        }

        [HttpDelete("shipments/{id}")]
        public async Task<IActionResult> DeleteShipment(long id)
        {
            _logger.LogInformation("Gateway receiving DELETE shipment request");
            var success = await _shipmentService.DeleteShipmentAsync(id);
            if (!success) return NotFound(new { message = "Shipment not found or could not be deleted" });

            return NoContent();
        }
    }
}
