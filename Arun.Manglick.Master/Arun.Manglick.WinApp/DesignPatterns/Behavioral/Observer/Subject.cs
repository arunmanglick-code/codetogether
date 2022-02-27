using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.DesignPatterns.Behavioral.Observer
{
    public abstract class Subject
    {
        private List<Observer_Banks> _observers = new List<Observer_Banks>();

        public void Attach(Observer_Banks observer)
        {
            _observers.Add(observer);
        }

        public void Detach(Observer_Banks observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (Observer_Banks o in _observers)
            {
                o.UpdateObserverState();
            }
        }
    }

    public class ConcreteSubject : Subject
    {
        private double _intrestRate;

        // Gets or sets subject state
        public double IntrestRate
        {
            get { return _intrestRate; }
            set { _intrestRate = value; }
        }
    }
}
