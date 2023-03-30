package com.bonatto.airline.airline;

import com.bonatto.airline.custumer.Customer;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AirlineRepository extends JpaRepository<Airline, Long> {
}
