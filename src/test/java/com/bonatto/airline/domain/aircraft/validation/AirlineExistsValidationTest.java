package com.bonatto.airline.domain.aircraft.validation;

import static org.mockito.Mockito.when;

import java.util.Collections;

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

import com.bonatto.airline.domain.aircraft.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;
import com.bonatto.airline.infra.error.RegisterException;

@SpringBootTest
@AutoConfigureTestDatabase(replace = Replace.NONE)
@ActiveProfiles("test")
@ExtendWith(MockitoExtension.class)

class AirlineExistsValidationTest {

	
	@Mock
	private AirlineRepository airlineRepo;
	
	private AirlineExistsValidation validator;

	
	@BeforeEach
	void setup()
	{
		this.validator = new AirlineExistsValidation(airlineRepo);
	}
	
	@Test
	@DisplayName("expect RegisterException when there's not airline registered with id")
	void existxValidationS1()
	{
		//given
		when(airlineRepo.existsById(1l)).thenReturn(false);
		
		//when //then
		Assertions.assertThatThrownBy(() -> validator.validate(getAircraftRegisterData()))
		.isInstanceOf(RegisterException.class)
		.hasMessage("This Airline doesn't exists!");
		
	}
	
	@Test
	@DisplayName("expect no exceptions when there's an airline registered with id")
	void existxValidationS2()
	{
		//given
		when(airlineRepo.existsById(1l)).thenReturn(true);
		
		//when //then
		Assertions.assertThatCode(() -> validator.validate(getAircraftRegisterData()))
		.doesNotThrowAnyException();
		
	}
	
	
	
	
	
	
	
	private AircraftRegisterData getAircraftRegisterData()
	{
		return new AircraftRegisterData(1l, "787", 300, 10000, 20000, Collections.emptyList());
	}
	
	
	
}
