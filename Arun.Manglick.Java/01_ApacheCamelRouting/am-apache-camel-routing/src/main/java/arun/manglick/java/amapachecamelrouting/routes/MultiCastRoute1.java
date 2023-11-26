package arun.manglick.java.amapachecamelrouting.routes;

import org.apache.camel.Message;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.stereotype.Component;

@Component
public class MultiCastRoute1 extends RouteBuilder {
    @Override
    public void configure() throws Exception {
        from("direct://multiCastRoute1")
                .log("Enter Inside Multicast Child Route 1")
                .process(exchange -> {
                    Message message = exchange.getIn();
                    System.out.println("Inside MultiCastRoute1 Processor");
                    System.out.println("Print Header: " + message.getHeader("channelName"));
                })
                .end();
    }
}
