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
    public class Crm_SanctionController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_Sanction
        public ActionResult Index()
        {
            var model = (from c in db.Crm_Sanction
                         
                         join b in db.Respensable on c.CodeUtilisateur equals b.CodeRespensable into tl
                         from b in tl.DefaultIfEmpty()
                         
                         join tt in db.Crm_TypeTache on c.TypeTache equals tt.CodeTypeTache into ttch
                         from tt in ttch.DefaultIfEmpty()
                         
                         join mt in db.crm_ModeTache on c.ModeTache equals mt.CodeModeTache into mtch
                         from mt in mtch.DefaultIfEmpty()
                         select new ViewModel
                         {
                             Crm_Sanction = c,
                             Respensable = b,
                             TypeTache = tt,
                             ModeTache = mt, 

                         }).ToList();

            return View(model);
        }

        // GET: Crm_Sanction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Sanction crm_Sanction = db.Crm_Sanction.Find(id);
            if (crm_Sanction == null)
            {
                return HttpNotFound();
            }
            var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle", crm_Sanction.ModeTache);
            ViewData["ListMode"] = ListMode;
            var ListType = new SelectList(db.Crm_TypeTache.OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle", crm_Sanction.TypeTache);
            ViewData["ListType"] = ListType;
            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", crm_Sanction.CodeUtilisateur);
            ViewData["ListResponsable"] = ListResponsable;
            return View(crm_Sanction);
        }

        // GET: Crm_Sanction/Create
        public ActionResult Create()
        {
            var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle");
            ViewData["ListMode"] = ListMode;
            var ListType = new SelectList(db.Crm_TypeTache.OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
            ViewData["ListType"] = ListType;
            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListResponsable"] = ListResponsable;
            return View();
        }

        // POST: Crm_Sanction/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSanction,LibelleSanction,NoteSanction,TypeTache,ModeTache,CodeUtilisateur,Degres")] Crm_Sanction crm_Sanction)
        {
            if (ModelState.IsValid)
            {
                db.Crm_Sanction.Add(crm_Sanction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crm_Sanction);
        }

        // GET: Crm_Sanction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Sanction crm_Sanction = db.Crm_Sanction.Find(id);
            if (crm_Sanction == null)
            {
                return HttpNotFound();
            }

            var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle", crm_Sanction.ModeTache);
            ViewData["ListMode"] = ListMode;
            var ListType = new SelectList(db.Crm_TypeTache.OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle", crm_Sanction.TypeTache);
            ViewData["ListType"] = ListType;
            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", crm_Sanction.CodeUtilisateur);
            ViewData["ListResponsable"] = ListResponsable;
            return View(crm_Sanction);
        }

        // POST: Crm_Sanction/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSanction,LibelleSanction,NoteSanction,TypeTache,ModeTache,CodeUtilisateur,TypeSanction")] Crm_Sanction crm_Sanction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_Sanction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_Sanction);
        }

        // GET: Crm_Sanction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Sanction crm_Sanction = db.Crm_Sanction.Find(id);
            if (crm_Sanction == null)
            {
                return HttpNotFound();
            }
            return View(crm_Sanction);
        }

        // POST: Crm_Sanction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Crm_Sanction crm_Sanction = db.Crm_Sanction.Find(id);
            db.Crm_Sanction.Remove(crm_Sanction);
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
