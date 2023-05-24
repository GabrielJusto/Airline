package com.bonatto.airline.domain.airline.repository;

import com.bonatto.airline.domain.airline.model.Airline;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AirlineRepository extends JpaRepository<Airline, Long> {

    Page<Airline> findAllByActiveTrue(Pageable pageable);
}
