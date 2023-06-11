package com.bonatto.airline.domain.airline.dto;

import com.fasterxml.jackson.annotation.JsonFormat;
import jakarta.validation.constraints.Future;
import jakarta.validation.constraints.NotNull;

import java.time.LocalDateTime;

public record FlightScheduleData(
		
		
		@NotNull
		Long aircfraftId,
		
		@Future
		@JsonFormat(pattern="yyyy-MM-dd HH:mm:ss")
		LocalDateTime departure,
		
		@Future
		@JsonFormat(pattern="yyyy-MM-dd HH:mm:ss")
		LocalDateTime arrival
		
		) {

}
