package com.bonatto.airline.airline.repository;

import com.bonatto.airline.airline.model.Aircraft;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AircraftRepository extends JpaRepository<Aircraft, Long> {
}
