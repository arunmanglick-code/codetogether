package am.learning.springboot.rest;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class FunRestController {

    @Value("${coach.name}")
    private String coachName;

    @Value("${team.name}")
    private String teamName;

    @GetMapping("/")
    public String healthCheck(){
        return "App is Healthy";
    }

    @GetMapping("/TeamName")
    public String testCheck(){
        return "Team Name:" + teamName;
    }

}
