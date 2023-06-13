package com.bonatto.airline.domain.validation;

import static org.mockito.Mockito.when;

import java.time.LocalDateTime;

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
import com.bonatto.airline.domain.airport.repository.AirportRepository;
import com.bonatto.airline.infra.error.RegisterException;


@SpringBootTest
@AutoConfigureTestDatabase(replace = Replace.NONE)
@ActiveProfiles("test")
@ExtendWith(MockitoExtension.class)
class AirportExistsValidationTest {

	
	private AirportExistsValidation airportValidation;
	
	@Mock
	private AirportRepository airportRepo;
	
	@BeforeEach
	void setup ()
	{
		this.airportValidation = new AirportExistsValidation(airportRepo);
	}
	
	
	@Test
	@DisplayName("Return RegisterException when source airport doesn't exists")
	void airportValidationTesteS1()
	{
		
		//given
		when(airportRepo.existsByCode("POA")).thenReturn(false);
		
		//when / then
		
		Assertions.assertThatThrownBy((() -> airportValidation.validate(new FlightScheduleData(1l, 
					LocalDateTime.now().plusHours(1), 
					LocalDateTime.now().plusHours(3), 
					"POA", 
					"SAO"))))
		.isInstanceOf(RegisterException.class)
		.hasMessage("Source airport does not exists");
		
		
		
	}
	
	@Test
	@DisplayName("Return RegisterException when destination airport doesn't exists")
	void airportValidationTesteS2()
	{
		
		//given
		when(airportRepo.existsByCode("POA")).thenReturn(true);
		when(airportRepo.existsByCode("SAO")).thenReturn(false);
		
		//when / then
		
		Assertions.assertThatThrownBy((() -> airportValidation.validate(new FlightScheduleData(1l, 
					LocalDateTime.now().plusHours(1), 
					LocalDateTime.now().plusHours(3), 
					"POA", 
					"SAO"))))
		.isInstanceOf(RegisterException.class)
		.hasMessage("Destination airport does not exists");
		
	}
	
	
	@Test
	@DisplayName("It doesn't throw exception when source and destination airports exists")
	void airportValidationTesteS3()
	{
		
		//given
		when(airportRepo.existsByCode("POA")).thenReturn(true);
		when(airportRepo.existsByCode("SAO")).thenReturn(true);
		
		//when / then
		Assertions.assertThatCode((() -> airportValidation.validate(new FlightScheduleData(1l, 
					LocalDateTime.now().plusHours(1), 
					LocalDateTime.now().plusHours(3), 
					"POA", 
					"SAO"))))
		.doesNotThrowAnyException();;
		
	}
	

}















