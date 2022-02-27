using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Arun.Manglick.MVC.NerdDinner.Models.EDM;
using Arun.Manglick.MVC.NerdDinner.Models;


namespace Arun.Manglick.MVC.NerdDinner.Controllers
{
    public class DinnersEDMController : Controller
    {
        DinnerRepositoryEDM edmRepository;
        public DinnersEDMController()
        {
            edmRepository = new DinnerRepositoryEDM();
        }

        #region Index/Details

        // GET: /DinnersEDM/
        public ActionResult Index()
        {
            ViewData.Model = edmRepository.FindAllDinners();
            return View();
        }

        //
        // GET: /DinnersEDM/Details/5
        public ActionResult Details(int id)
        {
            Dinners dinner = edmRepository.GetDinner(id);
            return View(dinner);
        }

        #endregion

        #region Create

        //
        // GET: /DinnersEDM/Create
        public ActionResult Create()
        {
            Dinners dinner = new Dinners();
            dinner.Address = "Address1";
            dinner.Country = "India";
            dinner.Description = "Indian Dinner";
            dinner.EventDate = DateTime.Now.AddDays(7);
            dinner.HostedBy = "Arun M";
            dinner.Title = "Indian Title";

            return View(dinner);
        } 

        //
        // POST: /DinnersEDM/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection formCollection)
        {
            Dinners dinner = new Dinners();
            try
            {
                //UpdateModel(dinner); // Does not Work
                TryUpdateModel(dinner, formCollection.ToValueProvider());
                if (ModelState.IsValid)
                {
                    edmRepository.Add(dinner);
                    edmRepository.Save();
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Edit

        //
        // GET: /DinnersEDM/Edit/5 
        public ActionResult Edit(int id)
        {
            Dinners dinner = edmRepository.GetDinner(id);
            return View(dinner);  // OR ViewData.Model = dinner;return View();
        }

        //
        // POST: /DinnersEDM/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Dinners dinner = null;
            try
            {
                dinner = edmRepository.GetDinner(id);
                //UpdateModel(dinner); // Does not Work
                TryUpdateModel(dinner, formCollection.ToValueProvider());
                edmRepository.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Delete

        // GET: /Dinners/Delete/5 
        public ActionResult Delete(int id)
        {
            Dinners dinner = edmRepository.GetDinner(id);

            if (dinner == null)
                return View("NotFound");

            return View(dinner);
        }

        // POST: /Dinners/Delete/5 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, string confirmButton)
        {
            Dinners dinner = null;
            try
            {
                dinner = edmRepository.GetDinner(id);

                if (dinner == null)
                    return View("NotFound");

                edmRepository.Delete(dinner);
                edmRepository.Save();

                return View("Deleted");
            }
            catch
            {
                return View();
            }

        }

        #endregion
    }
}
