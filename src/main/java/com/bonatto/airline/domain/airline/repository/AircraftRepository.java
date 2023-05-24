package com.bonatto.airline.domain.airline.repository;

import com.bonatto.airline.domain.airline.model.Aircraft;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AircraftRepository extends JpaRepository<Aircraft, Long> {
}
