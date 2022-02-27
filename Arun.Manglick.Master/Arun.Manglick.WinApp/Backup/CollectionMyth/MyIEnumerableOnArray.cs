using System;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WindowsApplication1.CollectionMyth
{

    /// <summary>
    /// Summary description for MyICollection
    /// </summary>
    public class MyIEnumerableOnArray : IEnumerable
    {
        private int[] intArr = { 1, 5, 9 };
        private int Ct;

        public MyIEnumerableOnArray()
        {
            Ct = 3;
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MyEnumerator(intArr);
        }

        #endregion
        
    }

    public class MyEnumerator : IEnumerator
    {
        private int[] intArr;
        private int Cursor;

        public MyEnumerator(int[] intarr)
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
