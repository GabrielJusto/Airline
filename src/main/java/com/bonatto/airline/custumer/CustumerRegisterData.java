package com.bonatto.airline.custumer;

import com.bonatto.airline.address.AdressData;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

public record CustumerRegisterData(
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
