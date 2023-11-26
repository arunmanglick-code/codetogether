package arun.manglick.java.amapachecamelrouting.routes;

import org.apache.camel.Message;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.stereotype.Component;

@Component
public class MultiCastRoute1 extends RouteBuilder {
    @Override
    public void configure() throws Exception {
        onException(NullPointerException.class)
//                .log("Received NullPointerException in MultiCastRoute1")
                .redeliveryDelay(1000)
                .maximumRedeliveries(3)
                .log("Retried Processing 3 Times In MultiCastRoute1 Before Raising Exception")
                .process(exchange -> {
                    Message message = exchange.getIn();
                    System.out.println("Process NullPointerException in MultiCastRoute1");
                    System.out.println("Print Header: " + message.getHeader("channelName"));
                 }).handled(true);

        onException(Exception.class)
                .log("Received Generic Exception in MultiCastRoute1");

        from("direct://multiCastRoute1")
                .log("Enter Inside Multicast Child Route 1")
                .errorHandler(noErrorHandler())     // This is used when you want exception to be handled in parent route
                .process(exchange -> {
                    Message message = exchange.getIn();
                    System.out.println("Inside MultiCastRoute1 Processor");
                    System.out.println("Print Header: " + message.getHeader("channelName"));
                    throw new NullPointerException("Exception Raised from MultiCastRoute1");
                })
                .end();
    }
}
