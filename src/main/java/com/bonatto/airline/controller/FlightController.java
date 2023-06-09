package com.bonatto.airline.controller;

import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.dto.FlightScheduleDetailData;
import com.bonatto.airline.domain.airline.service.FlightScheduleService;

import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;

@RestController
@RequestMapping("/flight")
@SecurityRequirement(name = "bearer-key")
public class FlightController {
	
	@Autowired
	private FlightScheduleService flightService;
	
	@SuppressWarnings("rawtypes")
	@PutMapping("/schedule")
	public ResponseEntity scheduleFlight(@RequestBody @Valid FlightScheduleData fligh, UriComponentsBuilder uriBuilder)
	{
		
		FlightScheduleDetailData flightData = flightService.schedule(fligh);
		
		
		URI uri = uriBuilder.path("/flight/{id}").buildAndExpand(flightData.id()).toUri();
        return ResponseEntity.created(uri).body(flightData);
        
	}

}
