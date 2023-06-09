package com.bonatto.airline.domain.airline.service;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.dto.FlightScheduleDetailData;
import com.bonatto.airline.domain.airline.model.Aircraft;
import com.bonatto.airline.domain.airline.model.Flight;
import com.bonatto.airline.domain.airline.repository.AircraftRepository;
import com.bonatto.airline.domain.airline.repository.FlightRepository;
import com.bonatto.airline.domain.validation.FlightScheduleValidation;
import com.bonatto.airline.infra.error.RegisterException;

@Service
public class FlightScheduleService {

	
	@Autowired
	private AircraftRepository aircraftRepo;
	
	@Autowired
	private FlightRepository flightRepo;
	
	
	@Autowired
	private List<FlightScheduleValidation> validators;
	
	
	public FlightScheduleDetailData schedule(FlightScheduleData data )
	{
		
		if(!aircraftRepo.existsById(data.aircfraftId()))
			throw new RegisterException("Aircraft ID not found");
		
		validators.stream().forEach(v -> v.validate(data));

		
		Aircraft aircraft = aircraftRepo.getReferenceById(data.aircfraftId());
		Flight flight = new Flight(aircraft, data.departure(), data.arrival());
		
		
		flightRepo.save(flight);
		return new FlightScheduleDetailData(flight);
		
	}
}
