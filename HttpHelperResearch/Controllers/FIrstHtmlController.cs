using HttpHelperResearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HttpHelperResearch.Controllers
{
    public class FIrstHtmlController : Controller
    {
        // GET: FIrstHtml
        public ActionResult Index()
        {
            List<FirstModel> models = new List<FirstModel>();
            models.Add(new FirstModel() { Text = "page1", Url = "view1" });
            models.Add(new FirstModel() { Text = "page2", Url = "view2" });
            models.Add(new FirstModel() { Text = "page3", Url = "view3" });
            return View(models);
        }

        // GET: FIrstHtml/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FIrstHtml/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FIrstHtml/Create
        [HttpPost]
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

        // GET: FIrstHtml/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FIrstHtml/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FIrstHtml/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FIrstHtml/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
