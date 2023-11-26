package arun.manglick.java.amapachecamelrouting.routes;

import org.apache.camel.Exchange;
import org.apache.camel.Message;
import org.apache.camel.Processor;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.stereotype.Component;

@Component
public class MultiCastRouter extends RouteBuilder {

    @Override
    public void configure() throws Exception {
        onException(Exception.class)
                .log("Handle Generic Exception in Parent Route")
                .process(exchange -> {
                    Message message = exchange.getIn();
                    System.out.println("Processed NullPointerException in Parent Route");
                    System.out.println("Print Header: " + message.getHeader("channelName"));
                }).handled(true);

        from("timer://parentRoute?repeatCount=1")
                .log("Enter Inside Multicast Parent Router")
                .setHeader("channelName", constant("AM Multicast Channel"))
                .multicast()
                .parallelProcessing()
                .onPrepare(new Processor() {
                    @Override
                    public void process(Exchange exchange) throws Exception {
                        System.out.println("Preparing Deep copy of Exchange");
                    }
                })
                .to("direct://multiCastRoute1", "direct://multiCastRoute2","direct://multiCastRoute3")
                .end();
    }
}
