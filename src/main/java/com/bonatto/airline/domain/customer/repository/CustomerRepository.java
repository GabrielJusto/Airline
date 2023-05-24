package com.bonatto.airline.domain.customer.repository;

import com.bonatto.airline.domain.customer.model.Customer;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CustomerRepository extends JpaRepository<Customer, Long> {
    Page<Customer> findAllByActiveTrue(Pageable pageable);
}
