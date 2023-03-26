package com.bonatto.airline.custumer;

public record CustomerDto(Long id, String name, String email, Level level)
{
    public CustomerDto(Customer customer)
    {
        this(customer.getId(), customer.getName(), customer.getEmail(), customer.getLevel());
    }
}
