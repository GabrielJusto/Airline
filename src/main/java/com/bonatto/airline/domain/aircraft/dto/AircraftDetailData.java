package com.bonatto.airline.domain.aircraft.dto;

import com.bonatto.airline.domain.aircraft.model.Aircraft;

public record AircraftDetailData(
		Long id,
		Long airlineId,
		String model,
		int passengerCapacity
		) 
{

	public AircraftDetailData(Aircraft aircraft)
	{
		this(aircraft.getId(), aircraft.getAirline().getId(), aircraft.getModel(), aircraft.getPassengerCapacity());
	}
}
