package com.bonatto.airline.domain.validation;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.model.Flight;
import com.bonatto.airline.domain.airline.repository.FlightRepository;
import com.bonatto.airline.infra.error.RegisterException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.Optional;

@Component
public class FlightScheduleTimeValidator implements FlightScheduleValidation {

	@Autowired
	private FlightRepository flightRepo;
	
	
	@Override
	public void validate(FlightScheduleData data) {
		Optional<Flight> opFlight = flightRepo.findByDate(data.departure(), data.arrival());
		if(opFlight.isPresent())
			throw new RegisterException("This aricraft already has a flight at this time ");
		
	}

}
