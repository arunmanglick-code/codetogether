package arun.manglick.java.amapachecamelrouting.routes;

import org.apache.camel.Message;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.stereotype.Component;

@Component
public class MultiCastRoute3 extends RouteBuilder {

    @Override
    public void configure() throws Exception {
        from("direct://multiCastRoute3")
                .log("Enter Inside Multicast Child Route 3")
                .process(exchange -> {
                    Message message = exchange.getIn();
                    System.out.println("Inside MultiCastRoute3 Processor");
                    System.out.println("Print Header: " + message.getHeader("channelName"));
                })
                .end();
    }
}
