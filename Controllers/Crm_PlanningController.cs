using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRMSTUBSOFT;
using CRMSTUBSOFT.Services.Business;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_PlanningController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_Planning
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            return View(db.Crm_Planning.ToList());
        }

        // GET: Crm_Planning/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Planning crm_Planning = db.Crm_Planning.Find(id);
            //Respensable respensable = db.Respensable.Find(crm_Planning.CodeResponsable);
            var list = db.Crm_PlanningTime.Where(c => c.Id_Plannig == id).ToList();
            string[] rsp = new string[list.Count];
            string[] obj = new string[list.Count];

            var x = ""; var y = "";
            for (int i = 0, j = 0; i < list.Count && j < rsp.Length; i++, j++)
            {

                if (x != list[i].CodeResponsable)
                {

                    rsp[i] = list[j].CodeResponsable;
                    x = rsp[i];
                }

                if (y != list[i].ObjectisNonRealiser)
                {

                    obj[i] = list[j].ObjectisNonRealiser;
                    y = obj[i];
                }

            }


            //ViewData["Nom"]= respensable.Nom; 
            ViewData["DayListe"] = list;
            ViewData["ListeRsp"] = rsp.Where(a => !string.IsNullOrEmpty(a)).ToArray().ToList();
            ViewData["ListeOBJ"] = obj.Where(a => !string.IsNullOrEmpty(a)).ToArray().ToList();


            if (ViewData["ListeOBJ"] == null)
            {
                var n = 0;
                foreach (var item in ViewData["ListeRsp"] as List<string>)
                {
                    if (obj[n] == null)
                    {
                        obj[n] = "En cours";
                        y = obj[n];
                    }
                    n++;
                }
                ViewData["ListeOBJ"] = obj.ToList();

            }

            if (crm_Planning == null)
            {
                return HttpNotFound();
            }
            return View(crm_Planning);
        }

        // GET: Crm_Planning/Create
        public ActionResult Create()
        {
            Crm_Planning crm_Planning = new Crm_Planning();

            var ListResponsable = new SelectList(db.Respensable.ToList(), "Nom", "Nom", @Session["UserPrenom"].ToString() +" " +@Session["UserNom"].ToString());

            ViewData["ListResponsable"] = ListResponsable;

            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("CPL", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

            crm_Planning.Id_Planning = "CPL" + "210" + valeur.ToString("00000");



            return View(crm_Planning);
        }

        // POST: Crm_Planning/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Planning,Datedebut,DateFin,CodeResponsable")] Crm_Planning crm_Planning, string Day1, string Day2, string Day3, string Day4, string Day5, string Day6, string[] MatinO1, string [] MatinO2, string [] MatinO3, string [] MatinO4, string [] MatinO5, string [] MatinO6, string[] ApresMidiO1, string[] ApresMidiO2, string[] ApresMidiO3, string[] ApresMidiO4, string[] ApresMidiO5, string[] ApresMidiO6, string[] MatinR1, string[] MatinR2, string[] MatinR3, string[] MatinR4, string[] MatinR5, string[] MatinR6, string[] ApresMidiR1, string[] ApresMidiR2, string[] ApresMidiR3, string[] ApresMidiR4, string[] ApresMidiR5, string[] ApresMidiR6, string[] OBJ,string[] developpeur)
        {
            if (ModelState.IsValid)
            {


                crm_Planning.Description = "";
                crm_Planning.Nom = "";
                crm_Planning.Prenom = "";
                crm_Planning.Titre = "";
                crm_Planning.CodeClient = "";
                crm_Planning.RaisonSocial = "";

                //*Control IdBase*/
                CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_Planning");
                decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur); decimal valeur2 = Convert.ToDecimal(crm_Planning.Id_Planning.Substring(5));

                if (valeur2 - valeur1 > 0)
                {
                    crm_Planning.Id_Planning = "CPL" + "210" + valeur2.ToString("00000");

                }
                else
                {

                    crm_Planning.Id_Planning = "CPL" + "210" + (valeur1 + 1).ToString("00000");

                }

                compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_Planning");
                compteurPiece.Compteur = crm_Planning.Id_Planning.Substring(5);
                db.Entry(compteurPiece).State = EntityState.Modified;


                db.Crm_Planning.Add(crm_Planning);
                db.SaveChanges();
                int k = 0;
                foreach (var item in developpeur)
                {

                    for (int j = 0; j < 6; j++)
                {
                        Crm_PlanningTime crm_PlanningTime = new Crm_PlanningTime();
                        crm_PlanningTime.Id_Plannig = crm_Planning.Id_Planning;
                        crm_PlanningTime.CodeResponsable = item;
                        crm_PlanningTime.ObjectisNonRealiser = OBJ[k];

                        if (j==0)crm_PlanningTime.Date = DateTime.Parse(Day1);
                        if (j == 1) crm_PlanningTime.Date = DateTime.Parse(Day2);
                        if (j == 2) crm_PlanningTime.Date = DateTime.Parse(Day3);
                        if (j == 3) crm_PlanningTime.Date = DateTime.Parse(Day4);
                        if (j == 4) crm_PlanningTime.Date = DateTime.Parse(Day5);
                        if (j == 5) crm_PlanningTime.Date = DateTime.Parse(Day6);

                        if (j == 0)
                        {
                            crm_PlanningTime.Titre = MatinO1[k] + " " + ApresMidiO1[k];
                            crm_PlanningTime.Matin = MatinO1[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO1[k];
                            crm_PlanningTime.MatinReel = MatinR1[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR1[k];
                        }
                        if (j == 1)
                        { 
                            crm_PlanningTime.Titre =MatinO2[k] + " " + ApresMidiO2[k];
                            crm_PlanningTime.Matin = MatinO2[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO2[k];
                            crm_PlanningTime.MatinReel = MatinR2[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR2[k];
                        
                        }
                        if (j == 2) 
                        { 
                            crm_PlanningTime.Titre =MatinO3[k] + " " + ApresMidiO3[k];
                            crm_PlanningTime.Matin = MatinO3[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO3[k];
                            crm_PlanningTime.MatinReel= MatinR3[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR3[k];
                            
                        }
                        if (j == 3) 
                        {
                            crm_PlanningTime.Titre =MatinO4[k] + " " + ApresMidiO4[k];
                            crm_PlanningTime.Matin = MatinO4[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO4[k];
                            crm_PlanningTime.MatinReel = MatinR4[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR4[k];
                         
                        }
                        if (j == 4) 
                        { 
                            crm_PlanningTime.Titre =MatinO5[k] + " " + ApresMidiO5[k];
                            crm_PlanningTime.Matin = MatinO5[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO5[k];
                            crm_PlanningTime.MatinReel = MatinR5[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR5[k];
                          
                        }
                        if (j == 5)
                            {
                                crm_PlanningTime.Titre = MatinO6[k] + " " + ApresMidiO6[k];
                                crm_PlanningTime.Matin = MatinO6[k];
                                crm_PlanningTime.ApresMidi = ApresMidiO6[k];
                                crm_PlanningTime.MatinReel = MatinR6[k];
                                crm_PlanningTime.ApresMidiReel = ApresMidiR6[k];
                            }
                       
                        db.Crm_PlanningTime.Add(crm_PlanningTime);
                        db.SaveChanges();
                    }
               

                    k++;
                }
                TempData["SuccessMesage"] = "Plan N°" + crm_Planning.Id_Planning + " est enregistrée! ";
                return RedirectToAction("Index");
            }

            return View(crm_Planning);
        }

        // GET: Crm_Planning/Edit/5
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

            var ListResponsable = new SelectList(db.Respensable.ToList(), "Nom", "Nom", @Session["UserNom"].ToString() + " " + @Session["UserPrenom"].ToString());

            ViewData["ListResponsable"] = ListResponsable;
            Crm_Planning crm_Planning = db.Crm_Planning.Find(id);
            //Respensable respensable = db.Respensable.Find(crm_Planning.CodeResponsable);
            var list = db.Crm_PlanningTime.Where(c => c.Id_Plannig == id).ToList();
            string[] rsp = new string[list.Count];
            string[] obj = new string[list.Count];

            var x = ""; var y = "";
            for (int i = 0, j = 0; i < list.Count && j < rsp.Length; i++, j++)
            {

                if (x != list[i].CodeResponsable)
                {

                    rsp[i] = list[j].CodeResponsable;
                    x = rsp[i];
                }

                if (y != list[i].ObjectisNonRealiser)
                {

                    obj[i] = list[j].ObjectisNonRealiser;
                    y = obj[i];

                }

            }

           

            //ViewData["Nom"]= respensable.Nom; 
            ViewData["DayListe"] = list;
            ViewData["ListeRsp"] = rsp.Where(a => !string.IsNullOrEmpty(a)).ToArray().ToList();
            ViewData["ListeOBJ"] = obj.Where(a => !string.IsNullOrEmpty(a)).ToArray().ToList();

            if (ViewData["ListeOBJ"] == null)
            {
                var n = 0;
                foreach (var item in ViewData["ListeRsp"] as List<string>)
                {
                    if (obj[n] == null)
                    {
                        obj[n] = "En cours";
                        y = obj[n];
                    }
                    n++;
                }
                ViewData["ListeOBJ"] = obj.ToList();

            }
         
            if (crm_Planning == null)
            {
                return HttpNotFound();
            }
            return View(crm_Planning);
        }

        // POST: Crm_Planning/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Planning,Datedebut,DateFin,CodeResponsable")] Crm_Planning crm_Planning, string Day_0, string Day_1, string Day_2, string Day_3, string Day_4, string Day_5, string[] MatinO1, string[] MatinO2, string[] MatinO3, string[] MatinO4, string[] MatinO5, string[] MatinO6, string[] ApresMidiO1, string[] ApresMidiO2, string[] ApresMidiO3, string[] ApresMidiO4, string[] ApresMidiO5, string[] ApresMidiO6, string[] MatinR1, string[] MatinR2, string[] MatinR3, string[] MatinR4, string[] MatinR5, string[] MatinR6, string[] ApresMidiR1, string[] ApresMidiR2, string[] ApresMidiR3, string[] ApresMidiR4, string[] ApresMidiR5, string[] ApresMidiR6, string[] OBJ, string[] developpeur)
        {
            if (ModelState.IsValid)
            {
                crm_Planning.Description = "";
                crm_Planning.Nom = "";
                crm_Planning.Prenom = "";
                crm_Planning.Titre = "";
                crm_Planning.CodeClient = "";
                crm_Planning.RaisonSocial = "";



                /*delete from table timreplan*/
                List<Crm_PlanningTime> PLANTIME = db.Crm_PlanningTime.Where(c => c.Id_Plannig == crm_Planning.Id_Planning).ToList();
                foreach (var item in PLANTIME)
                {
                    db.Crm_PlanningTime.Remove(item);
                    db.SaveChanges();
                }
                /*add fter delete*/
                int k = 0;
                foreach (var item in developpeur)
                {

                    for (int j = 0; j < 6; j++)
                    {
                        Crm_PlanningTime crm_PlanningTime = new Crm_PlanningTime();
                        crm_PlanningTime.Id_Plannig = crm_Planning.Id_Planning;
                        crm_PlanningTime.CodeResponsable = item;
                        if (OBJ[k] != null)
                        { crm_PlanningTime.ObjectisNonRealiser = OBJ[k]; }
                        else
                        { OBJ[k] = ""; }
                        if (j == 0) crm_PlanningTime.Date = DateTime.Parse(Day_0);
                        if (j == 1) crm_PlanningTime.Date = DateTime.Parse(Day_1);
                        if (j == 2) crm_PlanningTime.Date = DateTime.Parse(Day_2);
                        if (j == 3) crm_PlanningTime.Date = DateTime.Parse(Day_3);
                        if (j == 4) crm_PlanningTime.Date = DateTime.Parse(Day_4);
                        if (j == 5) crm_PlanningTime.Date = DateTime.Parse(Day_5);


                        if (j == 0)
                        {
                            crm_PlanningTime.Titre = MatinO1[k] + " " + ApresMidiO1[k];
                            crm_PlanningTime.Matin = MatinO1[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO1[k];
                            crm_PlanningTime.MatinReel = MatinR1[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR1[k];
                        }
                        if (j == 1)
                        {
                            crm_PlanningTime.Titre = MatinO2[k] + " " + ApresMidiO2[k];
                            crm_PlanningTime.Matin = MatinO2[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO2[k];
                            crm_PlanningTime.MatinReel = MatinR2[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR2[k];
                         
                        }
                        if (j == 2)
                        {
                            crm_PlanningTime.Titre = MatinO3[k] + " " + ApresMidiO3[k];
                            crm_PlanningTime.Matin = MatinO3[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO3[k];
                            crm_PlanningTime.MatinReel = MatinR3[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR3[k];
                        }
                        if (j == 3)
                        {
                            crm_PlanningTime.Titre = MatinO4[k] + " " + ApresMidiO4[k];
                            crm_PlanningTime.Matin = MatinO4[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO4[k];
                            crm_PlanningTime.MatinReel = MatinR4[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR4[k];
                        }
                        if (j == 4)
                        {
                            crm_PlanningTime.Titre = MatinO5[k] + " " + ApresMidiO5[k];
                            crm_PlanningTime.Matin = MatinO5[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO5[k];
                            crm_PlanningTime.MatinReel = MatinR5[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR5[k];
                        }
                        if (j == 5)
                        {
                            crm_PlanningTime.Titre = MatinO6[k] + " " + ApresMidiO6[k];
                            crm_PlanningTime.Matin = MatinO6[k];
                            crm_PlanningTime.ApresMidi = ApresMidiO6[k];
                            crm_PlanningTime.MatinReel = MatinR6[k];
                            crm_PlanningTime.ApresMidiReel = ApresMidiR6[k];
                        }


                        db.Crm_PlanningTime.Add(crm_PlanningTime);
                        db.SaveChanges();

                    }
                    k++;
                }


                db.Entry(crm_Planning).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMesage"] = "Plan N°" + crm_Planning.Id_Planning + " est modifiée! ";
                return RedirectToAction("Index");
            }
            return View(crm_Planning);
        }

        // GET: Crm_Planning/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_Planning crm_Planning = db.Crm_Planning.Find(id);
            if (crm_Planning == null)
            {
                return HttpNotFound();
            }
            return View(crm_Planning);
        }

        // POST: Crm_Planning/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            List <Crm_PlanningTime> PLANTIME = db.Crm_PlanningTime.Where(c => c.Id_Plannig == id).ToList();
            foreach (var item in PLANTIME) {
                db.Crm_PlanningTime.Remove(item);
                db.SaveChanges();
            }
            Crm_Planning crm_Planning = db.Crm_Planning.Find(id);
            db.Crm_Planning.Remove(crm_Planning);
            db.SaveChanges();
            //TempData["SuppressionMessage"] = "Plan N°" + crm_Planning.Id_Planning + " est supprimée! ";
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

        public List<Respensable> ResponsableData()
        {
            return db.Respensable.ToList();
        }
    }
}
