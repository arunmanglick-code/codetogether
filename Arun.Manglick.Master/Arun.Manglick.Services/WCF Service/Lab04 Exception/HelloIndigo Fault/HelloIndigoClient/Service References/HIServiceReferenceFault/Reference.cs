﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HelloIndigoClient.HIServiceReferenceFault {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.thatindigogirl.com/samples/2006/06", ConfigurationName="HIServiceReferenceFault.IHelloIndigoService")]
    public interface IHelloIndigoService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowSimpleFaul" +
            "t", ReplyAction="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowSimpleFaul" +
            "tResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.InvalidOperationException), Action="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowSimpleFaul" +
            "tInvalidOperationExceptionFault", Name="InvalidOperationException", Namespace="http://schemas.datacontract.org/2004/07/System")]
        void ThrowSimpleFault();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowFaultExcep" +
            "tion", ReplyAction="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowFaultExcep" +
            "tionResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.InvalidOperationException), Action="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowFaultExcep" +
            "tionInvalidOperationExceptionFault", Name="InvalidOperationException", Namespace="http://schemas.datacontract.org/2004/07/System")]
        void ThrowFaultException();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowMessageFau" +
            "lt", ReplyAction="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowMessageFau" +
            "ltResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.InvalidOperationException), Action="http://www.thatindigogirl.com/samples/2006/06/IHelloIndigoService/ThrowMessageFau" +
            "ltInvalidOperationExceptionFault", Name="InvalidOperationException", Namespace="http://schemas.datacontract.org/2004/07/System")]
        void ThrowMessageFault();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IHelloIndigoServiceChannel : HelloIndigoClient.HIServiceReferenceFault.IHelloIndigoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HelloIndigoServiceClient : System.ServiceModel.ClientBase<HelloIndigoClient.HIServiceReferenceFault.IHelloIndigoService>, HelloIndigoClient.HIServiceReferenceFault.IHelloIndigoService {
        
        public HelloIndigoServiceClient() {
        }
        
        public HelloIndigoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public HelloIndigoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HelloIndigoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HelloIndigoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void ThrowSimpleFault() {
            base.Channel.ThrowSimpleFault();
        }
        
        public void ThrowFaultException() {
            base.Channel.ThrowFaultException();
        }
        
        public void ThrowMessageFault() {
            base.Channel.ThrowMessageFault();
        }
    }
}