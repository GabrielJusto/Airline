package com.bonatto.airline.domain.airline.repository;

import com.bonatto.airline.domain.airline.model.Flight;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.time.LocalDateTime;
import java.util.Optional;

public interface FlightRepository extends JpaRepository<Flight, Long> {
	
	
	
	
	
	@Query("""
			SELECT f FROM Flight f
			WHERE 
			(f.departure >= :departure AND f.arrival <= :arrival)
			OR 
			(f.departure <= :departure AND f.arrival >= :departure)
			OR
			(f.departure <= :arrival AND f.arrival >= :arrival)
			""")
	public Optional<Flight> findByDate(LocalDateTime departure, LocalDateTime arrival);

}
