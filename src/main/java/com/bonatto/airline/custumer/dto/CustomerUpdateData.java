package com.bonatto.airline.custumer.dto;

import com.bonatto.airline.address.AdressData;
import com.bonatto.airline.custumer.model.Level;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotNull;

public record CustomerUpdateData(

        @NotNull
        Long id,
        String name,
        @Email
        String email,
        Level level,
        @Valid
        EmergencyContactData emergencyContact,
        @Valid
        AdressData address) {
}
