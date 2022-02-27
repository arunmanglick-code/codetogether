using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arun.Manglick.Business
{
    public class PersonFactory
    {
        public List<Person> GetPersonList()
        {
            List<Person> personList = new List<Person>
            {
                new Person {PersonId=1, FName="David", LName = "Scott" },
                new Person {PersonId=2, FName="Goerge", LName = "Skip" },
                new Person {PersonId=3, FName="Peter", LName = "Parker" },
            };

            return personList;
        }
    }
}
