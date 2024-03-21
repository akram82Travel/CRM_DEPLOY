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
    public class Crm_TacheNatureController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_TacheNature
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            var d1 = DateTime.Now.AddMonths(-1).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d1"] = d1;
            ViewData["d2"] = d2;
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);


            string codeuser = Session["UserID"].ToString();
            if (Session["UserRole"].ToString() != "03")
            {
                return View(db.Crm_TacheNature.ToList());
            }

            else
            {
                codeuser = Session["UserID"].ToString();
                return View(db.Crm_TacheNature.ToList());
            }
        }

        // GET: Crm_TacheNature/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_TacheNature crm_TacheNature = db.Crm_TacheNature.Find(id);
            if (crm_TacheNature == null)
            {
                return HttpNotFound();
            }
            return View(crm_TacheNature);
        }

        // GET: Crm_TacheNature/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: Crm_TacheNature/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nature,Libelle")] Crm_TacheNature crm_TacheNature)
        {
            if (ModelState.IsValid)
            {
                db.Crm_TacheNature.Add(crm_TacheNature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crm_TacheNature);
        }

        // GET: Crm_TacheNature/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_TacheNature crm_TacheNature = db.Crm_TacheNature.Find(id);
            if (crm_TacheNature == null)
            {
                return HttpNotFound();
            }
            return View(crm_TacheNature);
        }

        // POST: Crm_TacheNature/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nature,Libelle")] Crm_TacheNature crm_TacheNature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_TacheNature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crm_TacheNature);
        }

        // GET: Crm_TacheNature/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_TacheNature crm_TacheNature = db.Crm_TacheNature.Find(id);
            if (crm_TacheNature == null)
            {
                return HttpNotFound();
            }
            return View(crm_TacheNature);
        }

        // POST: Crm_TacheNature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Crm_TacheNature crm_TacheNature = db.Crm_TacheNature.Find(id);
            db.Crm_TacheNature.Remove(crm_TacheNature);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Detailsmodule(int id)
        {
           /* if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            Crm_ModuleNature crm_ModuleNature = db.Crm_ModuleNature.Find(id);
            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            if (crm_ModuleNature == null)
            {
                return HttpNotFound();
            }
            return View("ModuleNature", crm_ModuleNature);
        }
        public ActionResult Createmodule()
        {
            //DropdownList
            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            return View("ModuleNature");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createmodule([Bind(Include = "Nature,Libelle")]Crm_ModuleNature crm_ModuleNature)
        {
            if (ModelState.IsValid)
            {
                db.Crm_ModuleNature.Add(crm_ModuleNature);
                db.SaveChanges();
                return RedirectToAction("IndexModule");
            }
            return View("ModuleNature", crm_ModuleNature);
        }

      
        public ActionResult IndexModule()
        {
            //List<Crm_ModuleNature> Listcrm_ModuleNature = db.Crm_ModuleNature.Where(t => t.Nature == Nature).ToList();
            // ViewData["Listcrm_ModuleNature"] = Listcrm_ModuleNature;
            return View(db.Crm_ModuleNature.ToList());
        }



        // GET: Crm_TacheNature/Edit/5
        public ActionResult EditModule(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_ModuleNature crm_ModuleNature = db. Crm_ModuleNature.Find(id);
            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            if (crm_ModuleNature == null)
            {
                return HttpNotFound();
            }
            return View("ModuleNature", crm_ModuleNature);
        }

        // POST: Crm_TacheNature/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule([Bind(Include = "IdNatureType, Nature,Libelle")] Crm_ModuleNature crm_ModuleNature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crm_ModuleNature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexModule");
            }
            return View("ModuleNature", crm_ModuleNature);
        }

        // GET: Crm_TacheNature/Delete/5
        public ActionResult DeleteModule(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_ModuleNature crm_ModuleNature = db.Crm_ModuleNature.Find(id);
            if (crm_ModuleNature == null)
            {
                return HttpNotFound();
            }
            return View(crm_ModuleNature);
        }

        // POST: Crm_TacheNature/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteModuleConfirmed(int id)
        {
            Crm_ModuleNature crm_ModuleNature = db.Crm_ModuleNature.Find(id);
            db.Crm_ModuleNature.Remove(crm_ModuleNature);
            db.SaveChanges();
            return RedirectToAction("IndexModule");
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
