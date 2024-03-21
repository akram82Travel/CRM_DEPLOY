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
    public class Crm_Degres_SanctionController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_Degres_Sanction
        public ActionResult Index()
        {
            return View(db.Crm_Degres_Sanction.ToList());
        }

        // GET: Crm_Degres_Sanction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Degres_Sanction crm_Degres_Sanction = db.Crm_Degres_Sanction.Find(id);
            if (crm_Degres_Sanction == null)
            {
                return HttpNotFound();
            }
            return View(crm_Degres_Sanction);
        }

        // GET: Crm_Degres_Sanction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Crm_Degres_Sanction/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,du,au,libelle,type")] Crm_Degres_Sanction crm_Degres_Sanction)
        {
            if (ModelState.IsValid)
            {
                db.Crm_Degres_Sanction.Add(crm_Degres_Sanction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crm_Degres_Sanction);
        }

        // GET: Crm_Degres_Sanction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Degres_Sanction crm_Degres_Sanction = db.Crm_Degres_Sanction.Find(id);
            if (crm_Degres_Sanction == null)
            {
                return HttpNotFound();
            }
            return View(crm_Degres_Sanction);
        }

        // POST: Crm_Degres_Sanction/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,du,au,libelle,type")] Crm_Degres_Sanction crm_Degres_Sanction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_Degres_Sanction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_Degres_Sanction);
        }

        // GET: Crm_Degres_Sanction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Degres_Sanction crm_Degres_Sanction = db.Crm_Degres_Sanction.Find(id);
            if (crm_Degres_Sanction == null)
            {
                return HttpNotFound();
            }
            return View(crm_Degres_Sanction);
        }

        // POST: Crm_Degres_Sanction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Crm_Degres_Sanction crm_Degres_Sanction = db.Crm_Degres_Sanction.Find(id);
            db.Crm_Degres_Sanction.Remove(crm_Degres_Sanction);
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
