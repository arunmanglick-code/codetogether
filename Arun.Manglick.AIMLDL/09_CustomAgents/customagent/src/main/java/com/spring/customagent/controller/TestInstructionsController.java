/*
 * Author: Arun Manglick
 * Created: 2026-04-05
 * Updated: 2026-06-29
 */

package com.spring.customagent.controller;

import org.springframework.context.annotation.Profile;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/student")
@Profile("dev")
public class TestInstructionsController {

    @GetMapping("/checkinstructions")
    public String checkInstructions() {
        return "Confirm Github instructions are used which are defined in the file copilot-instructions.md!";
    }
}
