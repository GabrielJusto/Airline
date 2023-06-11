package com.bonatto.airline.controller;

import com.bonatto.airline.domain.user.dto.AuthenticationData;
import com.bonatto.airline.domain.user.model.User;
import com.bonatto.airline.infra.security.TokenJwtData;
import com.bonatto.airline.infra.security.TokenService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/login")
public class AuthenticationController {

    @Autowired
    private AuthenticationManager manager;

    @Autowired
    private TokenService tokenService;

    
    @SuppressWarnings("rawtypes")
    @PostMapping
    public ResponseEntity login( @RequestBody @Valid AuthenticationData data)
    {
        Authentication authenticationToken = new UsernamePasswordAuthenticationToken(data.login(), data.password());
        Authentication authentication =  manager.authenticate(authenticationToken);
        System.out.println(data.login());
        String tokenJWT = tokenService.generateToken((User) authentication.getPrincipal());

        return ResponseEntity.ok(new TokenJwtData(tokenJWT));
    }

}
