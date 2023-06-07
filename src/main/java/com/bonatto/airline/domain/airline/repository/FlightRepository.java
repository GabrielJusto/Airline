package com.bonatto.airline.domain.airline.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.bonatto.airline.domain.airline.model.Flight;

public interface FlightRepository extends JpaRepository<Flight, Long> {

}
