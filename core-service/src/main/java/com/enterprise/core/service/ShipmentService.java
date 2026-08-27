package com.enterprise.core.service;

import com.enterprise.core.entity.Shipment;
import com.enterprise.core.repository.ShipmentRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.springframework.util.ReflectionUtils;
import java.lang.reflect.Field;

@Service
public class ShipmentService {

    private static final Logger logger = LoggerFactory.getLogger(ShipmentService.class);

    private final ShipmentRepository shipmentRepository;

    @Autowired
    public ShipmentService(ShipmentRepository shipmentRepository) {
        this.shipmentRepository = shipmentRepository;
    }

    public List<Shipment> getAllShipments() {
        logger.info("Fetching all shipments");
        return shipmentRepository.findAll();
    }

    public Optional<Shipment> getShipmentById(Long id) {
        logger.info("Fetching shipment with ID: {}", id);
        return shipmentRepository.findById(id);
    }

    public Optional<Shipment> getShipmentByTrackingNumber(String trackingNumber) {
        logger.info("Fetching shipment with tracking number: {}", trackingNumber);
        return shipmentRepository.findByTrackingNumber(trackingNumber);
    }

    @Transactional
    public Shipment createShipment(Shipment shipment) {
        logger.info("Creating new shipment with tracking number: {}", shipment.getTrackingNumber());
        return shipmentRepository.save(shipment);
    }

    @Transactional
    public Optional<Shipment> updateShipmentStatus(Long id, String status) {
        logger.info("Updating status for shipment ID: {} to {}", id, status);
        return shipmentRepository.findById(id).map(existingShipment -> {
            existingShipment.setStatus(status);
            return shipmentRepository.save(existingShipment);
        });
    }

    @Transactional
    public Optional<Shipment> updateShipmentDestination(Long id, String destination) {
        logger.info("Updating destination for shipment ID: {} to {}", id, destination);
        return shipmentRepository.findById(id).map(existingShipment -> {
            existingShipment.setDestination(destination);
            return shipmentRepository.save(existingShipment);
        });
    }

    @Transactional
    public Optional<Shipment> updateShipment(Long id, Shipment updatedShipment) {
        logger.info("Full update for shipment ID: {}", id);
        return shipmentRepository.findById(id).map(existingShipment -> {
            existingShipment.setTrackingNumber(updatedShipment.getTrackingNumber());
            existingShipment.setStatus(updatedShipment.getStatus());
            existingShipment.setDestination(updatedShipment.getDestination());
            existingShipment.setOrigin(updatedShipment.getOrigin());
            existingShipment.setWeight(updatedShipment.getWeight());
            existingShipment.setExpectedDeliveryDate(updatedShipment.getExpectedDeliveryDate());
            return shipmentRepository.save(existingShipment);
        });
    }

    @Transactional
    public Optional<Shipment> patchShipment(Long id, Map<String, Object> fields) {
        logger.info("Patching shipment ID: {} with fields: {}", id, fields.keySet());
        return shipmentRepository.findById(id).map(existingShipment -> {
            fields.forEach((key, value) -> {
                Field field = ReflectionUtils.findField(Shipment.class, key);
                if (field != null && !key.equals("id") && !key.equals("createdAt") && !key.equals("updatedAt")) {
                    field.setAccessible(true);
                    // Special handling for Double if it comes in as Integer/String from JSON, but simple reflection is fine for POC
                    ReflectionUtils.setField(field, existingShipment, value);
                }
            });
            return shipmentRepository.save(existingShipment);
        });
    }

    @Transactional
    public void deleteShipment(Long id) {
        logger.info("Deleting shipment with ID: {}", id);
        shipmentRepository.deleteById(id);
    }
}
