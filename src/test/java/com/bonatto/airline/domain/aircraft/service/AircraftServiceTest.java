package com.bonatto.airline.domain.aircraft.service;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.Collections;
import java.util.List;
import java.util.Optional;

import org.assertj.core.api.Assertions;
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

import com.bonatto.airline.domain.aircraft.dto.AircraftDetailData;
import com.bonatto.airline.domain.aircraft.dto.AircraftRegisterData;
import com.bonatto.airline.domain.aircraft.dto.SeatRegisterData;
import com.bonatto.airline.domain.aircraft.model.Aircraft;
import com.bonatto.airline.domain.aircraft.model.SeatClass;
import com.bonatto.airline.domain.aircraft.repository.AircraftRepository;
import com.bonatto.airline.domain.aircraft.repository.SeatRepository;
import com.bonatto.airline.domain.aircraft.validation.AircraftRegisterValidation;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;


@SpringBootTest
@AutoConfigureTestDatabase(replace = Replace.NONE)
@ActiveProfiles("test")
@ExtendWith(MockitoExtension.class)
class AircraftServiceTest {

	private AircraftService aircraftService;
	
	@Mock
	private AircraftRepository aircraftRepo;
	
	@Mock
	private SeatRepository seatRepo;
	
	@Mock
	private AirlineRepository airlineRepo;
	
	@Mock
    private List<AircraftRegisterValidation> validators;
	
	

	@BeforeEach
	void setup()
	{
		this.aircraftService = new AircraftService(
				aircraftRepo, seatRepo, airlineRepo, validators);
	}
	
	
	@Test
	@DisplayName("Register aircraft")
	void registerAircraft()
	{
		//given
		when(airlineRepo.findById(1l)).thenReturn(getAirline());
		
		//when
		AircraftDetailData aircraft = aircraftService.register(getRegisterData());
		
		//then
		ArgumentCaptor<Aircraft> argCaptor = ArgumentCaptor.forClass(Aircraft.class);
		verify(aircraftRepo).save(argCaptor.capture());
		Aircraft aircraftCap = argCaptor.getValue();
		AircraftDetailData aircraftDataResult = new AircraftDetailData(aircraftCap);

		
		Assertions.assertThat(aircraftDataResult).isEqualTo(aircraft);
	}
	
	
	
	private Optional<Airline> getAirline()
	{
		return Optional.of( new Airline(1l, true, "Test Airline",
				Collections.emptyList(),
				Collections.emptyList()));
	}
	
	private AircraftRegisterData getRegisterData()
	{
		return new AircraftRegisterData(1l, "787", 300, 10000, 20000, getSeatList());
	}
	
	private List<SeatRegisterData> getSeatList()
	{
		
		return List.of(
				new SeatRegisterData("A1", SeatClass.ECONOMY),
				new SeatRegisterData("A2", SeatClass.ECONOMY),
				new SeatRegisterData("B1", SeatClass.BUSINESS),
				new SeatRegisterData("B2", SeatClass.BUSINESS),
				new SeatRegisterData("C1", SeatClass.FIRST_CLASS)
				);
	}
}



















