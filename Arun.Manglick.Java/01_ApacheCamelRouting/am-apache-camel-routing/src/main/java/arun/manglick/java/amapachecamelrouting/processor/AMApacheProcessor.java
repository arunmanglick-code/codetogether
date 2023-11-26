package arun.manglick.java.amapachecamelrouting.processor;

import org.apache.camel.Exchange;
import org.apache.camel.Message;
import org.apache.camel.Processor;
import org.springframework.stereotype.Component;

@Component
public class AMApacheProcessor implements Processor {
    @Override
    public void process(Exchange exchange) throws Exception {
        Message message = exchange.getIn();
        System.out.println("Enter AMApacheProcessor");
        System.out.println("Print Header: " + message.getHeader("channelName"));
        String exchangeProperty = exchange.getProperty("propName", String.class);
        System.out.println("Print Property: " + exchangeProperty);

        // Return Back to Router setting new values after processing
        message.setHeader("processedChannelName", "AM Processed Channel");
        exchange.setIn(message);
        System.out.println("Exit AMApacheProcessor");
    }

    public void processExchangeData(final Exchange exchange){
        System.out.println("Enter Bean ProcessExchangeData");;
        exchange.setProperty("amPropName", "FirstName");
    }
}
