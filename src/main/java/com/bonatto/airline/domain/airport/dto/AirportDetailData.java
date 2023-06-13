package com.bonatto.airline.domain.airport.dto;

import com.bonatto.airline.domain.airport.model.Airport;

public record AirportDetailData(
		Long id,
		String code,
		String city
		
		) {
	
	
	public AirportDetailData(Airport data)
	{
		this(data.getId(), data.getCode(), data.getCity());
	}

}
