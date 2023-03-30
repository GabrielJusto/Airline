package com.bonatto.airline.controller;


import com.bonatto.airline.airline.Airline;
import com.bonatto.airline.airline.AirlineRegisterDto;
import com.bonatto.airline.airline.AirlineRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("airline")
public class AirlineController {

    @Autowired
    private AirlineRepository repo;

    @PostMapping("register")
    @Transactional
    public void register(@RequestBody AirlineRegisterDto data)
    {
        repo.save(new Airline(data));
    }

}
