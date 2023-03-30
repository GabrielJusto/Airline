package com.bonatto.airline.airline;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

@Entity
public class Aircraft {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    private Airline airline;

    @NotBlank
    private String model;

    @NotNull
    private int passagerCapacity;

    @NotNull
    private double takeoffWeight;

    @NotNull
    private double aircraftRange;

    @Embedded
    private Scheduler scheduler;
}
