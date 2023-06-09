package com.bonatto.airline.controller;


import com.bonatto.airline.domain.airline.dto.AirlineDetailData;
import com.bonatto.airline.domain.airline.dto.AirlineRegisterData;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.util.UriComponentsBuilder;

import java.net.URI;

@RestController
@RequestMapping("airline")
@SecurityRequirement(name = "bearer-key")
public class AirlineController {

    @Autowired
    private AirlineRepository repo;

    
    @SuppressWarnings("rawtypes")
	@PutMapping("/register")
    @Transactional
    public ResponseEntity register(@Valid @RequestBody AirlineRegisterData data, UriComponentsBuilder uriBuilder)
    {
        Airline airline = new Airline(data);
		repo.save(airline);
        
        URI uri = uriBuilder.path("/airline/{id}").buildAndExpand(airline.getId()).toUri();
        return ResponseEntity.created(uri).body(new AirlineDetailData(airline));
        
    }

    @SuppressWarnings("rawtypes")
    @GetMapping("/{id}")
    public ResponseEntity detail( @PathVariable Long id)
    {
    	Airline airline = repo.getReferenceById(id);
    	AirlineDetailData airlineData = new AirlineDetailData(airline);
    	
    	return ResponseEntity.ok(airlineData);
    }
    
    
    @SuppressWarnings("rawtypes")
    @GetMapping("/list")
    public ResponseEntity  list (Pageable pageable)
    {
    	Page<AirlineDetailData> page = repo
                .findAllByActiveTrue(pageable)
                .map( AirlineDetailData:: new);
    	
        return ResponseEntity.ok(page);
    }

    @SuppressWarnings("rawtypes")
    @DeleteMapping("/delete/{id}")
    @Transactional
    public ResponseEntity delete(@PathVariable Long id)
    {
        Airline cus = repo.getReferenceById(id);
        cus.deactivate();
        

        return ResponseEntity.noContent().build();
    }

}
