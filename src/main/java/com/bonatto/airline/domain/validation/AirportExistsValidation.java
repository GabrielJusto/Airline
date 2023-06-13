package com.bonatto.airline.domain.validation;

import org.springframework.beans.factory.annotation.Autowired;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airport.repository.AirportRepository;
import com.bonatto.airline.infra.error.RegisterException;

public class AirportExistsValidation implements FlightScheduleValidation {

	@Autowired
	private AirportRepository airportRepo;
	
	@Override
	public void validate(FlightScheduleData data) {
		
		if(!airportRepo.existsByCode(data.sourceCode()))
			throw new RegisterException("Source airport does not exists");
		

		if(!airportRepo.existsByCode(data.destinatoinCode()))
			throw new RegisterException("Destination airport does not exists");
	}

}
