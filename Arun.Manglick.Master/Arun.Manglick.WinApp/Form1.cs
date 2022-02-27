using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Reflection;
using WindowsApplication1.MultiThread;
using WindowsApplication1.MultiInheritanceMyth;
using WindowsApplication1.PublicPrivateInterfaceInheritanceMyth;
using WindowsApplication1.UsingMyth;
using WindowsApplication1.CopyConstructor_Myth;
using WindowsApplication1.IDisposable_Myth;
using WindowsApplication1.CollectionMyth;
using WindowsApplication1.Generics;
using ShallowDeepCopyMyth;
using Abstract_Factory = WindowsApplication1.DesignPatterns.Creational.Abstract_Factory;
using Factory = WindowsApplication1.DesignPatterns.Creational.Factory;
using Bridge = WindowsApplication1.DesignPatterns.Structural.Bridge;
using Strategy = WindowsApplication1.DesignPatterns.Behavioral.Strategy;
using Facade = WindowsApplication1.DesignPatterns.Structural.Facade;
using Singleton = WindowsApplication1.DesignPatterns.Creational.Singleton;
using Observer = WindowsApplication1.DesignPatterns.Behavioral.Observer;
using System.Threading.Tasks;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        WindowsApplication1.Equality_Myth.ValueTypes objValue;
        WindowsApplication1.Equality_Myth.ReferenceTypes objReference;
        WindowsApplication1.Equality_Myth.StringTypes objString;


        public Form1()
        {
            InitializeComponent();
            objValue = new WindowsApplication1.Equality_Myth.ValueTypes();
            objReference = new WindowsApplication1.Equality_Myth.ReferenceTypes();
            objString = new WindowsApplication1.Equality_Myth.StringTypes();

        }

        #region Shadow Myth
        private void btnShadowMyth_Click(object sender, EventArgs e)
        {
            MajorBase major = new MinorBase();
            MessageBox.Show(major.getMessage());  // Major - Depends on the Type of Object

            MajorBase major1 = new MajorBase();
            MessageBox.Show(major1.getMessage()); // Major - Depends on the Type of Object
        }

        #endregion

        #region Equality Concept

        #region Value Types

        private void btnValueEquality_Click(object sender, EventArgs e)
        {
            objValue.FindEquality();
        }
        private void btnValueReference_Click(object sender, EventArgs e)
        {
            objValue.FindReferenceEquality();
        }
        private void btnValueVirtual_Click(object sender, EventArgs e)
        {
            objValue.FindVirtualEqualsEquality();
        }
        private void btnValueStatic_Click(object sender, EventArgs e)
        {
            objValue.FindStaticEqualsEquality();
        }

        #endregion

        #region Reference Types

        private void btnReferenceEquality_Click(object sender, EventArgs e)
        {
            objReference.FindEquality();
        }
        private void btnReferenceReference_Click(object sender, EventArgs e)
        {
            objReference.FindReferenceEquality();
        }
        private void btnReferenceVirtual_Click(object sender, EventArgs e)
        {
            objReference.FindVirtualEqualsEquality();
        }
        private void btnReferenceStatic_Click(object sender, EventArgs e)
        {
            objReference.FindStaticEqualsEquality();
        }

        private void btnReferenceClone_Click(object sender, EventArgs e)
        {
            objReference.MakeClone();
        }
        private void btnReferenceRemoveClone_Click(object sender, EventArgs e)
        {
            objReference.RemoveClone();
        }
        private void btnReferenceSetNull_Click(object sender, EventArgs e)
        {
            objReference.SetNull();
        }

        #endregion

        #region String Types
        private void button7_Click(object sender, EventArgs e)
        {
            objString.FindEquality();
        }
        private void btnStringReference_Click(object sender, EventArgs e)
        {
            objString.FindReferenceEquality();
        }
        private void btnrStringVirtual_Click(object sender, EventArgs e)
        {
            objString.FindVirtualEqualsEquality();
        }
        private void btnStringStatic_Click(object sender, EventArgs e)
        {
            objString.FindStaticEqualsEquality();
        }
        #endregion

        private void btnSingleton_Click(object sender, EventArgs e)
        {
            SingleTon obj = SingleTon.GetInstance();
            MessageBox.Show(obj.i.ToString());

            //SingleTon obj1 = new SingleTon();
            //MessageBox.Show(obj1.i.ToString());
        }


        #endregion

        private void btnSwap_Click(object sender, EventArgs e)
        {
            WindowsApplication1.Swap_Myth.Employee obj1 = new WindowsApplication1.Swap_Myth.Employee(5);
            WindowsApplication1.Swap_Myth.Employee obj2 = new WindowsApplication1.Swap_Myth.Employee(6);


            MessageBox.Show("Before Swap: " + obj1.i.ToString() + "," + obj2.i.ToString());
            WindowsApplication1.Swap_Myth.Swap.SwapMe(obj1, obj2);
            //WindowsApplication1.Swap_Myth.Swap.SwapMeByRef(ref obj1, ref obj2);

            MessageBox.Show("After Swap: " + obj1.i.ToString() + "," + obj2.i.ToString());

            //obj1 = new WindowsApplication1.Swap_Myth.Employee(5);
            //obj2 = new WindowsApplication1.Swap_Myth.Employee(6);
            //MessageBox.Show("Before Change: " + obj1.i.ToString() + "," + obj2.i.ToString());
            //WindowsApplication1.Swap_Myth.Swap.ChangeMe(obj1, obj2);
            //MessageBox.Show("After Change: " + obj1.i.ToString() + "," + obj2.i.ToString());

            //// For String 'ref' is required
            //string str1 = "Arun";
            //string str2 = "Manglick";
            //MessageBox.Show("Before Change: " + str1 + "," + str2);
            ////WindowsApplication1.Swap_Myth.Swap.SwapMe( str1,  str2);  // Without 'ref' will not swap 
            //WindowsApplication1.Swap_Myth.Swap.SwapMe(ref str1, ref str2); // Require 'ref' to swap 
            //MessageBox.Show("After Change: " + str1 + "," + str2);

            // For other Reference Type 'ref' is not required
            //ArrayList arr = new ArrayList();
            //arr.Add(new WindowsApplication1.Swap_Myth.Employee(5));
            //arr.Add(new WindowsApplication1.Swap_Myth.Employee(6));
            //arr.Add(new WindowsApplication1.Swap_Myth.Employee(7));
            //MessageBox.Show("Before Additon: " + arr.Count.ToString());  // Count =3
            //WindowsApplication1.Swap_Myth.Swap.AddMore(arr);  // 'ref' is not required
            //MessageBox.Show("After Additon: " + arr.Count.ToString());  // Count =4
        }

        private void btnCollectionMyth_Click(object sender, EventArgs e)
        {
            string strKey, strValue;
            Hashtable hash = new Hashtable();
            hash.Add("AA", "AAA");
            hash.Add("AB", "AAB");
            hash.Add("AC", "AAC");
            hash.Add("AD", "AAD");
            hash.Add("AE", "AAE");

            MessageBox.Show(hash.Count.ToString());
            string[] arrKeys = new string[hash.Count];

            hash.Keys.CopyTo(arrKeys, 0);
            for (int i = 0; i < arrKeys.Length; i++)
            {
                strKey = arrKeys[i];
                if (strKey.Equals("AB"))
                {
                    hash.Remove(strKey);
                }
            }

            // For each will not allow to tamper the collection while looping
            //foreach (string str in hash.Keys)
            //{
            //    if (str.Equals("AB"))
            //    {
            //        hash.Remove(str);
            //    }
            //}

            MessageBox.Show(hash.Count.ToString());
        }
        private void btnArraylistMyth_Click(object sender, EventArgs e)
        {
            ArrayList array = new ArrayList();
            array.Add("AA");
            array.Add("AB");
            array.Add("AC");
            array.Add("AD");
            array.Add("AE");

            MessageBox.Show(array.Count.ToString());

            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].Equals("AA"))
                {
                    array.Remove(array[i]);
                }
            }

            // For each will not allow to tamper the collection while looping
            //foreach (string str in array)
            //{
            //    if (str.Equals("AB"))
            //    {
            //        array.Remove(str);
            //    }
            //}

            MessageBox.Show(array.Count.ToString());
        }
        private void btnEnum_Click(object sender, EventArgs e)
        {
            CheckEnum(LogLevel.Debug);
        }

        private void CheckEnum(LogLevel loglevel)
        {
            if (loglevel == LogLevel.Debug)
            {
                MessageBox.Show(loglevel.ToString() + " : " + loglevel.GetHashCode().ToString());
            }
        }

        private void btnWithoutThreadPool_Click(object sender, EventArgs e)
        {
            WithoutThreadPool td = new WithoutThreadPool();

            for (int i = 0; i < 50; i++)
            {
                Thread t1 = new Thread(new ThreadStart(td.LongTask1));
                t1.Start();
                Thread t2 = new Thread(new ThreadStart(td.LongTask2));
                t2.Start();
            }

            Console.Read();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnShallowCopy_Click(object sender, EventArgs e)
        {
            // Intent - Assign Values of Object2 to Object1 and then change Object2 without affecting Object1
            WindowsApplication1.Swap_Myth.Employee obj1 = new WindowsApplication1.Swap_Myth.Employee(5);
            WindowsApplication1.Swap_Myth.Employee obj2 = new WindowsApplication1.Swap_Myth.Employee(6);

            obj1 = obj2;
            obj2.i = 7;
            MessageBox.Show("Shallow Copy Problem: " + obj1.i.ToString() + "," + obj2.i.ToString());

            obj1 = new WindowsApplication1.Swap_Myth.Employee(5);
            obj2 = new WindowsApplication1.Swap_Myth.Employee(6);

            obj1.i = obj2.i;
            obj2.i = 7;
            MessageBox.Show("Shallow Copy Solved: " + obj1.i.ToString() + "," + obj2.i.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowsApplication1.CLR_Profile.Code1.ProfileStringCode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WindowsApplication1.CLR_Profile.Code2.ProfileBrushCode();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowsApplication1.CLR_Profile.Code3.ProfileStreamCode();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowsApplication1.CLR_Profile.ProfileCode1.ProfileStringCode();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WindowsApplication1.CLR_Profile.ProfileCode2.ProfileBrushCode();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            WindowsApplication1.GC_Myth.Class1 obj1 = new WindowsApplication1.GC_Myth.Class1();
            WindowsApplication1.GC_Myth.Class2 obj2 = new WindowsApplication1.GC_Myth.Class2();
            WindowsApplication1.GC_Myth.Class2 temp;

            obj1.Class1Id = 5;
            obj2.Class2Id = 6;

            obj1.ObjClass2 = obj2;
            temp = obj1.ObjClass2;


            MessageBox.Show("Class 1: " + obj1.Class1Id);
            MessageBox.Show("Class 2: " + obj2.Class2Id);

            obj1 = null;

            MessageBox.Show("Class 2: " + obj1.ObjClass2.Class2Id);



        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void btn_MultiInheritanceMyth_Click(object sender, EventArgs e)
        {
            MyClass obj = new MyClass();
            MessageBox.Show(obj.GetMessage());

            InterfaceB b = new MyClass();
            MessageBox.Show(b.GetMessage());
        }

        private void btn_PublicPrivateInterfaceInheritanceMyth_Click(object sender, EventArgs e)
        {
            try
            {
                MyInterface a = new ChildClass();
                MessageBox.Show(a.GetMessage());


            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        private void btn_UsingMyth_Click(object sender, EventArgs e)
        {

            #region Approach 1
            Resource theInstance = null;
            try
            {
                theInstance = new Resource();
            }
            finally
            {
                if (theInstance != null) theInstance.Dispose();
            }

            #endregion


            #region Approach 2

            using (Resource theInstance1 = new Resource())
            {
                // Here the Dispose will be called automatically.
                // using requires the Object to implement 'IDispoasble'
            }

            #endregion
        }

        private void btn_CopyConstructorMyth_Click(object sender, EventArgs e)
        {
            // Unlike some languages, C# does not provide a copy constructor. 
            //If you create a new object and want to copy the values from an existing object, you have to write the appropriate method yourself.

            Employee emp1 = new Employee(55, "Boston");
            Employee emp2 = new Employee(emp1);

            //Employee emp2;
            //emp2 = emp1; // Unlike C++, This approach will not call the defined Copy Constructor in C#. Hence this will result in Shallow Copy

            MessageBox.Show("Before Change Employee 1' data:" + emp1.Details);
            emp2.age = 66; // Change in one will not affect another. That's tbe beauty of Copy constructor Implementtion

            MessageBox.Show("Before Change Employee 1' data: Age: " + emp1.Details);
        }

        private void btn_TrialCode_Click(object sender, EventArgs e)
        {
            try
            {
                //Active active = Active.Inactive;
                //bool res = Convert.ToBoolean(active.GetHashCode());
                //MessageBox.Show(res.ToString());

                //Object obj = null;
                //string str1 = Convert.ToString(obj);
                //string str2 = obj as string;
                //string str3 = (string)obj;
                //string str4 = obj.ToString();

                //string str = "1";
                //double d = 12.54;
                ////int i = Convert.ToInt32(str);
                //int i = (int)d;

                //MessageBox.Show(i.ToString());



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_IDisposableMyth_Click(object sender, EventArgs e)
        {
            unsafe
            {
                try
                {
                    int x = 10;
                    IntPtr ptr = new IntPtr(x);

                    //MyResource obj = new MyResource(ptr);
                    //obj.Dispose();  // Require Explicit Call

                    using (MyResource obj = new MyResource(ptr))  // This will Implicitly Call Dispose()
                    {

                    }

                    // --------------------------------------------------------
                    // Note - The below one will also call the Dispose on MyResource
                    //TestClass clsObject = new TestClass();

                    //using (MyResource obj = clsObject.GetResource())
                    //{

                    //}

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }

        private void btn_ICollection_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyICollection userList = new MyICollection();
            UserI user = new UserI();
            user.Id = "1";
            user.Fname = "AA";
            userList.Add(user);

            user = new UserI();
            user.Id = "2";
            user.Fname = "BB";
            userList.Add(user);

            user = new UserI();
            user.Id = "3";
            user.Fname = "CC";
            userList.Add(user);

            foreach (UserI myUser in userList)
            {
                str.Append("Id: ").Append(myUser.Id).Append(", Name: ").Append(myUser.Fname).Append(Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "ICollection");
        }

        private void btn_CollectionBase_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyCollectionBase userList = new MyCollectionBase();
            UserI user = new UserI();
            user.Id = "1";
            user.Fname = "AA";
            userList.Add(user);

            user = new UserI();
            user.Id = "2";
            user.Fname = "BB";
            userList.Add(user);

            user = new UserI();
            user.Id = "3";
            user.Fname = "CC";
            userList.Add(user);

            foreach (UserI myUser in userList)
            {
                str.Append("Id: ").Append(myUser.Id).Append(", Name: ").Append(myUser.Fname).Append(Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "CollectionBase");
        }

        private void btn_IEnumerable_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyIEnumerable userList = new MyIEnumerable();
            UserI user = new UserI();
            user.Id = "1";
            user.Fname = "AA";
            userList.Add(user);

            user = new UserI();
            user.Id = "2";
            user.Fname = "BB";
            userList.Add(user);

            user = new UserI();
            user.Id = "3";
            user.Fname = "CC";
            userList.Add(user);

            foreach (UserI myUser in userList)
            {
                str.Append("Id: ").Append(myUser.Id).Append(", Name: ").Append(myUser.Fname).Append(Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "IEnumerable");
        }

        private void btn_ICollectionArray_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyICollectionOnArray MyCol = new MyICollectionOnArray();

            foreach (object MyObj in MyCol)
            {
                str.Append("Item: " + MyObj.ToString() + Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "ICollection on Array");

        }

        private void btn_IEnumerableArray_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyIEnumerableOnArray MyCol = new MyIEnumerableOnArray();

            foreach (object MyObj in MyCol)
            {
                str.Append("Item: " + MyObj.ToString() + Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "IEnumerable on Array");
        }

        private void btn_Iterator_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyIterator userList = new MyIterator();

            #region Create Collection
            UserI user = new UserI();
            user.Id = "1";
            user.Fname = "AA";
            userList.Add(user);

            user = new UserI();
            user.Id = "2";
            user.Fname = "BB";
            userList.Add(user);

            user = new UserI();
            user.Id = "3";
            user.Fname = "CC";
            userList.Add(user);

            user = new UserI();
            user.Id = "4";
            user.Fname = "DD";
            userList.Add(user);

            user = new UserI();
            user.Id = "5";
            user.Fname = "EE";
            userList.Add(user);
            #endregion

            #region Default Iterator

            foreach (UserI myUser in userList)
            {
                str.Append("Id: ").Append(myUser.Id).Append(", Name: ").Append(myUser.Fname).Append(Environment.NewLine);
            }
            MessageBox.Show(str.ToString(), "Iterator");
            str.Remove(0, str.ToString().Length);

            #endregion

            #region Additional Iterators

            foreach (UserI myUser in userList.BottomToTop)
            {
                str.Append("Id: ").Append(myUser.Id).Append(", Name: ").Append(myUser.Fname).Append(Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "Iterator: BottomToTop");
            str.Remove(0, str.ToString().Length);

            foreach (UserI myUser in userList.FromToBuy(1, 3))
            {
                str.Append("Id: ").Append(myUser.Id).Append(", Name: ").Append(myUser.Fname).Append(Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "Iterator: FromToBuy(1,3)");

            #endregion

        }

        private void btn_IteratorArray_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder(5);
            MyIteratorOnArray MyCol = new MyIteratorOnArray();

            foreach (object MyObj in MyCol)
            {
                str.Append("Item: " + MyObj.ToString() + Environment.NewLine);
            }

            MessageBox.Show(str.ToString(), "Iterator on Array");
        }

        private void btn_StackTrace_Click(object sender, EventArgs e)
        {
            try
            {
                WindowsApplication1.StackTrace.Class1.ThrowClass1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Stack Trace");
                MessageBox.Show(ex.StackTrace, "Stack Trace");
            }
        }

        private void btn_InnerException_Click(object sender, EventArgs e)
        {
            WindowsApplication1.InnerException.ExceptExample testInstance = new WindowsApplication1.InnerException.ExceptExample();

            try
            {
                testInstance.CatchInner();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Message + " : " + ex.InnerException.Message);
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            MyStack<int> stack = new MyStack<int>();

            for (int i = 1; i <= 5; i++)
            {
                stack.push(i);
            }

            MessageBox.Show(stack.pop(2).ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MyDictionary<string, string> dict = new MyDictionary<string, string>();
            dict.Add("Arun1", "Manglick1");
            dict.Add("Arun2", "Manglick2");
            dict.Add("Arun3", "Manglick3");

            MessageBox.Show(dict.Pop("Arun2"));
        }

        private void btnReadAssemblyInfo_Click(object sender, EventArgs e)
        {
            AssemblyCopyrightAttribute objCopyright = (AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
            String str = objCopyright.Copyright;
            MessageBox.Show(str);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Days2 meetingDays = Days2.Tuesday | Days2.Thursday | Days2.Friday;
            Days2 meetingDays1 = Days2.Tuesday | Days2.Thursday;

            bool res1 = (meetingDays & Days2.Tuesday) == Days2.Tuesday;
            bool res2 = (meetingDays & Days2.Monday) == Days2.Thursday;
            bool res3 = (meetingDays | Days2.Tuesday | Days2.Thursday) == meetingDays1;

            if (res1) MessageBox.Show("Tuesday"); else MessageBox.Show("Falil");
            if (res2) MessageBox.Show("Thursday"); else MessageBox.Show("Falil");
            if (res3) MessageBox.Show("Tuesday & Thursday"); else MessageBox.Show("Falil");


            meetingDays = meetingDays | Days2.Friday;

            switch (meetingDays)
            {
                case Days2.Tuesday & Days2.Thursday | Days2.Friday:
                    MessageBox.Show("Hurrah");
                    break;
            }

            Active active = Active.Active;
            switch (active)
            {
                case Active.Active | Active.Inactive:
                    MessageBox.Show("Hurrah2");
                    break;
            }

            MessageBox.Show(string.Format("Meeting days are {0}", meetingDays));
            // Output: Meeting days are Tuesday, Thursday, Friday

            // Remove a flag using bitwise XOR.
            meetingDays = meetingDays ^ Days2.Tuesday;
            MessageBox.Show(string.Format("Meeting days are {0}", meetingDays));

            string s = Enum.GetName(typeof(Days2), 4);
            MessageBox.Show(s);

            //Console.WriteLine("The values of the Days Enum are:");
            //foreach (int i in Enum.GetValues(typeof(Days2)))
            //    MessageBox.Show(i.ToString());

            //Console.WriteLine("The names of the Days Enum are:");
            //foreach (string str in Enum.GetNames(typeof(Days2)))
            //    MessageBox.Show(str);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //AbstractFactory objAbstract = new ConcreteFactory();
            //string str = objAbstract.DoOperation();
            //MessageBox.Show(str);
        }

        private void btn_InheritanceMyth_Click(object sender, EventArgs e)
        {
            InheritanceMyth.MajorBase objMajor = new InheritanceMyth.MajorBase();
            MessageBox.Show(objMajor.GetMajorMessage());

            InheritanceMyth.MinorBase objMinor = new InheritanceMyth.MinorBase();
            MessageBox.Show(objMinor.GetMinorMessage());
            MessageBox.Show(objMinor.GetMajorMessage());

            //InheritanceMyth.MinorBase objMinor1 = new InheritanceMyth.MajorBase();  // Not Possible.
            InheritanceMyth.MajorBase objMajor1 = new InheritanceMyth.MinorBase();
            MessageBox.Show(objMajor1.GetMajorMessage());
            //MessageBox.Show(objMajor1.GetMinorMessage());   /// Error
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Person p1 = new Person();
            p1.Age = 33;
            p1.Name = "Arun";
            p1.IdInfo = new IdInfo(0719);

            Person p2 = (Person)p1.DeepCopy(); // Perform a shallow copy of p1 and assign it to p2.

            string str = "Original values of p1 and p2:" + Environment.NewLine;
            str += "   p1 instance values: ";
            str += DisplayValues(p1);
            str += "   p2 instance values:";
            str += DisplayValues(p2);
            MessageBox.Show(str);

            // Change the value of p1 properties and display the values of p1 and p2.
            //p1.Age = 22;
            //p1.Name = "Frank";
            //p1.IdInfo.IdNumber = 0006;
            //str += "\nValues of p1 and p2 after changes to p1:" + Environment.NewLine;
            //str += "   p1 instance values: ";
            //str += DisplayValues(p1);
            //str += "   p2 instance values:";
            //str += DisplayValues(p2);
            //MessageBox.Show(str);

            p2.Age = 22;
            p2.Name = "Frank";
            p2.IdInfo.IdNumber = 0006;
            str += "\nValues of p1 and p2 after changes to p1:" + Environment.NewLine;
            str += "   p1 instance values: ";
            str += DisplayValues(p1);
            str += "   p2 instance values:";
            str += DisplayValues(p2);
            MessageBox.Show(str);


            //Person p3 = p1.DeepCopy(); // Make a deep copy of p1 and assign it to p3.
            //// Change the members of the p1 class to new values to show the deep copy.
            //p1.Name = "George";
            //p1.Age = 39;
            //p1.IdInfo.IdNumber = 8641;
            //str += "\nValues of p1 and p3 after changes to p1:" + Environment.NewLine;
            //str += "   p1 instance values: ";
            //str += DisplayValues(p1);
            //str += "   p3 instance values:";
            //str += DisplayValues(p3);

            //MessageBox.Show(str);
        }

        public static string DisplayValues(Person p)
        {
            string str;
            str = string.Format("Name: {0:s}, Age: {1:d}", p.Name, p.Age);
            str += string.Format("      Value: {0:d}", p.IdInfo.IdNumber) + Environment.NewLine;

            return str;
        }

        #region Design Patterns

        #region Abstract Factory

        private void btnAbsFactory_Click(object sender, EventArgs e)
        {
            GetFromBuilder(new Abstract_Factory.ConcreteFactory_BuilderHighStyle());
            GetFromBuilder(new Abstract_Factory.ConcreteFactory_BuilderMediumStyle());
            GetFromBuilder(new Abstract_Factory.ConcreteFactory_BuilderLowStyle());
        }

        public void GetFromBuilder(Abstract_Factory.AbstractFactory_Builder objBuilder)
        {
            Abstract_Factory.AbstractProduct_Home objHome = objBuilder.ProvideHome();
            Abstract_Factory.AbstractProduct_Car objCar = objBuilder.GiftCar();

            MessageBox.Show(objHome.ShowHomeDetails() + "\n" + objCar.ShowCarDetails());
        }

        #endregion  

        #region Factory

        private void btnFactory_Click(object sender, EventArgs e)
        {
            GetFromBuilder1(new Factory.ConcreteFactory_BuilderHighStyle());
            GetFromBuilder1(new Factory.ConcreteFactory_BuilderMediumStyle());
            GetFromBuilder1(new Factory.ConcreteFactory_BuilderLowStyle());
        }

        public void GetFromBuilder1(Factory.Factory_Builder objBuilder)
        {
            Factory.Product_Home objHome = objBuilder.ProvideHome();
            MessageBox.Show(objHome.ShowHomeDetails());
        }

        #endregion

        #region Bridge

        private void btnBridge_Click(object sender, EventArgs e)
        {
            // Define Product
            Bridge.Product_Home objHome = new Bridge.Refined_Product_Home();

            // Decide Implementor  - High Style       
            Bridge.Implementor_Builder objImplementor;
            objImplementor = new Bridge.ConcreteImplementor_BuilderHighStyle();
            objHome.Implementor = objImplementor;
            MessageBox.Show(objHome.ShowHomeDetails());

            // Decide Implementor  - Medium Style
            objImplementor = new Bridge.ConcreteImplementor_BuilderMediumStyle();
            objHome.Implementor = objImplementor;
            MessageBox.Show(objHome.ShowHomeDetails());

            // Decide Implementor  - Low Style
            objImplementor = new Bridge.ConcreteImplementor_BuilderLowStyle();
            objHome.Implementor = objImplementor;
            MessageBox.Show(objHome.ShowHomeDetails());
        }

        #endregion

        #region Strategy
        private void button13_Click(object sender, EventArgs e)
        {
            // Define Product
            Strategy.Product_Home objHome = new Strategy.Product_Home();

            // Decide Strategy  - High Style       
            Strategy.Strategy_Builder objStrategy;
            objStrategy = new Strategy.ConcreteStrategy_BuilderHighStyle();
            objHome.Strategy = objStrategy;
            MessageBox.Show(objHome.ShowHomeDetails());

            // Decide Strategy  - Medium Style
            objStrategy = new Strategy.ConcreteStrategy_BuilderMediumStyle();
            objHome.Strategy = objStrategy;
            MessageBox.Show(objHome.ShowHomeDetails());

            // Decide Strategy  - Low Style
            objStrategy = new Strategy.ConcreteStrategy_BuilderLowStyle();
            objHome.Strategy = objStrategy;
            MessageBox.Show(objHome.ShowHomeDetails());
        }

        #endregion

        #region Facade
        private void btnFacade_Click(object sender, EventArgs e)
        {
            Facade.Facade objFacade = new Facade.Facade();

            MessageBox.Show("Build Multi City..." + "\n\n" + objFacade.BuildMetroCity());
            MessageBox.Show("Build Urban City..." + "\n\n" + objFacade.BuildUrbanCity());
        }

        #endregion

        #region Singleton
        private void btnSingletonNew_Click(object sender, EventArgs e)
        {
            Singleton.Singleton_Lazy objLazy = Singleton.Singleton_Lazy.GetInstance();
            MessageBox.Show("Singleton_Lazy : " + objLazy.i.ToString());

            Singleton.Singleton_Lock objLock = Singleton.Singleton_Lock.GetInstance();
            MessageBox.Show("Singleton_Lock : " + objLock.i.ToString());

            Singleton.Singleton_Volatile objVolatile = Singleton.Singleton_Volatile.GetInstance();
            MessageBox.Show("Singleton_Volatile : " + objVolatile.i.ToString());

            Singleton.SingleTon_ThreadSafe objThreadSafe = Singleton.SingleTon_ThreadSafe.GetInstance();
            MessageBox.Show("SingleTon_ThreadSafe : " + objThreadSafe.i.ToString());
        }
        #endregion

        #region Observer

        private void btnObserver_Click(object sender, EventArgs e)
        {
            Observer.ConcreteSubject s = new Observer.ConcreteSubject();
            s.IntrestRate = 1.2;

            s.Attach(new Observer.ConcreteObserver_Bank_SBI(s));
            s.Attach(new Observer.ConcreteObserver_Bank_HDFC(s));
            s.Attach(new Observer.ConcreteObserver_Bank_ICICI(s));

            s.IntrestRate = 2.4;
            s.Notify();
        }

        #endregion

        #endregion

        private async void StartButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("StartButton_Click Starts..AccessTheWebAsync...");
            Task<int> doSeomethingAsync = AccessTheWebAsync();

            Console.WriteLine();
            Console.WriteLine("StartButton_Click Resumes .. Only after AccessTheWebAsync Wait Starts...");
            // Independent work which doesn't need the result of LongRunningOperationAsync can be done here
            DoIndependenctWork("StartButton_Click..");
            Console.WriteLine("StartButton_Click Done. .Now Waiting For AccessTheWebAsync to finish.");
            int result = await doSeomethingAsync;
            Console.WriteLine("StartButton_Click Wait Finishes..");
            Console.WriteLine("StartButton_Click Finishing Result.." + result);
        }

        // Three things to note in the signature:  
        //  - The method has an async modifier.   
        //  - The return type is Task or Task<T>. (See "Return Types" section.)  
        //  - The method name ends in "Async."  
        public async Task<int> AccessTheWebAsync()
        {
            Console.WriteLine("AccessTheWebAsync Start...GetStringAsync ");
            Task<int> longRunningTask = GetStringAsync();

            Console.WriteLine();
            Console.WriteLine("AccessTheWebAsync Immediately Resume..");
            // Independent work which doesn't need the result of LongRunningOperationAsync can be done here
            DoIndependenctWork("AccessTheWebAsync..");
            Console.WriteLine("AccessTheWebAsync Done..Waiting For GetStringAsync to finish.. by the time return back to 'StartButton_Click'..");

            int result = await longRunningTask;
            Console.WriteLine("AccessTheWebAsync Wait Finishes..");
            Console.WriteLine("AccessTheWebAsync Finishing Result.." + result);
            return 11;
        }

        public async Task<int> GetStringAsync() // assume we return an int from this long running operation 
        {
            Console.WriteLine("...GetStringAsync Starts");
            await Task.Delay(5000); // Just 5 second delay
            Console.WriteLine();
            Console.WriteLine("...GetStringAsync Done");
            return 2;
        }

        private void DoIndependenctWork(string work)
        {
            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine(work + "Doing something since: " + i);
            }
        }
    }
}