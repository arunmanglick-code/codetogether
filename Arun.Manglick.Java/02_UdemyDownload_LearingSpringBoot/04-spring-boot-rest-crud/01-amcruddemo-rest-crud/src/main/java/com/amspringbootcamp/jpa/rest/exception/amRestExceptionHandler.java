package com.amspringbootcamp.jpa.rest.exception;

import com.amspringbootcamp.jpa.entity.ErrorResponse;
import com.amspringbootcamp.jpa.entity.StudentNotFoundException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

import static org.springframework.http.HttpStatus.BAD_REQUEST;
import static org.springframework.http.HttpStatus.NOT_FOUND;

@ControllerAdvice
public class amRestExceptionHandler {
    @ExceptionHandler
    public ResponseEntity<ErrorResponse> handleException(StudentNotFoundException exc) {
        ErrorResponse errorResponse = new ErrorResponse();

        errorResponse.setErrorStatus(NOT_FOUND.value());
        errorResponse.setErrorMessage(exc.getMessage());
        errorResponse.setErrorTimestamp(System.currentTimeMillis());

        return new ResponseEntity<>(errorResponse, NOT_FOUND);
    }

    @ExceptionHandler
    public ResponseEntity<ErrorResponse> handleException(Exception exc) {
        ErrorResponse errorResponse = new ErrorResponse();

        errorResponse.setErrorStatus(BAD_REQUEST.value());
        errorResponse.setErrorMessage(exc.getMessage());
        errorResponse.setErrorTimestamp(System.currentTimeMillis());

        return  new ResponseEntity<>(errorResponse, BAD_REQUEST);
    }
}
