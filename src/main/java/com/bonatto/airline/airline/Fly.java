package com.bonatto.airline.airline;

import jakarta.persistence.Embeddable;
import lombok.AllArgsConstructor;
import lombok.Getter;

import java.time.LocalDate;

@AllArgsConstructor
@Getter
@Embeddable
public class Fly
{
    private LocalDate dateDeparture;
    private LocalDate dateArrival;
    private String source;
    private String destination;
}
