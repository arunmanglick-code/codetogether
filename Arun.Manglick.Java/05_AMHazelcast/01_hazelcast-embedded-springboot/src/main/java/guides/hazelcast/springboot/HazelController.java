package guides.hazelcast.springboot;

import com.hazelcast.core.HazelcastInstance;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.concurrent.ConcurrentMap;

@RestController
public class HazelController {
    @Autowired
    private HazelcastInstance hazelcastInstance;

    private ConcurrentMap<String,String> hazelMap() {
        return hazelcastInstance.getMap("map");
    }

    @PostMapping("/addValue")
    public HazelResponse addToHazelMap(@RequestParam(value = "key") String key, @RequestParam(value = "value") String value) {
        hazelMap().put(key, value);
        return new HazelResponse(value);
    }

    @GetMapping("/getValue")
    public HazelResponse getFromHazelMap(@RequestParam(value = "key") String key) {
        String value = hazelMap().get(key);
        return new HazelResponse(value);
    }
}
