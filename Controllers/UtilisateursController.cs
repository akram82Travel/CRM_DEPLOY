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
    public class UtilisateursController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Utilisateurs
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var utilisateur = db.Utilisateur.Include(u => u.Fonction);
            return View(utilisateur.ToList());
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "Libelle");
            return View();
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NomUtilisateur,CodeSociete,CodeFonction,Nom,Prenom,MotDePasse,Skin,CodeRepresentant,MotDePasseValidation,MotDePasseAnnulation,MDPValidation,MDPReimpression,SaisieLibreLivraisonVente,SaisieLibreReceptionAchat,CodeEmployer,Admin,Administrateur,CreerPar,CreerLe,Actif,SaisieLibreRetourVente,SaisieLibreRetourAchat,CodeService,MDPAdministrateur,CodeIntervenant,CodeFonctionAndroid,SaisieLibreNotification")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Utilisateur.Add(utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "Libelle", utilisateur.CodeFonction);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "Libelle", utilisateur.CodeFonction);
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NomUtilisateur,CodeSociete,CodeFonction,Nom,Prenom,MotDePasse,Skin,CodeRepresentant,MotDePasseValidation,MotDePasseAnnulation,MDPValidation,MDPReimpression,SaisieLibreLivraisonVente,SaisieLibreReceptionAchat,CodeEmployer,Admin,Administrateur,CreerPar,CreerLe,Actif,SaisieLibreRetourVente,SaisieLibreRetourAchat,CodeService,MDPAdministrateur,CodeIntervenant,CodeFonctionAndroid,SaisieLibreNotification")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "Libelle", utilisateur.CodeFonction);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            db.Utilisateur.Remove(utilisateur);
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
