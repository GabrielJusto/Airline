package com.bonatto.airline.domain.airport.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.bonatto.airline.domain.airport.model.Airport;

public interface AirportRepository extends JpaRepository<Airport, Long>{

}
