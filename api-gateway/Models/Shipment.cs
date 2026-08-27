using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class Shipment
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Tracking number is required")]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination is required")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Origin is required")]
        public string Origin { get; set; } = string.Empty;

        public double? Weight { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateDestinationRequest
    {
        [Required]
        public string Destination { get; set; } = string.Empty;
    }

    public class UpdateShipmentRequest
    {
        [Required]
        public string TrackingNumber { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public string Destination { get; set; } = string.Empty;
        [Required]
        public string Origin { get; set; } = string.Empty;
        public double? Weight { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
    }
}
