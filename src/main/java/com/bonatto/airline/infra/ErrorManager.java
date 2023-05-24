package com.bonatto.airline.infra;

import jakarta.persistence.EntityNotFoundException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class ErrorManager
{

    @ExceptionHandler(EntityNotFoundException.class)
    public ResponseEntity manage404()
    {
        return ResponseEntity.notFound().build();
    }

}
