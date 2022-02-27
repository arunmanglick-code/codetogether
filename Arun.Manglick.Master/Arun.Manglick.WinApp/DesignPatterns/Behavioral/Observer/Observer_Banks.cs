using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.DesignPatterns.Behavioral.Observer
{
    public abstract class Observer_Banks
    {        
        public abstract void UpdateObserverState();
    }

    public class ConcreteObserver_Bank_SBI : Observer_Banks
    {
        private double observerState;
        private ConcreteSubject objSubject; 

        public ConcreteObserver_Bank_SBI(ConcreteSubject subject)
        {
            this.objSubject = subject;
        }

        public override void UpdateObserverState()
        {
            this.observerState = this.objSubject.IntrestRate;
            MessageBox.Show("SBI Interest Rate :" + observerState);
        }
    }

    public class ConcreteObserver_Bank_ICICI : Observer_Banks
    {
        private double observerState;
        private ConcreteSubject objSubject;

        public ConcreteObserver_Bank_ICICI(ConcreteSubject subject)
        {
            this.objSubject = subject;
        }

        public override void UpdateObserverState()
        {
            this.observerState = this.objSubject.IntrestRate;
            MessageBox.Show("ICICI Interest Rate :" + observerState);
        }
    }

    public class ConcreteObserver_Bank_HDFC : Observer_Banks
    {
        private double observerState;
        private ConcreteSubject objSubject;

        public ConcreteObserver_Bank_HDFC(ConcreteSubject subject)
        {
            this.objSubject = subject;
        }

        public override void UpdateObserverState()
        {
            this.observerState = this.objSubject.IntrestRate;
            MessageBox.Show("HDFC Interest Rate :" + observerState);
        }
    }
}
