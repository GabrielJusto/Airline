package com.bonatto.airline.domain.airline.model;

import java.time.LocalDateTime;

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

	public Flight(Aircraft aircraft, LocalDateTime departure, LocalDateTime arrival) {
		super();
		this.aircraft = aircraft;
		this.departure = departure;
		this.arrival = arrival;
	}
	
	
}
