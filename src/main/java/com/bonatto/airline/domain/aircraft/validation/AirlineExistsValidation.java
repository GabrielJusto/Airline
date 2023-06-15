package com.bonatto.airline.domain.aircraft.validation;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.bonatto.airline.domain.aircraft.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;
import com.bonatto.airline.infra.error.RegisterException;

@Component
public class AirlineExistsValidation implements AircraftRegisterValidation {

	
	@Autowired
	private AirlineRepository airlineRepo;
	
	@Override
	public void validate(AircraftRegisterData data) {
		
		if(!airlineRepo.existsById(data.airlineId()))
			throw new RegisterException("This Airline doesn't exists!");
	}

}
