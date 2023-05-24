package com.bonatto.airline.controller;


import com.bonatto.airline.domain.airline.dto.AirlineData;
import com.bonatto.airline.domain.airline.dto.AirlineRegisterData;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("airline")
public class AirlineController {

    @Autowired
    private AirlineRepository repo;

    @PostMapping("register")
    @Transactional
    public void register(@Valid @RequestBody AirlineRegisterData data)
    {
        repo.save(new Airline(data));
    }

    @GetMapping("/list")
    public Page<AirlineData> list (Pageable pageable)
    {
        return repo
                .findAllByActiveTrue(pageable)
                .map( AirlineData:: new);
    }

    @DeleteMapping("/delete/{id}")
    @Transactional
    public void delete(@PathVariable Long id)
    {
        Airline cus = repo.getReferenceById(id);
        cus.deactivate();
    }

}
