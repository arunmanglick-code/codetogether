using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arun.Manglick.MVC.NerdDinner.Models
{
    public interface IDinnerRepository
    {
        IQueryable<Dinner> FindAllDinners();
        IQueryable<Dinner> FindByLocation(float latitude, float longitude);
        IQueryable<Dinner> FindUpcomingDinners();
        Dinner GetDinner(int id);

        void Add(Dinner dinner);
        void Delete(Dinner dinner);

        void Save();        
    }
}
