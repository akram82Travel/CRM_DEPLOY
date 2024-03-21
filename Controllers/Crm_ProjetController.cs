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
    public class Crm_ProjetController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_Projet
        public ActionResult Index()
        {
            return View(db.Crm_Projet.ToList());
        }

        // GET: Crm_Projet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Projet crm_Projet = db.Crm_Projet.Find(id);
            if (crm_Projet == null)
            {
                return HttpNotFound();
            }
            return View(crm_Projet);
        }

        // GET: Crm_Projet/Create
        public ActionResult Create()
        {
            var ListResponsable = new SelectList(db.Respensable.ToList(), "Nom", "Nom", @Session["codeResponsable"]);

            ViewData["ListResponsable"] = ListResponsable;
            return View();
        }

        // POST: Crm_Projet/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeProjet,CodeTypeProjet,Libelle,MontantProjet,DateDeclenchement,DateFinPrevu,Cloture,DateCloture,ResponsableProjet,ObjectifProjet,DescriptionProjet")] Crm_Projet crm_Projet)
        {
            if (ModelState.IsValid)
            {
                db.Crm_Projet.Add(crm_Projet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var ListResponsable = new SelectList(db.Respensable.ToList(), "Nom", "Nom");

            ViewData["ListResponsable"] = ListResponsable;
            return View(crm_Projet);
        }

        // GET: Crm_Projet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Projet crm_Projet = db.Crm_Projet.Find(id);
            if (crm_Projet == null)
            {
                return HttpNotFound();
            }
            return View(crm_Projet);
        }

        // POST: Crm_Projet/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeProjet,CodeTypeProjet,Libelle,MontantProjet,DateDeclenchement,DateFinPrevu,Cloture,DateCloture,ResponsableProjet,ObjectifProjet,DescriptionProjet")] Crm_Projet crm_Projet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_Projet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_Projet);
        }

        // GET: Crm_Projet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Projet crm_Projet = db.Crm_Projet.Find(id);
            if (crm_Projet == null)
            {
                return HttpNotFound();
            }
            return View(crm_Projet);
        }

        // POST: Crm_PVReunion/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Crm_Projet Crm_Projets = db.Crm_Projet.Find(id);

            db.Crm_Projet.Remove(Crm_Projets);
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
