package com.bonatto.airline.domain.airline.service;

import static org.assertj.core.api.Assertions.assertThat;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.time.LocalDateTime;
import java.time.Month;
import java.util.Collections;
import java.util.List;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase.Replace;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ActiveProfiles;

import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.dto.FlightScheduleDetailData;
import com.bonatto.airline.domain.airline.model.Aircraft;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.model.Flight;
import com.bonatto.airline.domain.airline.repository.AircraftRepository;
import com.bonatto.airline.domain.airline.repository.FlightRepository;
import com.bonatto.airline.domain.validation.FlightScheduleValidation;
import com.bonatto.airline.infra.error.RegisterException;


@SpringBootTest
@AutoConfigureTestDatabase(replace = Replace.NONE)
@ActiveProfiles("test")
@ExtendWith(MockitoExtension.class)
class FlightScheduleServiceTest {

	
	
	
	
	private FlightScheduleService flightService;
	


	@Mock
	private AircraftRepository aircraftRepo;
	
	@Mock
    private FlightRepository flightRepo;
	
	
    @Mock
    private List<FlightScheduleValidation> validators;
    
	
    
    @BeforeEach
    void setUp()
    {
    	flightService = new FlightScheduleService(aircraftRepo, flightRepo, validators);
    }
    
	@Test
	@DisplayName("Expect RegisterException when aircraft id is invalid")
	public void scheduleS1()
	{
		
		
		FlightScheduleData scheduleData = new FlightScheduleData(1l,
					LocalDateTime.of(2023, Month.JANUARY, 1, 10, 0, 0), 
					LocalDateTime.of(2023, Month.JANUARY, 1, 14, 0, 0));
		
		  RegisterException thrown = assertThrows(RegisterException.class, () -> {
			  flightService.schedule(scheduleData);
	  });
		
	  assertEquals("Aircraft ID not found", thrown.getMessage());
	  
	}
	
	
	
	
	
	 @Test
	 @DisplayName("Flight is scheduled when all informations are valid")
	    void scheduleS2() {
		 
		 //given
		 LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 10, 0, 0);
		 LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 14, 0, 0);
		 Aircraft aircraft = new Aircraft(1l, 
				 new Airline(1l, true, "Test Airline", Collections.emptyList(), Collections.emptyList()),
				 "A350", 300, 10000, 20000, Collections.emptyList());
		 
		 when(aircraftRepo.existsById(1l)).thenReturn(true);
		 when(aircraftRepo.getReferenceById(any())).thenReturn(aircraft);
		 
		 FlightScheduleData scheduleData = new FlightScheduleData(1l, departure, arrival);
		 	
		 //when
		 
		 FlightScheduleDetailData flightData = flightService.schedule(scheduleData);
		 
		 //then
		 
		 ArgumentCaptor<Flight> argCaptor = ArgumentCaptor.forClass(Flight.class);
		 
		 verify(flightRepo).save(argCaptor.capture());
		 Flight capFlight = argCaptor.getValue();
		 FlightScheduleDetailData resultFlighDetail = new FlightScheduleDetailData(capFlight);
		 
		 assertThat(flightData).isEqualTo(resultFlighDetail);
		 
	 }
	     
	
	
	
	
	
	
	
	
	
}
