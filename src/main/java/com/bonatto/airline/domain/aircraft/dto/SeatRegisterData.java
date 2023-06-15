package com.bonatto.airline.domain.aircraft.dto;

import com.bonatto.airline.domain.aircraft.model.SeatClass;

public record SeatRegisterData(
		String place,
		SeatClass seatClass
		) {

}
