package com.bonatto.airline.custumer;


import com.bonatto.airline.address.Address;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Table(name = "custumer")
@Entity(name = "Custimer")
@Getter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode( of = "id")
public class Custumer {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String name;
    private String email;

    @Enumerated(EnumType.STRING)
    private Level level;
    @Embedded
    private EmergencyContact emergencyContact;


    @OneToOne( cascade=CascadeType.PERSIST)
    private Address address;

    public Custumer(CustumerRegisterData data) {
        this.name = data.name();
        this.email = data.email();
        this.level = data.level();
        this.emergencyContact = new EmergencyContact(data.emergencyContact());
        this.address = new Address(data.address());

    }
}
