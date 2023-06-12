package com.bonatto.airline.domain.airport.model;

import java.math.BigDecimal;

import com.bonatto.airline.domain.airport.dto.AirportRegisterData;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@EqualsAndHashCode(of = "id")
public class Airport {

	
	@Id @GeneratedValue(strategy = GenerationType.IDENTITY	)
	private Long id;
	private BigDecimal latitude;
	private BigDecimal longitude;
	private String code;
	private String city;
	private String country;
	
	
	public Airport(AirportRegisterData data)
	{
		this.latitude = data.latitude();
		this.longitude = data.longitude();
		this.code = data.code();
		this.city = data.city();
		this.country = data.country();
	}
}













