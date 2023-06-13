package com.bonatto.airline.domain.validation;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;

import java.time.LocalDateTime;
import java.util.Optional;

import org.assertj.core.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase.Replace;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ActiveProfiles;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.model.Flight;
import com.bonatto.airline.domain.airline.repository.FlightRepository;
import com.bonatto.airline.infra.error.RegisterException;


@SpringBootTest
@AutoConfigureTestDatabase(replace = Replace.NONE)
@ActiveProfiles("test")
@ExtendWith(MockitoExtension.class)
class FlightScheduleTimeValidatorTest {

	
	
	private FlightScheduleTimeValidator validator;
	
	@Mock
	private FlightRepository flightRepo;

	
	@BeforeEach
	void setup()
	{
		this.validator = new FlightScheduleTimeValidator(flightRepo);
	}
	
	
	@Test
	@DisplayName("Expect RegisterException when there is already a flight at this time")
	void validFlightS1()
	{
		//given
		when(flightRepo.findByDate(any(), any())).thenReturn(Optional.of(new Flight()));

		//when
		
		//then
		Assertions.assertThatExceptionOfType(RegisterException.class)
						.isThrownBy(
								() -> validator.validate(
										new FlightScheduleData(1l, LocalDateTime.now(), LocalDateTime.now().plusHours(1),"POA", "SAO")));
	}
	
	
	@Test
	@DisplayName("validation pass when there isn't a flight at this time")
	void validFlightS2()
	{
		//given
		when(flightRepo.findByDate(any(), any())).thenReturn(Optional.empty());

		//when / then
		Assertions.assertThatCode(() -> validator.validate(
				new FlightScheduleData(1l, LocalDateTime.now(), LocalDateTime.now().plusHours(1), "POA", "SAO")))
		.doesNotThrowAnyException();

	}
	
	
	
	
	
	
	
}
