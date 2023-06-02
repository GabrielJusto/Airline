package com.bonatto.airline.domain.airline.dto;

import com.bonatto.airline.domain.airline.model.Airline;

public record AirlineDetailData(
		Long id,
		String name
		) 
{

	public AirlineDetailData(Airline airline)
	{
		this(airline.getId(), airline.getName());
	}
}
