using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using Arun.Manglick.SL.Services.Binding.PollingDuplex;

namespace Arun.Manglick.SL.Services.Factories.Binding.Duplex
{
    public class PollingDuplexServiceHostFactory : ServiceHostFactoryBase
    {
        public override ServiceHostBase CreateServiceHost(string constructorString,
            Uri[] baseAddresses)
        {
            return new PollingDuplexServiceHost(baseAddresses);
        }
    }

    class PollingDuplexServiceHost : ServiceHost
    {
        public PollingDuplexServiceHost(params System.Uri[] addresses)
        {
            base.InitializeDescription(typeof(HelloIndigoPollingDuplexSvc), new UriSchemeKeyedCollection(addresses));
        }

        protected override void InitializeRuntime()
        {
            // Define the binding and set time-outs
            PollingDuplexBindingElement bindingElement = new PollingDuplexBindingElement()
            {
                ServerPollTimeout = TimeSpan.FromSeconds(10),
                InactivityTimeout = TimeSpan.FromMinutes(1)
            };

            // Add an endpoint for the given service contract
            this.AddServiceEndpoint(
                typeof(IHelloIndigoPollingDuplexSvc),
                new CustomBinding(
                    bindingElement,
                    new TextMessageEncodingBindingElement(
                        MessageVersion.Soap11,
                        System.Text.Encoding.UTF8),
                    new HttpTransportBindingElement()),
                    "");

            base.InitializeRuntime();
        }
    }
}