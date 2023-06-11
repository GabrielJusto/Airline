package com.bonatto.airline.controller;

import com.bonatto.airline.domain.airline.dto.AircraftDetailData;
import com.bonatto.airline.domain.airline.dto.FlightScheduleData;
import com.bonatto.airline.domain.airline.dto.FlightScheduleDetailData;
import com.bonatto.airline.domain.airline.service.FlightScheduleService;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.json.AutoConfigureJsonTesters;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.json.JacksonTester;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.web.servlet.MockMvc;

import java.time.LocalDateTime;

import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.put;


@AutoConfigureMockMvc
@SpringBootTest
@AutoConfigureJsonTesters
class FlightControllerTest {

    @Autowired
    private MockMvc mvc;

    @Autowired
    private JacksonTester<FlightScheduleData> flightScheduleDataJson;

    @Autowired
    private JacksonTester<FlightScheduleDetailData> flightScheduleDetailDataJson;


    @MockBean
    private FlightScheduleService scheduleService;

    @WithMockUser
    @Test
    @DisplayName("Return error 400 when request doesn't have body ")
    public void scheduleFlightS1() throws Exception {

        MockHttpServletResponse response = mvc.perform(put("/flight/schedule"))
                .andReturn().getResponse();

        assertThat(response.getStatus()).isEqualTo(HttpStatus.BAD_REQUEST.value());

    }

    @WithMockUser
    @Test
    @DisplayName("Return error 200 when all information on request are valid")
    public void scheduleFlightS2() throws Exception {

        Long aircraftId = 1l;
        LocalDateTime departure =  LocalDateTime.now().plusHours(1);
        LocalDateTime arrival = LocalDateTime.now().plusHours(5);

        FlightScheduleDetailData flightData = new FlightScheduleDetailData(
                1l,
                new AircraftDetailData(aircraftId, 1l, "A350", 300),
                departure,
                arrival);

        when(scheduleService.schedule(any())).thenReturn(flightData);


        MockHttpServletResponse response =
                mvc.perform(put("/flight/schedule")
                                .contentType(MediaType.APPLICATION_JSON)
                                .content(flightScheduleDataJson.write(
                                        new FlightScheduleData(
                                                aircraftId,
                                                departure,
                                                arrival
                                        )
                                ).getJson())
                        )
                .andReturn().getResponse();

        assertThat(response.getStatus()).isEqualTo(HttpStatus.CREATED.value());

        String responseJson = flightScheduleDetailDataJson.write(flightData).getJson();


        assertThat(response.getContentAsString()).isEqualTo(responseJson);
    }

}