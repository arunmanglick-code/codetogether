using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Arun.Manglick.MVC.NerdDinner.Models;
using Arun.Manglick.MVC.NerdDinner.Helpers;
using System.Web.Configuration;

namespace Arun.Manglick.MVC.NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        Models.DinnerRepository dinnerRepository = new Models.DinnerRepository();
        private const int pageSize = 5;

        #region Index/Details

        // GET: /Dinners/
        //public ActionResult Index()
        //{
        //    IEnumerable<Dinner> dinners = dinnerRepository.FindUpcomingDinners().ToList();
        //    return View("Index", dinners);
        //}

        // GET: /Dinners/
        // GET: /Dinners/?pageNumber=0
        // GET: /Dinners/?pageNumber=1
        //public ActionResult Index(int? pageNumber)
        //{
        //    IQueryable<Dinner> dinners = dinnerRepository.FindUpcomingDinners();
        //    var paginatedDinners = dinners.Skip((pageNumber ?? 0) * pageSize).Take(pageSize).ToList();
        //    return View("Index", paginatedDinners);
        //}


        // GET: /Dinners/
        // GET: /Dinners/?pageNumber=0
        // GET: /Dinners/?pageNumber=1
        public ActionResult Index(int? pageNumber)  // It is mandatroy to use the same name - 'pageNumber'
        {
            IQueryable<Dinner> dinners = dinnerRepository.FindUpcomingDinners();
            PaginationList<Dinner> paginatedDinners = new PaginationList<Dinner>(dinners, pageNumber ?? 0, pageSize);

            
            

            return View("Index", paginatedDinners);
        }

        // GET: /Dinners/Details/5
        public ActionResult Details(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            if (dinner != null)
            {
                return View("Details", dinner);
            }
            else
            {
                return View("NotFound");
            }
        }

        #endregion
        
        #region Create

        // GET: /Dinners/Create
        public ActionResult Create()
        {
            Dinner dinner = new Dinner();
            dinner.Address = "Address1";
            dinner.Country = "India";
            dinner.Description = "Indian Dinner";
            dinner.EventDate = DateTime.Now.AddDays(7);
            dinner.HostedBy = "Arun M";
            dinner.Title = "Indian Title";

            ViewData["Countries"] = new SelectList(PhoneValidator.Countries, dinner.Country);
            return View(dinner);

            //DinnerCreateFormViewModel dinnerCreateFormViewModel = new DinnerCreateFormViewModel(dinner);
            //return View(dinnerCreateFormViewModel);
        } 

        // First Approach - Create a new Dinner object and then use the UpdateModel() helper method to populate it with the posted form values.
        // POST: /Dinners/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateObsolete(FormCollection collection)
        {
            Dinner dinner = new Dinner();

            try
            {
                UpdateModel(dinner);
                dinnerRepository.Add(dinner);
                dinnerRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
            catch
            {
                ModelState.AddRuleViolations(dinner.GetRuleViolations());  // Second Approach
                return View(dinner);
            }
        }


        // Second Approach - Have our Create() action method take a Dinner object as a method parameter. ASP.NET MVC will then automatically instantiate a new Dinner object for us, populate its properties using the form inputs, and pass it to our action method: 
        // POST: /Dinners/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Dinner dinner)
        {             
                
                // Both approach works perfect

                #region Approach 1

                //if (dinner.IsAMValid)
                //{
                //    dinnerRepository.Add(dinner);
                //    dinnerRepository.Save(); // This will call Arun.Manglick.MVC.NerdDinner.Models.OnValidate() method to check the validation
                //    return RedirectToAction("Details", new { id = dinner.DinnerID });
                //}
                //else
                //{
                //    ModelState.AddRuleViolations(dinner.GetRuleViolations());  // Second Approach
                //    ViewData["Countries"] = new SelectList(PhoneValidator.Countries, dinner.Country);
                //    return View(dinner);
                //}

                #endregion

                #region Approach 2

                try
                {
                    // UpdateModel(dinner); // Not Required here
                    dinnerRepository.Add(dinner);
                    dinnerRepository.Save(); // This will call Arun.Manglick.MVC.NerdDinner.Models.OnValidate() method to check the validation
                    return RedirectToAction("Details", new { id = dinner.DinnerID });
                }
                catch
                {
                    ModelState.AddRuleViolations(dinner.GetRuleViolations());  // Second Approach
                    ViewData["Countries"] = new SelectList(PhoneValidator.Countries, dinner.Country);
                    return View(dinner);
                }

                #endregion
        }

        #endregion

        #region Create - Using Custom Model

        // GET: /Dinners/Create
        public ActionResult CreateCustomModel()
        {
            Dinner dinner = new Dinner();
            dinner.Address = "Address1";
            dinner.Country = "India";
            dinner.Description = "Indian Dinner";
            dinner.EventDate = DateTime.Now.AddDays(7);
            dinner.HostedBy = "Arun M";
            dinner.Title = "Indian Title";


           DinnerCreateFormViewModel dinnerCreateFormViewModel = new DinnerCreateFormViewModel(dinner);
           return View(dinnerCreateFormViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCustomModel(Dinner dinner)
        {
            try
            {
                dinnerRepository.Add(dinner);
                dinnerRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
            catch
            {
                ModelState.AddRuleViolations(dinner.GetRuleViolations());
                DinnerCreateFormViewModel dinnerCreateFormViewModel = new DinnerCreateFormViewModel(dinner);
                return View(dinnerCreateFormViewModel);
            }
        }

        #endregion

        #region Edit

        // GET: /Dinners/Edit/5 
        public ActionResult Edit(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            return View("Edit", dinner);
        }

        // POST: /Dinners/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Dinner dinner = null;
            try
            {
                dinner = dinnerRepository.GetDinner(id);
                string[] allowedProperties = new[]{ "Title", "Description"};

                UpdateModel(dinner, allowedProperties);

                #region Another Approach

                //dinner.Title = Request.Form["Title"];
                //dinner.Description = Request.Form["Description"];
                //dinner.EventDate = DateTime.Parse(Request.Form["EventDate"]);
                //dinner.Address = Request.Form["Address"];
                //dinner.Country = Request.Form["Country"];
                //dinner.ContactPhone = Request.Form["ContactPhone"];

                #endregion

                dinnerRepository.Save();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
            catch
            {
                #region One Approach
                //foreach (var issue in dinner.GetRuleViolations())
                //{
                //    ModelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
                //}
                #endregion

                ModelState.AddRuleViolations(dinner.GetRuleViolations());  // Second Approach
                return View();
            }
        }

        #endregion

        #region Delete

        // GET: /Dinners/Delete/5 
        public ActionResult Delete(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);

            if (dinner == null)
                return View("NotFound");

            return View(dinner);
        }

        // POST: /Dinners/Delete/5 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, string confirmButton)
        {
            Dinner dinner = null;
            try
            {
                dinner = dinnerRepository.GetDinner(id);

                if (dinner == null)
                    return View("NotFound");

                dinnerRepository.Delete(dinner);
                dinnerRepository.Save();

                return View("Deleted");
            }
            catch
            {
                ModelState.AddRuleViolations(dinner.GetRuleViolations());  // Second Approach
                return View();
            }

        }

        #endregion
    }
}
