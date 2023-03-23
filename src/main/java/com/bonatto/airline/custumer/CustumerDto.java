package com.bonatto.airline.custumer;

public record CustumerDto(Long id, String name, String email, Level level)
{
    public CustumerDto( Custumer custumer)
    {
        this(custumer.getId(), custumer.getName(), custumer.getEmail(), custumer.getLevel());
    }
}
