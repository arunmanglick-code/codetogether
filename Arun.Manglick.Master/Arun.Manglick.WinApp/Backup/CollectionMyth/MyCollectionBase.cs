using System;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WindowsApplication1.CollectionMyth
{
    /// <summary>
    /// Summary description for MyCollectionBase
    /// </summary>
    public class MyCollectionBase : CollectionBase
    {
        public MyCollectionBase()
        { }

        public int Add(UserI objUser)
        {
            return List.Add(objUser);
        }

        public void Remove(UserI objUser)
        {
            List.Remove(objUser);
        }

        //This is defined by me.
        public UserI Item(int i)
        {
            return List[i] as  UserI;
        }
    }
}
