using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arun.Manglick.MVC.NerdDinner.Models.EDM;

namespace Arun.Manglick.MVC.NerdDinner.Models
{
    public interface IDinnerRepositoryEDM
    {
        IQueryable<Dinners> FindAllDinners();
        IQueryable<Dinners> FindByLocation(float latitude, float longitude);
        IQueryable<Dinners> FindUpcomingDinners();
        Dinners GetDinner(int id);

        void Add(Dinners dinner);
        void Delete(Dinners dinner);

        void Save();        
    }
}
