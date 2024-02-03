package com.amspringbootcamp.jpa.model;

import com.amspringbootcamp.jpa.customvalidation.amCustomValidation;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;

import java.util.List;

public class Student {
    @NotNull(message = "is required")
    @Size(min = 1, message = "is required")
    @Pattern(regexp = "[a-zA-Z]*", message = "only alphabets allowed")
    @amCustomValidation(value = "AM", message = "must start with AM")
    private String firstName;
    private String lastName;
    private String country;
    private String favoriteLanguage;

    private List<String> favoriteOperatingSystems;

    public List<String> getFavoriteOperatingSystems() {
        return favoriteOperatingSystems;
    }

    public void setFavoriteOperatingSystems(List<String> favoriteOperatingSystems) {
        this.favoriteOperatingSystems = favoriteOperatingSystems;
    }

    public String getFavoriteLanguage() {
        return favoriteLanguage;
    }

    public void setFavoriteLanguage(String favoriteLanguage) {
        this.favoriteLanguage = favoriteLanguage;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public Student() {

    }

    public String getCountry() {
        return country;
    }

    public void setCountry(String country) {
        this.country = country;
    }
}
