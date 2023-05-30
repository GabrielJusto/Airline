package com.bonatto.airline.infra.error;

import jakarta.persistence.EntityNotFoundException;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.MethodArgumentNotValidException;
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

    @ExceptionHandler(MethodArgumentNotValidException.class)
    public ResponseEntity manage400(MethodArgumentNotValidException e)
    {
        var errors = e.getFieldErrors();



        return ResponseEntity.badRequest().body(errors.stream().map( err -> new BadRequestData(err)).toList());
    }

    private record BadRequestData( String field, String message){
        public BadRequestData(FieldError err)
        {
            this(err.getField(), err.getDefaultMessage());
        }

    }
}
