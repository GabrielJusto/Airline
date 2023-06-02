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
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.airline.dto.AirlineData;
import com.bonatto.airline.domain.airline.dto.AirlineDetailData;
import com.bonatto.airline.domain.airline.dto.AirlineRegisterData;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;

import jakarta.validation.Valid;

@RestController
@RequestMapping("airline")
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
    	Page<AirlineData> page = repo
                .findAllByActiveTrue(pageable)
                .map( AirlineData:: new);
    	
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
