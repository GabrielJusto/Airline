package com.bonatto.airline.infra.security;


import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;



@Component

public class SecurityFilter extends OncePerRequestFilter {

    @Autowired
    private TokenService tokenService;
    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain) throws ServletException, IOException {

        String token = getToken(request);
        String subject = tokenService.getSubject(token);



        filterChain.doFilter(request, response);

    }

    private String getToken(HttpServletRequest request)
    {
        String authorization = request.getHeader("Authorization");
        if(authorization == null)
            throw new RuntimeException("Token not received !");

        return authorization.replace("Bearer ", "");
    }
}
