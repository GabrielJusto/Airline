package com.bonatto.airline.domain.validation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airport.repository.AirportRepository;
import com.bonatto.airline.infra.error.RegisterException;

import lombok.AllArgsConstructor;

@Component
@AllArgsConstructor
public class AirportExistsValidation implements FlightScheduleValidation {

	@Autowired
	private AirportRepository airportRepo;
	
	@Override
	public void validate(FlightScheduleData data) {
		
		if(!airportRepo.existsByCode(data.sourceCode()))
			throw new RegisterException("Source airport does not exists");
		

		if(!airportRepo.existsByCode(data.destinationCode()))
			throw new RegisterException("Destination airport does not exists");
	}

}
