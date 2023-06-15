package com.bonatto.airline.domain.airline.model;

import java.time.LocalDateTime;

import com.bonatto.airline.domain.aircraft.model.Aircraft;
import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airport.model.Airport;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity (name = "Flight")
@Table (name= "flight")
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
public class Flight {

	@Id @GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;
	
	@ManyToOne()
	private Aircraft aircraft;
	
	private LocalDateTime departure;
	
	private LocalDateTime arrival;
	
	@ManyToOne()
	private Airport source;
	
	@ManyToOne()
	private Airport destination;

	public Flight(Aircraft aircraft, Airport source, Airport destination, FlightScheduleData data) {
		super();
		this.aircraft = aircraft;
		this.departure = data.departure();
		this.arrival = data.arrival();
		this.source = source;
		this.destination = destination;
		
	}

	public Flight(Aircraft aircraft, LocalDateTime departure, LocalDateTime arrival, Airport source,
			Airport destination) {
		super();
		this.aircraft = aircraft;
		this.departure = departure;
		this.arrival = arrival;
		this.source = source;
		this.destination = destination;
	}


	
	
}
