package com.bonatto.airline.domain.airport.dto;

import java.math.BigDecimal;

import org.hibernate.validator.constraints.Length;

import jakarta.validation.constraints.Max;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

public record AirportRegisterData(
		@NotNull @Min(value = -90) @Max(value=90)
		BigDecimal latitude, 
		
		@NotNull @Min(value = -180) @Max(value=180)
		BigDecimal longitude,
		
		@NotBlank @Length(min = 3, max =3)
		String code,

		@NotBlank
		String city,

		@NotBlank
		String country
		) {

}
