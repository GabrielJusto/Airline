package com.bonatto.airline.controller;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.dto.FlightScheduleDetailData;
import com.bonatto.airline.domain.airline.repository.FlightRepository;
import com.bonatto.airline.domain.airline.service.FlightScheduleService;
import com.bonatto.airline.domain.customer.dto.CustomerData;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.util.UriComponentsBuilder;

import java.net.URI;

@RestController
@RequestMapping("/flight")
@SecurityRequirement(name = "bearer-key")
public class FlightController {
	
	@Autowired
	private FlightScheduleService flightService;

	@Autowired
	private FlightRepository flightRepo;
	

	@PutMapping("/schedule")
	public ResponseEntity<FlightScheduleDetailData> scheduleFlight(@RequestBody @Valid FlightScheduleData fligh, UriComponentsBuilder uriBuilder)
	{
		
		FlightScheduleDetailData flightData = flightService.schedule(fligh);
		
		
		URI uri = uriBuilder.path("/flight/{id}").buildAndExpand(flightData.id()).toUri();
        return ResponseEntity.created(uri).body(flightData);
        
	}

	@GetMapping("/list")
	public ResponseEntity<Page<FlightScheduleDetailData>> list(Pageable pageable)
	{
		Page<FlightScheduleDetailData> page =
				flightRepo.findAll(pageable)
				.map(FlightScheduleDetailData :: new);
		return ResponseEntity.ok(page);

	}

}
