using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CRMSTUBSOFT;
using CRMSTUBSOFT.Services.Business;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_PVReunionController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_PVReunion
        public ActionResult Index()
        {
            return View(db.Crm_PVReunion.ToList());
        }
      

        // GET: Crm_PVReunion/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from c in db.Crm_PersonneParReunion
                         where c.NumeroPVReunion == id
                         select c.CodePersonne).ToList();

            List<string> Personnename = new List<string>();

            foreach (var item in model)
            {
                Crm_Personne crm_Personne = db.Crm_Personne.Find(item);
                Personnename.Add(crm_Personne.NomPrenom);
            }

            ViewData["listpersonne"] = Personnename;

            Crm_PVReunion crm_PVReunion = db.Crm_PVReunion.Find(id);
            if (crm_PVReunion == null)
            {
                return HttpNotFound();
            }

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            return View(crm_PVReunion);
        }

        // GET: Crm_PVReunion/Create
        public ActionResult Create()
        {
            Crm_PVReunion crm_PVReunion = new Crm_PVReunion();

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            //ViewBag.CodePersonne = new SelectList(db.Crm_Personne, "CodePersonne", "NomPrenom");

            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("PV", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

            crm_PVReunion.NumeroPVReunion = "PV" + "210" + valeur.ToString("00000");

            return View(crm_PVReunion);
        }

        // POST: Crm_PVReunion/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NumeroPVReunion,CodeClient,CodePersonne,Projet,Module,Createur,Objet,DateReunion,DatePrevusReunion,DescriptionGeneral,Dure")] Crm_PVReunion crm_PVReunion, string[] DynamicTextBox)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewBag.Values = serializer.Serialize(DynamicTextBox);

            if (ModelState.IsValid)
            {
                crm_PVReunion.Createur = Session["UserID"].ToString();
                crm_PVReunion.CodePersonne = " ";

                /*Control IdBase*/
                CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_PVReunion");
                decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur);
                decimal valeur2 = Convert.ToDecimal(crm_PVReunion.NumeroPVReunion.Substring(4));

                if (valeur2 - valeur1 > 0)
                {
                    crm_PVReunion.NumeroPVReunion= "PV" + "210" + valeur2.ToString("00000");

                }
                else
                {

                    crm_PVReunion.NumeroPVReunion = "PV" + "210" + (valeur1 + 1).ToString("00000");

                }

                compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_PVReunion");
                compteurPiece.Compteur = crm_PVReunion.NumeroPVReunion.Substring(4);
                db.Entry(compteurPiece).State = EntityState.Modified;

              
                Crm_Personne crm_Personne = new Crm_Personne();
                List<string> PersonneList = new List<string>();
                foreach (string textboxValue in DynamicTextBox)
                {
                    crm_Personne = new Crm_Personne();
                    /*Primary key */
                    SecurityServices securityService = new SecurityServices();
                    var CompteurId = securityService.GetIdByCompteur("Pr", "210").ToList();
                    foreach (var item in CompteurId)
                    {
                        ViewData["Compteur"] = item;
                    }
                    decimal valeur;
                    valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

                    crm_Personne.CodePersonne = "Pr" + "210" + valeur.ToString("00000");
                    /*Control IdBase*/
                    CompteurPiece compteurPiecep = db.CompteurPiece.Find("SOC001", "Crm_Personne");
                    decimal valeur11 = Convert.ToDecimal(compteurPiecep.Compteur);
                    decimal valeur22 = Convert.ToDecimal(crm_Personne.CodePersonne.Substring(4));

                    if (valeur22 - valeur11 > 0)
                    {
                        crm_Personne.CodePersonne = "Pr" + "210" + valeur22.ToString("00000");

                    }
                    else
                    {

                        crm_Personne.CodePersonne = "Pr" + "210" + (valeur11 + 1).ToString("00000");

                    }

                    compteurPiecep = db.CompteurPiece.Find("SOC001", "Crm_Personne");
                    compteurPiecep.Compteur = crm_Personne.CodePersonne.Substring(4);
                    db.Entry(compteurPiecep).State = EntityState.Modified;

                    /*end*/
                    crm_Personne.Adresse = " ";
                    crm_Personne.Tel = " ";
                    crm_Personne.Nationalite = " ";
                    crm_Personne.NomPrenom= textboxValue;
                    PersonneList.Add(crm_Personne.CodePersonne);
                    db.Crm_Personne.Add(crm_Personne);
                    db.SaveChanges();
                }

                /*Add selected respensable to base*/
         
                foreach (var item in PersonneList)
                {
                    Crm_PersonneParReunion crm_PersonneParReunion = new Crm_PersonneParReunion();
                    crm_PersonneParReunion.CodePersonne = item;
                    crm_PersonneParReunion.NumeroPVReunion = crm_PVReunion.NumeroPVReunion;
                    db.Crm_PersonneParReunion.Add(crm_PersonneParReunion);
                    db.SaveChanges();
                }


                db.Crm_PVReunion.Add(crm_PVReunion);
                db.SaveChanges();
                TempData["SuccessMesage"] = "PV Réunion N°" + @crm_PVReunion.NumeroPVReunion+ " est enregistrée! ";
                return RedirectToAction("Index");
            }

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            return View(crm_PVReunion);
        }

        // GET: Crm_PVReunion/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from c in db.Crm_PersonneParReunion
                         where c.NumeroPVReunion == id
                         select c.CodePersonne).ToList();

            List<string> Personnename= new List<string>();
            
            foreach (var item in model)
            {
                Crm_Personne crm_Personne = db.Crm_Personne.Find(item);
                Personnename.Add(crm_Personne.NomPrenom);
            }

            ViewData["listpersonne"] = Personnename;

            Crm_PVReunion crm_PVReunion = db.Crm_PVReunion.Find(id);
            if (crm_PVReunion == null)
            {
                return HttpNotFound();
            }

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            return View(crm_PVReunion);
        }

        // POST: Crm_PVReunion/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NumeroPVReunion,CodeClient,CodePersonne,Projet,Module,Createur,Objet,DateReunion,DatePrevusReunion,DescriptionGeneral,Dure")] Crm_PVReunion crm_PVReunion, string[] DynamicTextBox)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Values = serializer.Serialize(DynamicTextBox);
            if (ModelState.IsValid)
            {
                /*Update selected responsable to base*/
                Crm_PersonneParReunion crm_PersonneParReunion = new Crm_PersonneParReunion();
                List<string> modelid = new List<string>();
                var model = (from c in db.Crm_PersonneParReunion
                             where c.NumeroPVReunion== crm_PVReunion.NumeroPVReunion
                             select c.CodePersonne).ToList();
                //delete from association
                foreach (var item in model)
                {
                    crm_PersonneParReunion = db.Crm_PersonneParReunion.Find(crm_PVReunion.NumeroPVReunion, item);
                    if (crm_PersonneParReunion.NumeroPVReunion == crm_PVReunion.NumeroPVReunion && crm_PersonneParReunion.CodePersonne== item)
                     db.Crm_PersonneParReunion.Remove(crm_PersonneParReunion);
                    db.SaveChanges();
                }
                //delete from table personne
                foreach (var item in model)
                {
                    Crm_Personne crm_Personne1 = db.Crm_Personne.Find(item);
                    if (crm_Personne1.CodePersonne == item)
                        db.Crm_Personne.Remove(crm_Personne1);
                    db.SaveChanges();
                }



                // insert new person from data form

                Crm_Personne crm_Personne = new Crm_Personne();
                List<string> PersonneList = new List<string>();
                if (DynamicTextBox != null)
                {
                    foreach (string textboxValue in DynamicTextBox)
                    {
                        crm_Personne = new Crm_Personne();
                        /*Primary key */
                        SecurityServices securityService = new SecurityServices();
                        var CompteurId = securityService.GetIdByCompteur("Pr", "210").ToList();
                        foreach (var item in CompteurId)
                        {
                            ViewData["Compteur"] = item;
                        }
                        decimal valeur;
                        valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

                        crm_Personne.CodePersonne = "Pr" + "210" + valeur.ToString("00000");
                        /*Control IdBase*/
                        CompteurPiece compteurPiecep = db.CompteurPiece.Find("SOC001", "Crm_Personne");
                        decimal valeur11 = Convert.ToDecimal(compteurPiecep.Compteur);
                        decimal valeur22 = Convert.ToDecimal(crm_Personne.CodePersonne.Substring(4));

                        if (valeur22 - valeur11 > 0)
                        {
                            crm_Personne.CodePersonne = "Pr" + "210" + valeur22.ToString("00000");

                        }
                        else
                        {

                            crm_Personne.CodePersonne = "Pr" + "210" + (valeur11 + 1).ToString("00000");

                        }

                        compteurPiecep = db.CompteurPiece.Find("SOC001", "Crm_Personne");
                        compteurPiecep.Compteur = crm_Personne.CodePersonne.Substring(4);
                        db.Entry(compteurPiecep).State = EntityState.Modified;

                        /*end*/
                        crm_Personne.Adresse = " ";
                        crm_Personne.Tel = " ";
                        crm_Personne.Nationalite = " ";
                        crm_Personne.NomPrenom = textboxValue;
                        PersonneList.Add(crm_Personne.CodePersonne);
                        db.Crm_Personne.Add(crm_Personne);
                        db.SaveChanges();
                    }

                    /*Add selected respensable to base*/

                    foreach (var item in PersonneList)
                    {
                        Crm_PersonneParReunion crm_PersonneParReunion11 = new Crm_PersonneParReunion();
                        crm_PersonneParReunion11.CodePersonne = item;
                        crm_PersonneParReunion11.NumeroPVReunion = crm_PVReunion.NumeroPVReunion;
                        db.Crm_PersonneParReunion.Add(crm_PersonneParReunion11);
                        db.SaveChanges();
                    }
                }
                else
                    DynamicTextBox = new string[] { };
               
                // end insert



                crm_PVReunion.Createur = Session["UserID"].ToString();
                crm_PVReunion.CodePersonne = " ";
                ViewData["listpersonne"] = DynamicTextBox.ToList();
                db.Entry(crm_PVReunion).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMesage"] = "PV Réunion N°" + @crm_PVReunion.NumeroPVReunion + " est modifiée! ";

                return RedirectToAction("Index");
            }
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            ViewData["listpersonne"] = DynamicTextBox.ToList();
            return View(crm_PVReunion);
        }

        // GET: Crm_PVReunion/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_PVReunion crm_PVReunion = db.Crm_PVReunion.Find(id);
            if (crm_PVReunion == null)
            {
                return HttpNotFound();
            }
            return View(crm_PVReunion);
        }

        // POST: Crm_PVReunion/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Crm_PVReunion crm_PVReunion = db.Crm_PVReunion.Find(id);

            //delete from asociation table crm_personneparreunion
            var model = (from c in db.Crm_PersonneParReunion
                         where c.NumeroPVReunion == crm_PVReunion.NumeroPVReunion
                         select c.CodePersonne).ToList();

            foreach (var item in model)
            {
                Crm_PersonneParReunion crm_PersonneParReunion = db.Crm_PersonneParReunion.Find(crm_PVReunion.NumeroPVReunion, item);
                if (crm_PersonneParReunion.NumeroPVReunion == crm_PVReunion.NumeroPVReunion && crm_PersonneParReunion.CodePersonne == item)
                    db.Crm_PersonneParReunion.Remove(crm_PersonneParReunion);
                db.SaveChanges();
            }
            //delete from table personne
            foreach (var item in model)
            {
                Crm_Personne crm_Personne1 = db.Crm_Personne.Find(item);
                if (crm_Personne1.CodePersonne == item)
                    db.Crm_Personne.Remove(crm_Personne1);
                db.SaveChanges();
            }

            db.Crm_PVReunion.Remove(crm_PVReunion);
            db.SaveChanges();
            //TempData["SuppressionMessage"] = "PVRéunion N°" + @crm_PVReunion.NumeroPVReunion + " est supprimée! ";

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

        public List<Crm_Personne> PersonnesData()
        {
            return db.Crm_Personne.ToList();
        }
    }
}
