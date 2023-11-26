package arun.manglick.java.amapachecamelrouting.routes;

import arun.manglick.java.amapachecamelrouting.processor.AMApacheProcessor;
import org.apache.camel.Exchange;
import org.apache.camel.Message;
import org.apache.camel.Processor;
import org.apache.camel.builder.RouteBuilder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

@Component
public class TestApacheRetryRouter extends RouteBuilder {

    @Autowired
    private AMApacheProcessor amApacheProcessor;

    @Override
    public void configure() throws Exception {
       from("timer:amTimer?repeatCount=1")
               .log("Enter AM Router")
               .setHeader("channelName", constant("AM Channel"))
               .setProperty("propName", constant("BirthDate"))
               .process(amApacheProcessor)
               .log("${in.headers.processedChannelName}")
               .bean(amApacheProcessor, "processExchangeData")
               .end();
    }
}
