using System.Text.Json;
using ApiGateway.Models;

namespace ApiGateway.Services
{
    public interface IShipmentService
    {
        Task<IEnumerable<Shipment>?> GetAllShipmentsAsync();
        Task<Shipment?> GetShipmentByIdAsync(long id);
        Task<Shipment?> GetShipmentByTrackingAsync(string trackingNumber);
        Task<Shipment?> CreateShipmentAsync(Shipment shipment);
        Task<Shipment?> UpdateShipmentStatusAsync(long id, string status);
        Task<Shipment?> UpdateShipmentDestinationAsync(long id, string destination);
        Task<Shipment?> UpdateShipmentAsync(long id, UpdateShipmentRequest request);
        Task<Shipment?> PatchShipmentAsync(long id, JsonElement patch);
        Task<bool> DeleteShipmentAsync(long id);
    }
}
