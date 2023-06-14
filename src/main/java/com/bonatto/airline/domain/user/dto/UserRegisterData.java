package com.bonatto.airline.domain.user.dto;

import jakarta.validation.constraints.NotBlank;

public record UserRegisterData(
		@NotBlank
		String username,
		
		@NotBlank
		String password
		) {

}
