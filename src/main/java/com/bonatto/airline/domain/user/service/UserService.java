package com.bonatto.airline.domain.user.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.argon2.Argon2PasswordEncoder;
import org.springframework.stereotype.Service;

import com.bonatto.airline.domain.user.dto.UserRegisterData;
import com.bonatto.airline.domain.user.model.User;
import com.bonatto.airline.domain.user.repository.UserRepository;

@Service
public class UserService {

	@Autowired
	private UserRepository userRepo;
	
	
	
	public User saveUser(UserRegisterData data)
	{
		Argon2PasswordEncoder encoder = Argon2PasswordEncoder.defaultsForSpringSecurity_v5_8();
		String password = encoder.encode(data.password());
		password = "{argon2}" + password;
		
		User user = new User(data.username(), password);
		userRepo.save(user);
		
		return user;
		
	}
}
