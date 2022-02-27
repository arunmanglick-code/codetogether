using System;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WindowsApplication1.CollectionMyth
{

    /// <summary>
    /// Summary description for MyICollection
    /// </summary>
    public class MyICollectionOnArray : ICollection
    {
        private int[] intArr = { 1, 5, 9 };
        private int Ct;
        
        public MyICollectionOnArray()
        {
            Ct = 3;
        }

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (int i in intArr)
            {
                array.SetValue(i, index);
                index = index + 1;
            }

        }

        int ICollection.Count
        {
            get
            {
                return Ct;                
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(intArr);
        }

        #endregion
        
    }

    public class Enumerator : IEnumerator
    {
        private int[] intArr;
        private int Cursor;

        public Enumerator(int[] intarr)
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
