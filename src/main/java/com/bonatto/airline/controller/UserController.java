package com.bonatto.airline.controller;

import java.net.URI;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.util.UriComponentsBuilder;

import com.bonatto.airline.domain.user.dto.UserDetailData;
import com.bonatto.airline.domain.user.dto.UserRegisterData;
import com.bonatto.airline.domain.user.model.User;
import com.bonatto.airline.domain.user.service.UserService;

import jakarta.transaction.Transactional;
import jakarta.validation.Valid;

@RestController
@RequestMapping("/user")
public class UserController {

	@Autowired
	UserService userService;
	
	@PutMapping("register")
	@Transactional
	public ResponseEntity<UserDetailData> register(@RequestBody @Valid UserRegisterData data,  UriComponentsBuilder uriBuilder)
	{
		
		User user = userService.saveUser(data);
		UserDetailData userDetail = new UserDetailData(user);
		
		URI uri = uriBuilder.path("/user/{id}").buildAndExpand(userDetail.id()).toUri();
        return ResponseEntity.created(uri).body(userDetail);
	        
	}
}
