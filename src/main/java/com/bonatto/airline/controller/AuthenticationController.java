package com.bonatto.airline.controller;

import com.bonatto.airline.domain.user.dto.AuthenticationData;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/login")
public class AuthenticationController {

    @Autowired
    private AuthenticationManager manager;
    @PostMapping
    public ResponseEntity login( @RequestBody @Valid AuthenticationData data)
    {
        Authentication token = new UsernamePasswordAuthenticationToken(data.login(), data.password());
        Authentication authentication =  manager.authenticate(token);

        return ResponseEntity.ok().build();
    }

}
