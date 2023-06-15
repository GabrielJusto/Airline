package com.bonatto.airline.domain.aircraft.model;

import com.bonatto.airline.domain.aircraft.dto.SeatRegisterData;

import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Entity(name = "Seat")
@Table(name = "seat")
@AllArgsConstructor
@NoArgsConstructor
@Getter
@EqualsAndHashCode(of = "id")
public class Seat {

	@Id @GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;
	
	private String place;
	
	@Enumerated(EnumType.ORDINAL)
	private SeatClass seatClass;
	
	@ManyToOne
	private Aircraft aircraft;
	
	public Seat(SeatRegisterData data, Aircraft aircraft)
	{
		this.place = data.place();
		this.seatClass = data.seatClass();
		this.aircraft = aircraft;
	}
}

















