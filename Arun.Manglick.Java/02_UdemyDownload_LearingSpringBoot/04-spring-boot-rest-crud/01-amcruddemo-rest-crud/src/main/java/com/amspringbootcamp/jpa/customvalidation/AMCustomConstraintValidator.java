package com.amspringbootcamp.jpa.customvalidation;

import jakarta.validation.ConstraintValidator;
import jakarta.validation.ConstraintValidatorContext;

import java.lang.annotation.Annotation;

public class AMCustomConstraintValidator implements ConstraintValidator<amCustomValidation, String> {

    private String stringPrefix;
    @Override
    public void initialize(amCustomValidation constraintAnnotation) {
        stringPrefix = constraintAnnotation.value();
    }

    @Override
    public boolean isValid(String input, ConstraintValidatorContext constraintValidatorContext) {
        boolean result;
        if (input != null) {
            result = input.startsWith(stringPrefix);
        }
        else {
            result = true;
        }
        return result;
    }
}
