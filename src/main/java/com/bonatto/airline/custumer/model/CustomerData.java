package com.bonatto.airline.custumer.model;

public record CustomerData(Long id, String name, String email, Level level)
{
    public CustomerData(Customer customer)
    {
        this(customer.getId(), customer.getName(), customer.getEmail(), customer.getLevel());
    }
}
