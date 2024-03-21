using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using CRMSTUBSOFT;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_CycleExecTacheController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_CycleExecTache
        public ActionResult Index()
        {
            return View(db.Crm_CycleExecTache.ToList());
        }

        // GET: Crm_CycleExecTache/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_CycleExecTache crm_CycleExecTache = db.Crm_CycleExecTache.Find(id);
            if (crm_CycleExecTache == null)
            {
                return HttpNotFound();
            }
            return View(crm_CycleExecTache);
        }

        // GET: Crm_CycleExecTache/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Crm_CycleExecTache/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NumeroTache,Nomutilisateur,OldStatus,NewStatus,DateDebutExecution,DateFinExecution,Duree,idTacheExec,Encours")] Crm_CycleExecTache crm_CycleExecTache)
        {
            if (ModelState.IsValid)
            {
                db.Crm_CycleExecTache.Add(crm_CycleExecTache);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crm_CycleExecTache);
        }

        // GET: Crm_CycleExecTache/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_CycleExecTache crm_CycleExecTache = db.Crm_CycleExecTache.Find(id);
            ViewData["d1"] = DateTime.Parse(crm_CycleExecTache.DateDebutExecution.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            ViewData["d2"] = DateTime.Parse(crm_CycleExecTache.DateFinExecution.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            if (crm_CycleExecTache == null)
            {
                return HttpNotFound();
            }
            Session["NumeroTache"] = crm_CycleExecTache.NumeroTache;
            return View(crm_CycleExecTache);
        }

        // POST: Crm_CycleExecTache/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NumeroTache,Nomutilisateur,OldStatus,NewStatus,DateDebutExecution,DateFinExecution,Duree,idTacheExec,Encours")] Crm_CycleExecTache crm_CycleExecTache)
        {
            if (ModelState.IsValid)
            {
                /******************/
                /** calcul duree en minute **/
                /*******************/
                TimeSpan diff = TimeSpan.FromTicks(0);
                diff = crm_CycleExecTache.DateFinExecution - crm_CycleExecTache.DateDebutExecution;

                /***************/
                /**** Fin calcul duree *******/
                /***************/
                crm_CycleExecTache.Duree = Convert.ToInt32(diff.TotalMinutes); ;

                db.Entry(crm_CycleExecTache).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditByTache", "Crm_CycleExecTache", new { IdTache = crm_CycleExecTache.NumeroTache });
            }
            return View(crm_CycleExecTache);
        }

        // GET: Crm_CycleExecTache/EditByTache/5
       
        public ActionResult EditByTache()
        {
            string Idtache = Request["IdTache"];
           
           
            var crm_CycleExecTache = from c in db.Crm_CycleExecTache
                                                     where c.NumeroTache == Idtache
                                                     select c;

            ViewData["idTache"] = Idtache;
            
                //db.Crm_CycleExecTache.First(s=>s.NumeroTache == Idtache);
            //if (crm_CycleExecTache == null)
            //{
            //    return HttpNotFound();
            //}
            return View(crm_CycleExecTache.ToList());
        }

        // POST: Crm_CycleExecTache/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditByTacheExecute([Bind(Include = "NumeroTache,Nomutilisateur,OldStatus,NewStatus,DateDebutExecution,DateFinExecution,Duree,idTacheExec,Encours")] Crm_CycleExecTache crm_CycleExecTache)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_CycleExecTache).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_CycleExecTache);
        }

        
        
        
        
        
        // GET: Crm_CycleExecTache/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_CycleExecTache crm_CycleExecTache = db.Crm_CycleExecTache.Find(id);
            if (crm_CycleExecTache == null)
            {
                return HttpNotFound();
            }
            return View(crm_CycleExecTache);
        }

        // POST: Crm_CycleExecTache/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Crm_CycleExecTache crm_CycleExecTache = db.Crm_CycleExecTache.Find(id);
            db.Crm_CycleExecTache.Remove(crm_CycleExecTache);
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
