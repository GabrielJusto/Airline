package com.bonatto.airline.controller;


import com.bonatto.airline.airline.dto.AircraftRegisterData;
import com.bonatto.airline.airline.model.Aircraft;
import com.bonatto.airline.airline.model.Airline;
import com.bonatto.airline.airline.dto.AirlineRegisterData;
import com.bonatto.airline.airline.repository.AircraftRepository;
import com.bonatto.airline.airline.repository.AirlineRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("airline")
public class AirlineController {

    @Autowired
    private AirlineRepository repo;

    @Autowired
    private AircraftRepository aircraftRepo;

    @PostMapping("register")
    @Transactional
    public void register(@Valid @RequestBody AirlineRegisterData data)
    {
        repo.save(new Airline(data));
    }

    @PostMapping("register-aircraft")
    @Transactional
    public void registerAircraft(@Valid @RequestBody AircraftRegisterData data)
    {
        aircraftRepo.save(new Aircraft(data, repo));
    }


}
