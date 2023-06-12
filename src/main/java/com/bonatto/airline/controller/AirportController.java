package com.bonatto.airline.controller;

import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.airport.dto.AirportRegisterData;
import com.bonatto.airline.domain.airport.dto.AirprotDetailData;
import com.bonatto.airline.domain.airport.model.Airport;
import com.bonatto.airline.domain.airport.repository.AirportRepository;

import jakarta.transaction.Transactional;

@RestController
@RequestMapping("/airport")
public class AirportController {

	@Autowired
	private AirportRepository airportRepo;
	
	@SuppressWarnings("rawtypes")
	@PutMapping
	@Transactional
	public ResponseEntity register(AirportRegisterData data, UriComponentsBuilder uriBuilder)
	{
		Airport airport = new Airport(data);
		
		airportRepo.save(airport);
		
		URI uri = uriBuilder.path("/airport/{id}").buildAndExpand(airport.getId()).toUri();
        return ResponseEntity.created(uri).body(new AirprotDetailData(airport));
		
	}
}
