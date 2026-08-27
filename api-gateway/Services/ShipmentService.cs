using System.Text.Json;
using ApiGateway.Models;

namespace ApiGateway.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShipmentService> _logger;

        public ShipmentService(HttpClient httpClient, ILogger<ShipmentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Shipment>?> GetAllShipmentsAsync()
        {
            _logger.LogInformation("Proxying request to get all shipments");
            return await _httpClient.GetFromJsonAsync<IEnumerable<Shipment>>("shipments");
        }

        public async Task<Shipment?> GetShipmentByIdAsync(long id)
        {
            _logger.LogInformation("Proxying request to get shipment {Id}", id);
            var response = await _httpClient.GetAsync($"shipments/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Shipment>();
            }
            return null;
        }

        public async Task<Shipment?> GetShipmentByTrackingAsync(string trackingNumber)
        {
            _logger.LogInformation("Proxying request to get shipment by tracking {Tracking}", trackingNumber);
            var response = await _httpClient.GetAsync($"shipments/tracking/{trackingNumber}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Shipment>();
            }
            return null;
        }

        public async Task<Shipment?> CreateShipmentAsync(Shipment shipment)
        {
            _logger.LogInformation("Proxying request to create shipment");
            var response = await _httpClient.PostAsJsonAsync("shipments", shipment);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Shipment>();
        }

        public async Task<Shipment?> UpdateShipmentStatusAsync(long id, string status)
        {
            _logger.LogInformation("Proxying request to update shipment {Id} status", id);
            var payload = new { status = status };
            var response = await _httpClient.PutAsJsonAsync($"shipments/{id}/status", payload);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Shipment>();
            }
            return null;
        }

        public async Task<Shipment?> UpdateShipmentDestinationAsync(long id, string destination)
        {
            _logger.LogInformation("Proxying request to update shipment {Id} destination", id);
            var payload = new { destination = destination };
            var response = await _httpClient.PutAsJsonAsync($"shipments/{id}/destination", payload);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Shipment>();
            }
            return null;
        }

        public async Task<Shipment?> UpdateShipmentAsync(long id, UpdateShipmentRequest request)
        {
            _logger.LogInformation("Proxying request to fully update shipment {Id}", id);
            var response = await _httpClient.PutAsJsonAsync($"shipments/{id}", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Shipment>();
            }
            return null;
        }

        public async Task<Shipment?> PatchShipmentAsync(long id, JsonElement patch)
        {
            _logger.LogInformation("Proxying request to patch shipment {Id}", id);
            var jsonPayload = JsonSerializer.Serialize(patch);
            var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"shipments/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Shipment>();
            }
            return null;
        }

        public async Task<bool> DeleteShipmentAsync(long id)
        {
            _logger.LogInformation("Proxying request to delete shipment {Id}", id);
            var response = await _httpClient.DeleteAsync($"shipments/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
