package com.bonatto.airline.domain.address;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Entity(name = "Address")
@Table(name = "address")
@NoArgsConstructor
@AllArgsConstructor
@Getter
@EqualsAndHashCode( of = "id")
public class Address {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String address;
    private String district;
    private String zipCode;
    private String city;

    private String state;
    private String number;
    private String complement;

    public Address(AdressData address) {
        this.address = address.address();
        this.district = address.district();
        this.zipCode = address.zipCode();
        this.city = address.city();
        this.state = address.state();
        this.number = address.number();
        this.complement = address.complement();
    }

    public void update(AdressData data) {
        if(data.address() != null)
            this.address = data.address();

        if(data.district() != null)
            this.district = data.district();

        if(data.zipCode() != null)
            this.zipCode = data.zipCode();

        if(data.city() != null)
            this.city = data.city();

        if(data.state() != null)
            this.state = data.state();

        if(data.number() != null)
            this.number = data.number();

        if(data.complement() != null)
            this.complement = data.complement();
    }
}

