package com.bonatto.airline.infra.security;


import java.io.IOException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import com.bonatto.airline.domain.user.model.User;
import com.bonatto.airline.domain.user.repository.UserRepository;

import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;



@Component
public class SecurityFilter extends OncePerRequestFilter {

	
	@Autowired
	private UserRepository userRepo;
	
    @Autowired
    private TokenService tokenService;
    
    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain) throws ServletException, IOException {

        String token = getToken(request);
        
        if(token != null)
        {
        	String subject = tokenService.getSubject(token);
        	User user = userRepo.getByUsername(subject);
        	
        	
        	Authentication authentication = new UsernamePasswordAuthenticationToken(user, null, user.getAuthorities());
        	SecurityContextHolder.getContext().setAuthentication(authentication);


        }



        filterChain.doFilter(request, response);

    }

    private String getToken(HttpServletRequest request)
    {
        String authorization = request.getHeader("Authorization");
        if(authorization != null)
        	return authorization.replace("Bearer ", "");
        	
        
        return null;
    }
}
