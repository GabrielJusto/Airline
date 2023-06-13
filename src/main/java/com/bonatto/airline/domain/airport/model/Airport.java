package com.bonatto.airline.domain.airport.model;

import java.math.BigDecimal;
import java.util.List;

import com.bonatto.airline.domain.airline.model.Flight;
import com.bonatto.airline.domain.airport.dto.AirportRegisterData;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity(name = "Airport")
@Table(name = "airport")
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
	
	@OneToMany(mappedBy = "source")
	private List<Flight> sources;
	
	@OneToMany(mappedBy = "destination")
	private List<Flight> detinations;
	
	
	
	public Airport(AirportRegisterData data)
	{
		this.latitude = data.latitude();
		this.longitude = data.longitude();
		this.code = data.code();
		this.city = data.city();
		this.country = data.country();
	}
}













