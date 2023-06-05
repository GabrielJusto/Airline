package com.bonatto.airline.controller;

import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.airline.dto.AircraftDetailData;
import com.bonatto.airline.domain.airline.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.model.Aircraft;
import com.bonatto.airline.domain.airline.repository.AircraftRepository;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;

import jakarta.validation.Valid;

@RestController
@RequestMapping("/aircraft")
public class AircraftController {

    @Autowired
    private AircraftRepository aircraftRepo;

    @Autowired
    private AirlineRepository airlineRepository;

    @SuppressWarnings("rawtypes")
	@PutMapping("/register")
    @Transactional
    public ResponseEntity registerAircraft(@Valid @RequestBody AircraftRegisterData data, UriComponentsBuilder uriBuilder)
    {
        Aircraft aircraft = new Aircraft(data, airlineRepository);
		aircraftRepo.save(aircraft);
		
		URI uri = uriBuilder.path("/aircraft/{id}").buildAndExpand(aircraft.getId()).toUri();
        return ResponseEntity.created(uri).body(new AircraftDetailData(aircraft));
    }
    
    @SuppressWarnings("rawtypes")
    @GetMapping("/{id}")
    public ResponseEntity detail( @PathVariable Long id)
    {
    	Aircraft aircraft = aircraftRepo.getReferenceById(id);
    	AircraftDetailData aircraftDetailData = new AircraftDetailData(aircraft);
    	
    	return ResponseEntity.ok(aircraftDetailData);
    }
    
    @SuppressWarnings("rawtypes")
    @GetMapping("/list")
    public ResponseEntity list(Pageable pageable)
    {
    	Page<AircraftDetailData> page = 
    			aircraftRepo.findAll(pageable)
    			.map(AircraftDetailData :: new);
    	
    	return ResponseEntity.ok(page);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity delete(@PathVariable Long id)
    {
        aircraftRepo.deleteById(id);

        return ResponseEntity.noContent().build();
    }
}
















