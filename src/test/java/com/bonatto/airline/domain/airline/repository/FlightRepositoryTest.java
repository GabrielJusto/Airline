package com.bonatto.airline.domain.airline.repository;

import static org.assertj.core.api.Assertions.assertThat;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.time.Month;
import java.util.Optional;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase.Replace;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.autoconfigure.orm.jpa.TestEntityManager;
import org.springframework.test.context.ActiveProfiles;

import com.bonatto.airline.domain.airline.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.dto.AirlineRegisterData;
import com.bonatto.airline.domain.airline.model.Aircraft;
import com.bonatto.airline.domain.airline.model.Airline;
import com.bonatto.airline.domain.airline.model.Flight;
import com.bonatto.airline.domain.airport.dto.AirportRegisterData;
import com.bonatto.airline.domain.airport.model.Airport;


@DataJpaTest
@AutoConfigureTestDatabase(replace = Replace.NONE)
@ActiveProfiles("test")
class FlightRepositoryTest {

	
	@Autowired
	private FlightRepository flightRepo;


	@Autowired
	private TestEntityManager em;



	@BeforeEach
	public void setScenario()
	{
		Airline airline = registerAirline();
		Aircraft aircraft = registerAircraft(airline);
		Airport source = registerSource();
		Airport destination = registerDestination();
		registerFlight(LocalDateTime.of(2023, Month.JANUARY, 1, 15, 0),
				LocalDateTime.of(2023, Month.JANUARY, 1, 18, 0),
				aircraft, source, destination);
	}

	@Test
	@DisplayName("Return empty when departure and arrival time are before other flight's departure time")
	void testFindByDateDepartureBefore() {

		LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 10, 0);
		LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 14, 0);

		Optional<Flight> optFlight = flightRepo.findByDate(departure, arrival);

		assertThat(optFlight.isEmpty()).isTrue();
	}

	@Test
	@DisplayName("Return empty when departure and arrival time are before other flight's departure time")
	void testFindByDateArrivalAfter() {

		LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 19, 0);
		LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 20, 0);

		Optional<Flight> optFlight = flightRepo.findByDate(departure, arrival);

		assertThat(optFlight.isEmpty()).isTrue();
	}


	@Test
	@DisplayName("Return flight when departure time are between other flight's departure and arrival time")
	void testFindByDateDepartureBetween() {

		LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 16, 0);
		LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 19, 0);

		Optional<Flight> optFlight = flightRepo.findByDate(departure, arrival);

		assertThat(optFlight.isPresent()).isTrue();
	}


	@Test
	@DisplayName("Return flight when arrival time are between other flight's departure and arrival time")
	void testFindByDateArrivalBetween() {

		LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 10, 0);
		LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 16, 0);

		Optional<Flight> optFlight = flightRepo.findByDate(departure, arrival);

		assertThat(optFlight.isPresent()).isTrue();
	}

	@Test
	@DisplayName("Return flight when departure and arrival time are between other flight's departure and arrival time")
	void testFindByDateDepartureAndArrivalBetween() {

		LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 16, 0);
		LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 17, 0);

		Optional<Flight> optFlight = flightRepo.findByDate(departure, arrival);

		assertThat(optFlight.isPresent()).isTrue();
	}

	@Test
	@DisplayName("Return flight when other flight's departure and arrival time are between departure and arrival time")
	void testFindByDateOtherDepartureAndArrivalBetween() {

		LocalDateTime departure = LocalDateTime.of(2023, Month.JANUARY, 1, 14, 0);
		LocalDateTime arrival = LocalDateTime.of(2023, Month.JANUARY, 1, 19, 0);

		Optional<Flight> optFlight = flightRepo.findByDate(departure, arrival);

		assertThat(optFlight.isPresent()).isTrue();
	}



	private Airline registerAirline()
	{
		Airline airline = new Airline(createAirlineRegisterData());
		em.persist(airline);
		return airline;
	}

	private Aircraft registerAircraft(Airline airline)
	{
		Aircraft aircraft = new Aircraft(createAircraftRegisterData(airline), airline);
		em.persist(aircraft);
		return aircraft;
	}

	private Airport registerSource()
	{
		Airport airport = new Airport(createSource());
		
		em.persist(airport);
		return airport;
		
	}
	
	private Airport registerDestination()
	{
		Airport airport = new Airport(createDestination());
		
		em.persist(airport);
		return airport;
		
	}

	private void registerFlight(LocalDateTime departure, LocalDateTime arrival, Aircraft aircraft,
			Airport source, Airport destination)
	{
		
		
		Flight flight = new Flight(aircraft, departure, arrival, source, destination);
		em.persist(flight);
	}


	private AirportRegisterData createSource()
	{
		return new AirportRegisterData(
				new BigDecimal(60),
				new BigDecimal(-10),
				"LAS",
				"Las Vegas",
				"United States"
				);
	}
	
	private AirportRegisterData createDestination()
	{
		return new AirportRegisterData(
				new BigDecimal(-50),
				new BigDecimal(100),
				"LAS",
				"Las Vegas",
				"United States"
				);
	}
	
	private AircraftRegisterData createAircraftRegisterData(Airline airline)
	{
		return new AircraftRegisterData(
				airline.getId(),
				"A350",
				300,
				10000,
				20000
		);
	}

	private AirlineRegisterData createAirlineRegisterData()
	{
		return new AirlineRegisterData(
				"Teste Airline"
		);
	}
}
