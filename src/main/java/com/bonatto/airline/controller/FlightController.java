package com.bonatto.airline.controller;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.bonatto.airline.domain.airline.dto.FlighScheduleData;

import jakarta.validation.Valid;

@RestController
@RequestMapping("/flight")
public class FlightController {
	
	
	
	@SuppressWarnings("rawtypes")
	@PutMapping("/schedule")
	public ResponseEntity scheduleFlight(@RequestBody @Valid FlighScheduleData fligh)
	{
		
		return ResponseEntity.ok().build();
	}

}
