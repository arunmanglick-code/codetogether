package arun.manglick.java.amapachecamelrouting.routes;

import org.apache.camel.Message;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.stereotype.Component;

@Component
public class MultiCastRoute2 extends RouteBuilder {

    @Override
    public void configure() throws Exception {
        from("direct://multiCastRoute2")
                .log("Enter Inside Multicast Child Route 2")
                .process(exchange -> {
                    Message message = exchange.getIn();
                    System.out.println("Inside MultiCastRoute2 Processor");
                    System.out.println("Print Header: " + message.getHeader("channelName"));
                })
                .end();
    }
}
