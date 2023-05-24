package com.bonatto.airline.domain.customer.model;


import com.bonatto.airline.domain.address.Address;
import com.bonatto.airline.domain.customer.dto.CustomerRegisterData;
import com.bonatto.airline.domain.customer.dto.CustomerUpdateData;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Table(name = "customer")
@Entity(name = "Customer")
@Getter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode( of = "id")
public class Customer {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private boolean active;
    private String name;
    private String email;
    @Enumerated(EnumType.STRING)
    private Level level;
    @Embedded
    private EmergencyContact emergencyContact;
    @OneToOne( cascade=CascadeType.PERSIST)
    private Address address;

    public Customer(CustomerRegisterData data) {
        this.active = true;
        this.name = data.name();
        this.email = data.email();
        this.level = data.level();
        this.emergencyContact = new EmergencyContact(data.emergencyContact());
        this.address = new Address(data.address());

    }

    public void update(CustomerUpdateData data) {
        if(data.name() != null)
            this.name = data.name();

        if(data.email() != null)
            this.email = data.email();

        if(data.level() != null)
            this.level = data.level();

        if(data.emergencyContact() != null)
            this.emergencyContact.update(data.emergencyContact());

        if(data.address() != null)
            this.address.update(data.address());


    }

    public void deactivate() {
        this.active = false;
    }
}
