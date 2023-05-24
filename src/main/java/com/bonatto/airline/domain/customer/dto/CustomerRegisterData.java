package com.bonatto.airline.domain.customer.dto;

import com.bonatto.airline.domain.address.AdressData;
import com.bonatto.airline.domain.customer.model.Level;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

public record CustomerRegisterData(
        @NotBlank
        String name,
        @NotBlank
        @Email
        String email,
        @NotNull
        Level level,
        @NotNull
        @Valid
        EmergencyContactData emergencyContact,
        @NotNull
        @Valid
        AdressData address
) {


}
