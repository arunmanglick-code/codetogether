using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arun.Manglick.MVC.NerdDinner.Models
{
    public class DinnerRepository : IDinnerRepository
    {
        NerdDinnerDataContext context = new NerdDinnerDataContext();
        
        #region IDinnerRepository Members

        public IQueryable<Dinner> FindAllDinners()
        {
            return context.Dinners;
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            IQueryable<Dinner> nearestDinner = from dinner in FindUpcomingDinners()
                                    join nearDinner in context.NearestDinners(latitude, longitude)
                                    on dinner.DinnerID equals nearDinner.DinnerID
                                    select dinner;

            return nearestDinner;
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            IQueryable<Dinner> dinners = from dinner in context.Dinners
                                         where dinner.EventDate > DateTime.Now
                                         orderby dinner.DinnerID
                                         select dinner;

            return dinners;                                        
                                         
        }

        public Dinner GetDinner(int id)
        {
           Dinner dinner = context.Dinners.SingleOrDefault(d => d.DinnerID == id);
           return dinner;
        }

        public void Add(Dinner dinner)
        {
            context.Dinners.InsertOnSubmit(dinner);
        }

        public void Delete(Dinner dinner)
        {
            context.RSVPs.DeleteAllOnSubmit(dinner.RSVPs);
            context.Dinners.DeleteOnSubmit(dinner);
        }

        public void Save()
        {
            context.SubmitChanges();
        }

        #endregion
    }
}
