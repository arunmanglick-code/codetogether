using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace WindowsApplication1.CollectionMyth
{

    /// <summary>
    /// Summary description for MyICollection
    /// </summary>
    public class MyIterator
    {
        private List<UserI> listCollection;

        public MyIterator()
        {
            listCollection = new List<UserI>();
        }

        #region Default Iterator

        public IEnumerator GetEnumerator()
        {
            foreach (UserI user in listCollection)
            {
                yield return user;
            }
        } 

        #endregion

        #region Additonal Iterators

        public IEnumerable<UserI> BottomToTop
        {
            get
            {
                int count = listCollection.Count - 1;

                for (int i = count; i >= 0; i--)
                {
                    yield return listCollection[i];
                }
            }
        }

        public IEnumerable<UserI> FromToBuy(int from, int to)
        {
            for (int i = from -1; i < to; i++)
            {
                yield return listCollection[i];
            }
        }

        #endregion

        #region Custom Methods

        public void Add(UserI ObjUser)
        {
            listCollection.Add(ObjUser);
        }

        public void Remove(UserI ObjUser)
        {
            this.listCollection.Remove(ObjUser);
        }

        public UserI Item(int id)
        {
            return listCollection[id] as UserI;
        }

        public bool Contains(int AuId)
        {
            foreach (UserI ObjUser in this.listCollection)
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
            foreach (UserI ObjUser in this.listCollection)
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
