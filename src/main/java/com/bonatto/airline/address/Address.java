package com.bonatto.airline.address;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Entity(name = "Address")
@Table(name = "address")
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode( of = "id")
public class Address {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String address;
    private String district;
    private String zipCode;
    private String city;

    private String State;
    private String number;
    private String complement;

    public Address(AdressData address) {
        this.address = address.address();
        this.district = address.district();
        this.zipCode = address.zipCode();
        this.city = address.city();
        this.State = address.state();
        this.number = address.number();
        this.complement = address.complement();
    }
}

