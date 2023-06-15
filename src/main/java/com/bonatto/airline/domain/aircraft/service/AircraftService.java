package com.bonatto.airline.domain.aircraft.service;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.bonatto.airline.domain.aircraft.dto.AircraftDetailData;
import com.bonatto.airline.domain.aircraft.dto.AircraftRegisterData;
import com.bonatto.airline.domain.aircraft.dto.SeatRegisterData;
import com.bonatto.airline.domain.aircraft.model.Aircraft;
import com.bonatto.airline.domain.aircraft.model.Seat;
import com.bonatto.airline.domain.aircraft.repository.AircraftRepository;
import com.bonatto.airline.domain.aircraft.repository.SeatRepository;
import com.bonatto.airline.domain.aircraft.validation.AircraftRegisterValidation;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;

import lombok.AllArgsConstructor;

@Service
@AllArgsConstructor
public class AircraftService {

	
	
	@Autowired
	private AircraftRepository aircraftRepo;
	
	@Autowired
	private SeatRepository seatRepo;
	
	@Autowired
	private AirlineRepository airlineRepo;
	
	@Autowired
	private List<AircraftRegisterValidation> validators;
	
	public AircraftDetailData register(AircraftRegisterData data)
	{
		
		validators.stream().forEach(v -> v.validate(data));
		
		Airline airline = airlineRepo.findById(data.airlineId()).get();
		Aircraft aircraft = new Aircraft(data, airline);
		
		aircraftRepo.save(aircraft);
		
		registerSeats(data.seats(), aircraft);
		
		
		return new AircraftDetailData(aircraft);
	}
	
	
	private void registerSeats(List<SeatRegisterData> data, Aircraft aircraft)
	{
		
		seatRepo.saveAll(
				data.stream()
				.map(s -> new Seat(s, aircraft))
				.collect(Collectors.toList()));
	}
}

















