package com.bonatto.airline.custumer;

public record CustumerDto( String name, String email, Level level)
{
    public CustumerDto( Custumer custumer)
    {
        this(custumer.getName(), custumer.getEmail(), custumer.getLevel());
    }
}
