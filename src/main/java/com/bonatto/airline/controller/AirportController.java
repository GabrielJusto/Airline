package com.bonatto.airline.controller;

import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.airport.dto.AirportDetailData;
import com.bonatto.airline.domain.airport.dto.AirportRegisterData;
import com.bonatto.airline.domain.airport.model.Airport;
import com.bonatto.airline.domain.airport.repository.AirportRepository;

import jakarta.transaction.Transactional;
import jakarta.validation.Valid;

@RestController
@RequestMapping("/airport")
public class AirportController {

	@Autowired
	private AirportRepository airportRepo;
	
	@PutMapping("/register")
	@Transactional
	public ResponseEntity<AirportDetailData> register(@RequestBody @Valid AirportRegisterData data, UriComponentsBuilder uriBuilder)
	{
		Airport airport = new Airport(data);
		
		airportRepo.save(airport);
		
		URI uri = uriBuilder.path("/airport/{id}").buildAndExpand(airport.getId()).toUri();
        return ResponseEntity.created(uri).body(new AirportDetailData(airport));
		
	}
	
	
	@GetMapping("/detail/{id}")
	public ResponseEntity<AirportDetailData> detail (@PathVariable Long id)
	{
		Airport airport = airportRepo.getReferenceById(id);
		
		AirportDetailData airportDetail = new AirportDetailData(airport);
		
		return ResponseEntity.ok(airportDetail);
	}
	
    
    @GetMapping("/list")
    public ResponseEntity<Page<AirportDetailData>> list (Pageable pageable)
    {
        Page<AirportDetailData> page = airportRepo
                .findAll(pageable)
                .map( AirportDetailData:: new);

        return ResponseEntity.ok(page);

    }
    
    
    @SuppressWarnings("rawtypes")
    @DeleteMapping("/delete/{id}")
    @Transactional
    public ResponseEntity deleteCustomer(@PathVariable Long id)
    {
        airportRepo.deleteById(id);

        return ResponseEntity.noContent().build();
    }
	
}
