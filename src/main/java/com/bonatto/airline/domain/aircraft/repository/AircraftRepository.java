package com.bonatto.airline.domain.aircraft.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.bonatto.airline.domain.aircraft.model.Aircraft;

public interface AircraftRepository extends JpaRepository<Aircraft, Long> {
}
