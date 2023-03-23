package com.bonatto.airline.custumer;

import com.bonatto.airline.address.AdressData;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotNull;

public record CustumerUpdateDto(

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
