package com.amspringbootcamp.jpa.customvalidation;

import jakarta.validation.Constraint;
import jakarta.validation.Payload;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Constraint(validatedBy = AMCustomConstraintValidator.class)
@Target( { ElementType.METHOD, ElementType.FIELD } )
@Retention(RetentionPolicy.RUNTIME)
public @interface amCustomValidation {
    // define default course code
    public String value() default "AM";
    // define default error message
    public String message() default "must start with AM";

    // define default groups
    public Class<?>[] groups() default {};

    // define default payloads
    public Class <? extends Payload>[] payload() default {};
}
