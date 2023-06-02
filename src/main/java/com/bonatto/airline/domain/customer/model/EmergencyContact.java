package com.bonatto.airline.domain.customer.model;

import com.bonatto.airline.domain.customer.dto.EmergencyContactData;
import jakarta.persistence.Embeddable;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Embeddable
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class EmergencyContact {

    private String emergencyName;
    private String phone;

    public EmergencyContact(EmergencyContactData emergencyContact) {
        this.emergencyName = emergencyContact.name();
        this.phone = emergencyContact.phone();
    }

    public void update(EmergencyContactData data) {
        if(data.name() != null)
            this.emergencyName = data.name();

        if(data.phone() != null)
            this.phone = data.phone();
    }
}
