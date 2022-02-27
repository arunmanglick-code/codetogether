using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.Generics
{
    public class MyStack<T>
    {
        T[] items = new T[5];
        int count;

        public void push(T obj)
        {
            items[count++] = obj;
        }
        public T pop(T obj)
        {
            return items[--count] ;
        }
    }

    public class MyDictionary<K, V>
    {
        Dictionary<K, V> dict = new Dictionary<K, V>();
        public void Add(K key, V value) 
        {
            dict[key] = value;
        }

        public V Pop(K key)
        {
            return dict[key];
        }
    }

}
