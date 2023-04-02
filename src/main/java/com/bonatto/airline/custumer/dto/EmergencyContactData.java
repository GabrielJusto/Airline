package com.bonatto.airline.custumer.dto;

import jakarta.validation.constraints.NotBlank;

public record EmergencyContactData(
        @NotBlank
        String name,
        @NotBlank
        String phone) {
}
