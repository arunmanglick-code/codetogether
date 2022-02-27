using System;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WindowsApplication1.CollectionMyth
{

    /// <summary>
    /// Summary description for MyICollection
    /// </summary>
    public class MyIteratorOnArray
    {
        private int[] intArr = { 1, 5, 9 };
        private int Ct;

        public MyIteratorOnArray()
        {
            Ct = 3;
        }

        public IEnumerator GetEnumerator()
        {
            //return new MyIteratorEnumerator(intArr);
            yield return new MyIteratorEnumerator(intArr);
        }

        
    }

    public class MyIteratorEnumerator : IEnumerator
    {
        private int[] intArr;
        private int Cursor;

        public MyIteratorEnumerator(int[] intarr)
        {
            this.intArr = intarr;
            Cursor = -1;

        }

        void IEnumerator.Reset()
        {
            Cursor = -1;
        }

        bool IEnumerator.MoveNext()
        {
            if (Cursor < intArr.Length)
            {
                Cursor++;
            }

            return (!(Cursor == intArr.Length));
        }

        object IEnumerator.Current
        {
            get
            {
                if ((Cursor < 0) || (Cursor == intArr.Length))
                {
                    throw new InvalidOperationException();
                }

                return intArr[Cursor];
            }
        }

    }

}
