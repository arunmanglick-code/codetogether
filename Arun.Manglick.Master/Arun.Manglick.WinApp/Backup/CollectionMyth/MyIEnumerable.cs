using System;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WindowsApplication1.CollectionMyth
{

    /// <summary>
    /// Summary description for MyICollection
    /// </summary>
    public class MyIEnumerable : IEnumerable
    {
        private ArrayList ArrayListCollection;

        public MyIEnumerable()
        {
            ArrayListCollection = new ArrayList();
        }               

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ArrayListCollection.GetEnumerator();
        }

        #endregion

        #region Custom Methods

        public int Add(UserI ObjUser)
        {
            return ArrayListCollection.Add(ObjUser);
        }

        public void Remove(UserI ObjUser)
        {
            this.ArrayListCollection.Remove(ObjUser);
        }

        public UserI Item(int id)
        {
            return ArrayListCollection[id] as UserI;
        }

        public bool Contains(int AuId)
        {
            foreach (UserI ObjUser in this.ArrayListCollection)
            {
                if (System.Convert.ToInt32(ObjUser.Id) == AuId)
                {
                    return true;
                }
            }
            return false;
        }

        public UserI GetItemByValue(int AuId)
        {
            foreach (UserI ObjUser in this.ArrayListCollection)
            {
                if (System.Convert.ToInt32(ObjUser.Id) == AuId)
                {
                    return ObjUser;
                }
            }
            return null;
        }

        #endregion
    }
}
