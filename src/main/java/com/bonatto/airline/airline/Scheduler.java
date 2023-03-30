package com.bonatto.airline.airline;

import jakarta.persistence.Embeddable;
import jakarta.persistence.Embedded;
import lombok.NoArgsConstructor;

import java.time.LocalDate;
import java.util.LinkedList;
import java.util.List;


@Embeddable
public class Scheduler
{
    @Embedded
    private List<Fly> flights;

    public Scheduler()
    {
        this.flights = new LinkedList<>();
    }

    public boolean schedule(LocalDate departureDate, LocalDate arriveDate, String source, String destination)
    {
        for(Fly f : flights)
        {
            if(f.getDateDeparture().isAfter(departureDate) && f.getDateDeparture().isBefore(arriveDate)
            || f.getDateArrival().isAfter(departureDate) && f.getDateArrival().isBefore(arriveDate))
                return false;
        }

        flights.add(new Fly(departureDate, arriveDate, source, destination));

        return true;
    }

}
