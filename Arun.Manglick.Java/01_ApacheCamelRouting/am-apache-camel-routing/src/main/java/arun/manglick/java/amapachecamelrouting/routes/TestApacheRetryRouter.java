package arun.manglick.java.amapachecamelrouting.routes;

import org.apache.camel.Exchange;
import org.apache.camel.Message;
import org.apache.camel.Processor;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.stereotype.Component;

@Component
public class TestApacheRetryRouter extends RouteBuilder {

    @Override
    public void configure() throws Exception {
       from("timer:amTimer?repeatCount=1")
               .log("This is AM Router Running")
//               .process(new Processor() {
//                   @Override
//                   public void process(Exchange exchange) throws Exception {
//                       Message msg= exchange.getIn();
//                   }
//               })
               .end();
    }
}
