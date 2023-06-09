package com.bonatto.airline.controller;


import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.customer.dto.CustomerData;
import com.bonatto.airline.domain.customer.dto.CustomerRegisterData;
import com.bonatto.airline.domain.customer.dto.CustomerUpdateData;
import com.bonatto.airline.domain.customer.model.Customer;
import com.bonatto.airline.domain.customer.repository.CustomerRepository;

import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;


@RestController
@RequestMapping("/customer")
@SecurityRequirement(name = "bearer-key")
public class CustomerController {


    @Autowired
    private CustomerRepository customerRepository;
    
    
    
    @SuppressWarnings("rawtypes")
	@PutMapping("/register")
    @Transactional
    public ResponseEntity register(@RequestBody @Valid CustomerRegisterData data, UriComponentsBuilder uriBuilder)
    {
        Customer c = new Customer(data);
        customerRepository.save(c);

        URI uri = uriBuilder.path("/customer/{id}").buildAndExpand(c.getId()).toUri();
        return ResponseEntity.created(uri).body(new CustomerData(c));
    }

    
    @SuppressWarnings("rawtypes")
    @GetMapping("/{id}")
    public ResponseEntity detail(@PathVariable Long id)
    {
        Customer c = customerRepository.getReferenceById(id);
        return ResponseEntity.ok(new CustomerData(c));
    }
   
    
    @GetMapping("/list")
    public ResponseEntity<Page<CustomerData>> list (Pageable pageable)
    {
        Page<CustomerData> page = customerRepository
                .findAllByActiveTrue(pageable)
                .map( CustomerData:: new);

        return ResponseEntity.ok(page);

    }

    
    @PutMapping("/update")
    @Transactional
    public ResponseEntity<CustomerData> updateCustomer(@RequestBody @Valid CustomerUpdateData data)
    {
        Customer cus = customerRepository.getReferenceById(data.id());
        cus.update(data);

        return ResponseEntity.ok(new CustomerData(cus));
    }
    
    
    @SuppressWarnings("rawtypes")
    @DeleteMapping("/delete/{id}")
    @Transactional
    public ResponseEntity deleteCustomer(@PathVariable Long id)
    {
        Customer cus = customerRepository.getReferenceById(id);
        cus.deactivate();

        return ResponseEntity.noContent().build();
    }


}
