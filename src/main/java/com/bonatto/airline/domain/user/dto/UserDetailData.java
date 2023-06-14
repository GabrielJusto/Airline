package com.bonatto.airline.domain.user.dto;

import com.bonatto.airline.domain.user.model.User;

public record UserDetailData(
		Long id,
		String username
		) {
	public UserDetailData(User user)
	{
		this(user.getId(), user.getUsername());
	}

}
