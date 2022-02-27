using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Arun.Manglick.MvcUI.Models;

namespace Arun.Manglick.MvcUI.Controllers
{
    public class ProductsController : Controller
    {
        Northwind1DataContext dataContext = new Northwind1DataContext();

        //
        // GET: /Products/
        public ActionResult Index(int id)
        {
            var products = from p in dataContext.Products
                           select p;
            //ViewData.Model = products;  // Optional
            return View(products);
        }

        //
        // GET: /Products/Details/5
        public ActionResult Details(int id)
        {
            Product p1 = dataContext.Products.First(p => p.ProductID == id);
            return View(p1);
        }

        //
        // GET: /Products/Create
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Products/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Products/Edit/5 
        public ActionResult Edit(int id)
        {
            Product p1 = dataContext.Products.First(p => p.ProductID == id);
            return View(p1);
        }

        //
        // POST: /Products/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Product p1 = dataContext.Products.First(p => p.ProductID == id);

            try
            {
                // TODO: Add update logic here

                var whitelist = new[] { "ProductName", "QuantityPerUnit", "UnitPrice", "UnitsInStock" };
                UpdateModel(p1, "whitelist");
                dataContext.SubmitChanges();
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View(p1);
            }
        }
    }
}
