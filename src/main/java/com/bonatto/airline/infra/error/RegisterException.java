package com.bonatto.airline.infra.error;

public class RegisterException extends RuntimeException {
	
	
	private static final long serialVersionUID = 1L;

	public RegisterException(String message)
	{
		super(message);
	}

}
