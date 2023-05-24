package com.bonatto.airline.domain.customer.dto;

import com.bonatto.airline.domain.customer.model.Customer;
import com.bonatto.airline.domain.customer.model.Level;

public record CustomerData(Long id, String name, String email, Level level)
{
    public CustomerData(Customer customer)
    {
        this(customer.getId(), customer.getName(), customer.getEmail(), customer.getLevel());
    }
}
