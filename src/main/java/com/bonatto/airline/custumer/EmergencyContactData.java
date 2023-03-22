package com.bonatto.airline.custumer;

import jakarta.validation.constraints.NotBlank;

public record EmergencyContactData(
        @NotBlank
        String name,
        @NotBlank
        String phone) {
}
