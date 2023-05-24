package com.bonatto.airline.domain.customer.dto;

import jakarta.validation.constraints.NotBlank;

public record EmergencyContactData(
        @NotBlank
        String name,
        @NotBlank
        String phone) {
}
