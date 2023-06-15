package com.bonatto.airline.controller;

import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.aircraft.dto.AircraftDetailData;
import com.bonatto.airline.domain.aircraft.dto.AircraftRegisterData;
import com.bonatto.airline.domain.aircraft.dto.AircraftUpdateData;
import com.bonatto.airline.domain.aircraft.model.Aircraft;
import com.bonatto.airline.domain.aircraft.repository.AircraftRepository;
import com.bonatto.airline.domain.aircraft.service.AircraftService;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;

import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;

@RestController
@RequestMapping("/aircraft")
@SecurityRequirement(name = "bearer-key")
public class AircraftController {

    @Autowired
    private AircraftRepository aircraftRepo;

    @Autowired
    private AirlineRepository airlineRepo;
    
    @Autowired
    private AircraftService aircraftService;
    

    @SuppressWarnings("rawtypes")
	@PutMapping("/register")
    @Transactional
    public ResponseEntity registerAircraft(@Valid @RequestBody AircraftRegisterData data, UriComponentsBuilder uriBuilder)
    {
    	AircraftDetailData aircraftData = aircraftService.register(data);

		URI uri = uriBuilder.path("/aircraft/{id}").buildAndExpand(aircraftData.id()).toUri();
        return ResponseEntity.created(uri).body(aircraftData);
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
    @SuppressWarnings("rawtypes")
    @DeleteMapping("/{id}")
    public ResponseEntity delete(@PathVariable Long id)
    {
        aircraftRepo.deleteById(id);

        return ResponseEntity.noContent().build();
    }

    @SuppressWarnings("rawtypes")
    @PostMapping("/update")
    @Transactional
    public ResponseEntity update (@RequestBody @Valid AircraftUpdateData data)
    {
        Aircraft aircraft = aircraftRepo.getReferenceById(data.id());
        aircraft.update(data, airlineRepo);

       return ResponseEntity.ok(new AircraftDetailData(aircraft));
    }


}
















