package com.bonatto.airline.controller;


import com.bonatto.airline.custumer.dto.CustomerRegisterData;
import com.bonatto.airline.custumer.dto.CustomerUpdateData;
import com.bonatto.airline.custumer.model.Customer;
import com.bonatto.airline.custumer.model.CustomerData;
import com.bonatto.airline.custumer.repository.CustomerRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;


@RestController
@RequestMapping("/custumer")
public class CustumerController {


    @Autowired
    private CustomerRepository customerRepository;
    @PostMapping
    @Transactional
    public void register(@RequestBody @Valid CustomerRegisterData data)
    {
        customerRepository.save(new Customer(data));
    }

    @GetMapping("/list")
    public Page<CustomerData> list (Pageable pageable)
    {
        return customerRepository
                .findAllByActiveTrue(pageable)
                .map( CustomerData:: new);
    }

    @PutMapping("/update")
    @Transactional
    public void updateCustumer(@RequestBody @Valid CustomerUpdateData data)
    {
        Customer cus = customerRepository.getReferenceById(data.id());
        cus.update(data);
    }
    @DeleteMapping("/delete/{id}")
    @Transactional
    public void deleteCustumer(@PathVariable Long id)
    {
        Customer cus = customerRepository.getReferenceById(id);
        cus.deactivate();
    }


}
