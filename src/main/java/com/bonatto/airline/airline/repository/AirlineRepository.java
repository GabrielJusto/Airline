package com.bonatto.airline.airline.repository;

import com.bonatto.airline.airline.model.Airline;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AirlineRepository extends JpaRepository<Airline, Long> {
}
