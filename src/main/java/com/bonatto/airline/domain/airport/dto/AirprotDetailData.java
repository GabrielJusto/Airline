package com.bonatto.airline.domain.airport.dto;

import com.bonatto.airline.domain.airport.model.Airport;

public record AirprotDetailData(
		String code,
		String city
		
		) {
	
	
	public AirprotDetailData(Airport data)
	{
		this(data.getCode(), data.getCity());
	}

}
