using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRMSTUBSOFT;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_TypeTacheController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_TypeTache
        public ActionResult Index()
        {
            return View(db.Crm_TypeTache.ToList());
        }

        // GET: Crm_TypeTache/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_TypeTache crm_TypeTache = db.Crm_TypeTache.Find(id);
            if (crm_TypeTache == null)
            {
                return HttpNotFound();
            }
            return View(crm_TypeTache);
        }

        // GET: Crm_TypeTache/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Crm_TypeTache/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeTypeTache,Libelle")] Crm_TypeTache crm_TypeTache)
        {
            if (ModelState.IsValid)
            {
                db.Crm_TypeTache.Add(crm_TypeTache);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crm_TypeTache);
        }

        // GET: Crm_TypeTache/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_TypeTache crm_TypeTache = db.Crm_TypeTache.Find(id);
            if (crm_TypeTache == null)
            {
                return HttpNotFound();
            }
            return View(crm_TypeTache);
        }

        // POST: Crm_TypeTache/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeTypeTache,Libelle")] Crm_TypeTache crm_TypeTache)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_TypeTache).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_TypeTache);
        }

        // GET: Crm_TypeTache/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_TypeTache crm_TypeTache = db.Crm_TypeTache.Find(id);
            if (crm_TypeTache == null)
            {
                return HttpNotFound();
            }
            return View(crm_TypeTache);
        }

        // POST: Crm_TypeTache/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Crm_TypeTache crm_TypeTache = db.Crm_TypeTache.Find(id);
            db.Crm_TypeTache.Remove(crm_TypeTache);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
