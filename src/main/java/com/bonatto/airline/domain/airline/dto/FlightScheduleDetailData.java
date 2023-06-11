package com.bonatto.airline.domain.airline.dto;

import com.bonatto.airline.domain.airline.model.Flight;

import java.time.LocalDateTime;

public record FlightScheduleDetailData(
		
		Long id,
		AircraftDetailData aircraft,
		LocalDateTime departure,
		LocalDateTime arraival
		
		) {

	public FlightScheduleDetailData(Flight data)
	{
		this(data.getId(), new AircraftDetailData(data.getAircraft()), data.getDeparture(), data.getArrival());
	}
}
