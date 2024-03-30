package guides.hazelcast.springboot;

public class HazelResponse {

    private String value;

    public HazelResponse(String value) {
        this.value  = value;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }
}
