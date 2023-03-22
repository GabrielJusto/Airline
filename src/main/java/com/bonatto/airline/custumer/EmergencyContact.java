package com.bonatto.airline.custumer;

import jakarta.persistence.Embeddable;
import lombok.AllArgsConstructor;
import lombok.NoArgsConstructor;

@Embeddable
@NoArgsConstructor
@AllArgsConstructor
public class EmergencyContact {

    private String emergencyName;
    private String phone;

    public EmergencyContact(EmergencyContactData emergencyContact) {
        this.emergencyName = emergencyContact.name();
        this.phone = emergencyContact.phone();
    }
}
