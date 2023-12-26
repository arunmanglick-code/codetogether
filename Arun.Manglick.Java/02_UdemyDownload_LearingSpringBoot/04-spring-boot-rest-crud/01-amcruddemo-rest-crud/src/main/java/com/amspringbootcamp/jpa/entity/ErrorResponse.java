package com.amspringbootcamp.jpa.entity;

public class ErrorResponse {
    private int errorStatus;
    private String errorMessage;

    private long errorTimestamp;

    public int getErrorStatus() {
        return errorStatus;
    }

    public void setErrorStatus(int errorStatus) {
        this.errorStatus = errorStatus;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public void setErrorMessage(String errorMessage) {
        this.errorMessage = errorMessage;
    }

    public long getErrorTimestamp() {
        return errorTimestamp;
    }

    public void setErrorTimestamp(long errorTimestamp) {
        this.errorTimestamp = errorTimestamp;
    }
}
