package com.bonatto.airline.domain.aircraft.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.bonatto.airline.domain.aircraft.model.Seat;

public interface SeatRepository extends JpaRepository<Seat, Long>{

}
