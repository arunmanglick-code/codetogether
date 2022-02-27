using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arun.Manglick.MVC.NerdDinner.Models.EDM;

namespace Arun.Manglick.MVC.NerdDinner.Models
{
    public class DinnerRepositoryEDM : IDinnerRepositoryEDM
    {
        NerdDinnerEntities edm = new NerdDinnerEntities();
        
        #region IDinnerRepository Members

        public IQueryable<Dinners> FindAllDinners()
        {
            return edm.Dinners;
        }

        public IQueryable<Dinners> FindByLocation(float latitude, float longitude)
        {
            //IQueryable<Dinners> nearestDinner = from dinner in FindUpcomingDinners()
            //                        join nearDinner in edm.NearestDinners(latitude, longitude)
            //                        on dinner.DinnerID equals nearDinner.DinnerID
            //                        select dinner;

            //return nearestDinner;
            return null;
        }

        public IQueryable<Dinners> FindUpcomingDinners()
        {
            IQueryable<Dinners> dinners = from dinner in edm.Dinners
                                         where dinner.EventDate > DateTime.Now
                                         orderby dinner.DinnerID
                                         select dinner;

            return dinners;                                        
                                         
        }

        public Dinners GetDinner(int id)
        {
           Dinners dinner = edm.Dinners.First(d => d.DinnerID == id);
           return dinner;
        }

        public void Add(Dinners dinner)
        {
            // context.Dinners.InsertOnSubmit(dinner);
            edm.AddToDinners(dinner);
        }

        public void Delete(Dinners dinner)
        {
            //context.RSVPs.DeleteAllOnSubmit(dinner.RSVPs);
            //context.Dinners.DeleteOnSubmit(dinner);

            edm.DeleteObject(dinner);
        }

        public void Save()
        {
            //context.SubmitChanges();
            edm.SaveChanges();
        }

        #endregion
    }
}
