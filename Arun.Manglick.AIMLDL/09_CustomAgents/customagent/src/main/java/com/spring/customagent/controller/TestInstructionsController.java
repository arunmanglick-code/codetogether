/*
 * Author: Arun Manglick
 * Created: 2026-04-05
 * Updated: 2026-04-05
 */

package com.spring.customagent.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class TestInstructionsController {
    
 @GetMapping("/checkinstructions")
    public String CheckInstructions() {
        return "Confirm Github instructions are used which are defined in the file copilot-instructions.md!";
    }       
}
