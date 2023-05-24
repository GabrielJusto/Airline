package com.bonatto.airline.controller;

import com.bonatto.airline.domain.airline.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.model.Aircraft;
import com.bonatto.airline.domain.airline.repository.AircraftRepository;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("aircraft")
public class AircraftController {

    @Autowired
    private AircraftRepository repo;

    @Autowired
    private AirlineRepository airlineRepository;

    @PostMapping("register")
    @Transactional
    public void registerAircraft(@Valid @RequestBody AircraftRegisterData data)
    {
        repo.save(new Aircraft(data, airlineRepository));
    }
}
