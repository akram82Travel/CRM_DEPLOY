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
    public class Crm_HistoriqueTypeController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_HistoriqueType
        public ActionResult Index()
        {
            Crm_HistoriqueType crm_TacheReclamation = new Crm_HistoriqueType();
            var result =
                from a in db.Crm_HistoriqueType
                join b in db.Crm_TypeTache on a.Type equals b.CodeTypeTache into bb
                from prod1 in bb
                join c in db.crm_ModeTache on a.TypePiece equals c.CodeModeTache into cc
                from prod2 in cc
                join d in db.Respensable on a.NomValidateur equals d.CodeRespensable
               
                join e in db.Crm_Sanction on a.IdSanction equals e.IdSanction into ee
                from prod3 in ee
                
                join f in db.Crm_Degres_Sanction on a.IdDegres equals f.id into ff
                from prod4 in ff
                
            select new Crm_HistoriqueType
            {
                NumeroTache = a.NumeroTache,
                Type = prod1.Libelle,
                TypePiece = prod2.Libelle,
                IdSanction = prod3.IdSanction,
                IdDegres = prod4.id,
                NomValidateur = d.Nom,
                DateOperation= a.DateOperation,
               
            };
            
           
            return View("Index", db.Crm_HistoriqueType.ToList());
        }

        // GET: Crm_HistoriqueType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_HistoriqueType crm_HistoriqueType = db.Crm_HistoriqueType.Find(id);
            if (crm_HistoriqueType == null)
            {
                return HttpNotFound();
            }
            return View(crm_HistoriqueType);
        }

        // GET: Crm_HistoriqueType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Crm_HistoriqueType/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,NumeroTache,Type,TypePiece,IdSanction,IdDegres,NomValidateur,DateOperation")] Crm_HistoriqueType crm_HistoriqueType)
        {
            if (ModelState.IsValid)
            {
                crm_HistoriqueType.DateOperation = DateTime.Now;
                db.Crm_HistoriqueType.Add(crm_HistoriqueType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crm_HistoriqueType);
        }

        // GET: Crm_HistoriqueType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_HistoriqueType crm_HistoriqueType = db.Crm_HistoriqueType.Find(id);
            if (crm_HistoriqueType == null)
            {
                return HttpNotFound();
            }
            return View(crm_HistoriqueType);
        }

        // POST: Crm_HistoriqueType/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,NumeroTache,Type,TypePiece,IdSanction,IdDegres,NomValidateur,DateOperation")] Crm_HistoriqueType crm_HistoriqueType)
        {
            if (ModelState.IsValid)
            {

                db.Entry(crm_HistoriqueType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_HistoriqueType);
        }

        // GET: Crm_HistoriqueType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_HistoriqueType crm_HistoriqueType = db.Crm_HistoriqueType.Find(id);
            if (crm_HistoriqueType == null)
            {
                return HttpNotFound();
            }
            return View(crm_HistoriqueType);
        }

        // POST: Crm_HistoriqueType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Crm_HistoriqueType crm_HistoriqueType = db.Crm_HistoriqueType.Find(id);
            db.Crm_HistoriqueType.Remove(crm_HistoriqueType);
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
