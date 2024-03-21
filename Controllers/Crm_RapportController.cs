using CRMSTUBSOFT.Services.Business;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_RapportController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: Crm_Rapport
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d1"] = d1;
            ViewData["d2"] = d2;

            return View(db.Crm_Rapport.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchIndex()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var model = from r in db.Crm_Rapport
                        where r.DateCreation > d1 && r.DateCreation < d2
                        select r;
            return View("Index", model.ToList());
        }
        // GET: Crm_Rapport/Details/5
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

            Crm_Rapport crm_Rapport = db.Crm_Rapport.Find(id);
            ViewData["code"] = id;
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            /*Intialize required field*
            crm_Rapport.Duree = 0;
            var ListUtilisateur = new SelectList(db.Respensable.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            /*Liste des taches liéé aux client du rapport*/
            var model = (from c in db.Crm_TacheReclamation
                         orderby c.NumeroOrdre
                         where c.CodeClient == crm_Rapport.CodeClient && c.NumDossier == ""
                         select c.NumeroTache).ToList();
            
            List<Crm_TacheReclamation> crm_TacheReclamation = new List<Crm_TacheReclamation>();

            foreach (var item in model)
            {
                Crm_TacheReclamation crm_TacheReclamation1 = db.Crm_TacheReclamation.Find(item);
                crm_TacheReclamation.Add(crm_TacheReclamation1);
            }
            ViewData["ListTacheClient"] = crm_TacheReclamation;


            /*Liste des taches liéé aux client du rapport*/
            var model1 = (from c in db.Crm_TacheReclamation
                          orderby c.NumeroOrdre
                          where c.NumDossier == crm_Rapport.NumeroRapport
                         select c.NumeroTache).ToList();
            List<Crm_TacheReclamation> crm_TacheReclamationrapport = new List<Crm_TacheReclamation>();
            foreach (var item in model1)
            {
                Crm_TacheReclamation crm_TacheReclamation1 = db.Crm_TacheReclamation.Find(item);
                crm_TacheReclamationrapport.Add(crm_TacheReclamation1);
            }
            if (crm_TacheReclamation != null)
                ViewData["ListTacheRapport"] = crm_TacheReclamationrapport;
            else
                ViewData["ListTacheRapport"] = " ";
            if (crm_Rapport == null)
            {
                return HttpNotFound();
            }

            return View(crm_Rapport);
        }


        [HttpPost]
        public ActionResult Details(string[] ListRapportTache, string id)
        {
            Crm_Rapport crm_Rapport = db.Crm_Rapport.Find(id);
         
            if (ModelState.IsValid)
            {
                foreach (var item in ListRapportTache)
                {
                    //search tache
                    Crm_TacheReclamation crm_TacheReclamationn = db.Crm_TacheReclamation.Find(item);

                    //modifie numdossier tache with current rapporrtid
                    crm_TacheReclamationn.NumDossier = id;

                    var model3 = (from c in db.Crm_TacheReclamation
                                 select c.NumeroOrdre).ToList();

                    crm_TacheReclamationn.NumeroOrdre = model3.Max() + 1;

                    db.Entry(crm_TacheReclamationn).State = EntityState.Modified;
                    db.SaveChanges();

                    //delete numtache from list
                     ListRapportTache.ToList().Remove(item);
                     ListRapportTache = ListRapportTache.ToList().ToArray();
                }
            }
            //File rapport with lists
            //DropdownList
            ViewData["code"] = id;
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            /*Intialize required field*/
             crm_Rapport.Duree = 0;            var ListUtilisateur = new SelectList(db.Respensable.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            /*Liste des taches liéé aux client du rapport*/
            var model = (from c in db.Crm_TacheReclamation
                         orderby c.NumeroOrdre
                         where c.CodeClient == crm_Rapport.CodeClient && c.NumDossier == ""
                         select c.NumeroTache).ToList();

            List<Crm_TacheReclamation> crm_TacheReclamation = new List<Crm_TacheReclamation>();

            foreach (var item in model)
            {
                Crm_TacheReclamation crm_TacheReclamation1 = db.Crm_TacheReclamation.Find(item);
                crm_TacheReclamation.Add(crm_TacheReclamation1);
            }
            ViewData["ListTacheClient"] = crm_TacheReclamation;


            /*Liste des taches liéé aux client du rapport*/
            var model1 = (from c in db.Crm_TacheReclamation
                          orderby c.NumeroOrdre
                          where c.NumDossier == crm_Rapport.NumeroRapport
                          select c.NumeroTache
                          ).ToList();

            List<Crm_TacheReclamation> crm_TacheReclamationrapport = new List<Crm_TacheReclamation>();
            foreach (var item in model1)
            {
                Crm_TacheReclamation crm_TacheReclamation1 = db.Crm_TacheReclamation.Find(item);
                crm_TacheReclamationrapport.Add(crm_TacheReclamation1);
            }
         
            if (crm_TacheReclamation != null)
                ViewData["ListTacheRapport"] = crm_TacheReclamationrapport;
            else
                ViewData["ListTacheRapport"] = " ";
            if (crm_Rapport == null)
            {
                return HttpNotFound();
            }
      //End File rapport width lists
            return View(crm_Rapport);
        }

        [HttpPost]
        public ActionResult RapportOrder(string[] ListeOrder, string[] ListTache, string check, string id)
        {
           
            if(ListeOrder==null) ListeOrder = new string[] {""};
            if (ListTache == null) ListTache = new string[] { "" };

            Crm_Rapport crm_Rapport = db.Crm_Rapport.Find(id);
            if (ModelState.IsValid)
            {
                if (check == "true")
                {
                    for (int i = 0; i < ListeOrder.Count(); i++)
                    {
                        Crm_TacheReclamation crm_TacheReclamationn = db.Crm_TacheReclamation.Find(ListTache[i]);
                        crm_TacheReclamationn.NumeroOrdre = i + 1;
                        db.Entry(crm_TacheReclamationn).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                else
                {
                    for (int i = 0, j = 1; i < ListeOrder.Count() && j < ListeOrder.Count(); i++, j++)
                        {
                        Crm_TacheReclamation crm_TacheReclamationn = db.Crm_TacheReclamation.Find(ListTache[i]);
                        if (ListeOrder[i] == ListeOrder[j])
                        {
                            TempData["errorMesage"] = "Vérifier votre ordre! ";
                        }
                        else
                        {
                           
                                crm_TacheReclamationn.NumeroOrdre = Int32.Parse(ListeOrder[i]);
                                db.Entry(crm_TacheReclamationn).State = EntityState.Modified;
                                db.SaveChanges();
                     
                            
                        }
                        }
                   
                }
            

            }
            //File rapport with lists
            //DropdownList
            ViewData["code"] = id;
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            /*Intialize required field*/
            
             crm_Rapport.Duree = 0;
            var ListUtilisateur = new SelectList(db.Respensable.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            /*Liste des taches liéé aux client du rapport*/
            var model = (from c in db.Crm_TacheReclamation
                         orderby c.NumeroOrdre
                         where c.CodeClient == crm_Rapport.CodeClient && c.NumDossier == ""
                         select c.NumeroTache).ToList();

            List<Crm_TacheReclamation> crm_TacheReclamation = new List<Crm_TacheReclamation>();

            foreach (var item in model)
            {
                Crm_TacheReclamation crm_TacheReclamation1 = db.Crm_TacheReclamation.Find(item);
                crm_TacheReclamation.Add(crm_TacheReclamation1);
            }
            ViewData["ListTacheClient"] = crm_TacheReclamation;


            /*Liste des taches liéé aux client du rapport*/
            var model1 = (from c in db.Crm_TacheReclamation
                          orderby c.NumeroOrdre
                          where c.NumDossier == crm_Rapport.NumeroRapport
                          select c.NumeroTache).ToList();
            List<Crm_TacheReclamation> crm_TacheReclamationrapport = new List<Crm_TacheReclamation>();
            foreach (var item in model1)
            {
                Crm_TacheReclamation crm_TacheReclamation1 = db.Crm_TacheReclamation.Find(item);
                crm_TacheReclamationrapport.Add(crm_TacheReclamation1);
            }

            if (crm_TacheReclamation != null)
                ViewData["ListTacheRapport"] = crm_TacheReclamationrapport;
            else
                ViewData["ListTacheRapport"] = " ";

            return RedirectToAction("Details", "Crm_Rapport", new { id = id });
        }

        // GET: Crm_Rapport/Create
        public ActionResult Create()
        {

            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }


            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            /*Intialize required field*/
            Crm_Rapport crm_Rapport = new Crm_Rapport();
            crm_Rapport.DateCreation = DateTime.Now;
            crm_Rapport.Duree = 0;            var ListUtilisateur = new SelectList(db.Respensable.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("RP", "220").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

            crm_Rapport.NumeroRapport= "RP" + "220" + valeur.ToString("00000");



            return View(crm_Rapport);
        }

        // POST: Crm_Rapport/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NumeroRapport,DescriptionRapport,DateCreation,UtilisateurCreateur,Degres,NumeroEtat,NumeroOrdre,DatePrevusProchaineReunion,Duree,Status,CodeClient,RaisonSociale,NumeroDossier,DateRapport")] Crm_Rapport crm_Rapport)
        {
            if (ModelState.IsValid)
            {
                crm_Rapport.NumeroDossier = " ";
                crm_Rapport.NumeroEtat = " ";
                crm_Rapport.Status = "E05";
                crm_Rapport.UtilisateurCreateur = Session["UserID"].ToString();
                crm_Rapport.DateCreation = DateTime.Now;
                /*Control IdBase*/


                CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "crm_rapport");
                decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur); decimal valeur2 = Convert.ToDecimal(crm_Rapport.NumeroRapport.Substring(4));

                if (valeur2 - valeur1 > 0)
                {
                    crm_Rapport.NumeroRapport = "RP" + "220" + valeur2.ToString("00000");

                }
                else
                {

                    crm_Rapport.NumeroRapport = "RP" + "220" + (valeur1 + 1).ToString("00000");

                }

                compteurPiece = db.CompteurPiece.Find("SOC001", "crm_rapport");
                compteurPiece.Compteur = crm_Rapport.NumeroRapport.Substring(4);
                db.Entry(compteurPiece).State = EntityState.Modified;
                db.Crm_Rapport.Add(crm_Rapport);
                db.SaveChanges();
                TempData["SuccessMesage"] = "Rapport N°" + @crm_Rapport.NumeroRapport + " est enregistrée! ";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                // do something with the error list :)
            }
            else
            {
                //DropdownList
                var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
                ViewData["ListClient"] = ListClients;
            }
            return View(crm_Rapport);

        }

        // GET: Crm_Rapport/Edit/5
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
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            Crm_Rapport crm_Rapport = db.Crm_Rapport.Find(id);
            if (crm_Rapport == null)
            {
                return HttpNotFound();
            }


            var d1 = DateTime.Parse(crm_Rapport.DateRapport.ToString());
            

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");


            return View(crm_Rapport);
        }

        // POST: Crm_Rapport/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NumeroRapport,DescriptionRapport,DateCreation,UtilisateurCreateur,Degres,NumeroEtat,NumeroOrdre,DatePrevusProchaineReunion,Duree,Status,CodeClient,RaisonSociale,NumeroDossier,DateRapport")] Crm_Rapport crm_Rapport)
        {
            if (ModelState.IsValid)
            {
                crm_Rapport.NumeroDossier = " ";
                crm_Rapport.NumeroEtat = " ";
                crm_Rapport.Status = "E05";
                crm_Rapport.UtilisateurCreateur = Session["UserID"].ToString();
                db.Entry(crm_Rapport).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMesage"] = "Rapport N°" + @crm_Rapport.NumeroRapport + " est modifiée! ";
                return RedirectToAction("Index");
            }
            return View(crm_Rapport);
        }

        // GET: Crm_Rapport/Delete/5
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
            Crm_Rapport crm_Rapport = db.Crm_Rapport.Find(id);
            if (crm_Rapport == null)
            {
                return HttpNotFound();
            }
            return View(crm_Rapport);
        }

        // POST: Crm_Rapport/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Crm_Rapport crm_Rapport = db.Crm_Rapport.Find(id);
            db.Crm_Rapport.Remove(crm_Rapport);
            db.SaveChanges();
            //TempData["SuppressionMessage"] = "Rapport N°" + @crm_Rapport.NumeroRapport + " est supprimée! ";
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

        //Filtrage
        public ActionResult Search(string Status, string d1, string d2)
        {

            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);

            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var model = from r in db.Crm_Rapport
                        where r.NumeroEtat == Status && r.DateCreation > d11 && r.DateCreation < d22
                        select r;

            return View("Index", model);
        }

        public ActionResult SearchDate(string d1, string d2)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }


            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);

            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var model = from r in db.Crm_Rapport
                        where r.DateCreation > d11 && r.DateCreation < d22
                        select r;

            return View("Index", model);
        }

        public ActionResult Impression()
        {

            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }


            //<Crm_TacheReclamation> allCustomer = new List<Crm_TacheReclamation>();
            //allCustomer = db.Crm_TacheReclamation.ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_RapportParClient.rpt"));

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            //rd.SetDataSource(allCustomer);

            // rd.SetParameterValue("NomSociete", "GROUP MAS");
            //rd.SetParameterValue("Etat", "Vaider");
            // rd.RecordSelectionFormula = " {Crm_Rapport.NumeroRapport}='"++""'";



            // choisir quel type de document à télécharger
            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            // Stream stream = rd.ExportToStream(ExportFormatType.WordForWindowsl);
            //Stream stream = rd.ExportToStream(ExportFormatType.Excel);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            // type de document à construire
            Response.ContentType = "application/pdf";
            //Response.ContentType = "application/doc";
            //Response.ContentType = "application/vnd.ms-excel";

            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, (int)stream.Length);

            stream.Read(buffer, 0, (int)stream.Length);

            Response.BinaryWrite(buffer);

            Response.End();

            // stream.Seek(0, SeekOrigin.Begin);

            // retour téléchargement pdf
            return File(stream, "application/pdf", "CustomerList.pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");
        }
       

        protected void setDbInfo(ReportDocument rptDoc, string Server, string dbName, string UserId, string Pwd)
        {

            TableLogOnInfo logoninfo = new TableLogOnInfo();
            foreach (Table tbl in rptDoc.Database.Tables)
            {
                logoninfo = tbl.LogOnInfo;
                logoninfo.ReportName = rptDoc.Name;
                logoninfo.ConnectionInfo.ServerName = Server;
                logoninfo.ConnectionInfo.DatabaseName = dbName;
                logoninfo.ConnectionInfo.UserID = UserId;
                logoninfo.ConnectionInfo.Password = Pwd;
                logoninfo.TableName = "Crm_Rapport";
                tbl.ApplyLogOnInfo(logoninfo);
                tbl.Location = "Crm_Rapport";
            }
        }

        // téléchargement pdf excel word
        
        public ActionResult ImpressionRapport ()
        {
            string docType = "";
            string fileName = "";
            ExportFormatType formatFile = new ExportFormatType();


            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_RapportParClient.rpt"));

            
            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            // rd.SetParameterValue("NomSociete", "GROUP MAS");

            // condition filtre
           
            string etat = "";
            string NumeroRapport= "";
            string duree = "";
            string ConditionFiltre = "";
            if (Request["NumeroRapport"]  != "")
            {
                NumeroRapport = Request["NumeroRapport"];
            }
            if (Request["etat"] != "")
            {
                etat = Request["etat"];
            }
            if (Request["duree"] != "")
            {
                duree = Request["duree"];
            }
            if (Request["duree"] != "")
            {
                ConditionFiltre = " {Crm_Rapport.NumeroRapport} = '" + NumeroRapport + "' ";
            }

            string Etats = "";
            if (etat == "E11")// non commencer
            {
                if (etat == "E02")//en cour
                {
                    if (etat == "E01")// terminer
                    {
                        ConditionFiltre = ConditionFiltre + " and  ( {Crm_Rapport.NumeroRapport} = '" + NumeroRapport + "' or {Crm_Rapport.NumeroRapport} = '" + NumeroRapport + "' or  {Crm_Rapport.NumeroEtat} = '" + etat + "')";
                        Etats = "tous/nonCommencer/terminer/enCour";
                    }
                    else
                    {
                        ConditionFiltre = ConditionFiltre + " and  ({Crm_Rapport.NumeroEtat} = '" + etat+ "' or {Crm_Rapport.NumeroEtat} = '" + etat + "' )";
                        Etats = "nonCommencer/terminer/enCour";
                    }
                }
                else
                {
                    if (etat == "E01")
                    {
                        ConditionFiltre = ConditionFiltre + " and  ({Crm_Rapport.NumeroEtat} = '" + etat + "'  or  {Crm_Rapport.NumeroEtat} = '" + etat + "')";
                        Etats = "nonCommencer/terminer";
                    }
                    else
                    {
                        ConditionFiltre = ConditionFiltre + " and  {Crm_Rapport.NumeroEtat} = '" + etat + "'";
                        Etats = "nonCommencer";
                    }
                }
            }
            else
            {
                if (etat == "E02")
                {
                    if (etat == "E01")
                    {
                        ConditionFiltre = ConditionFiltre + " and  ({Crm_Rapport.NumeroEtat} = '" + etat + "' or  {Crm_Rapport.NumeroEtat} = '" + etat + "')";
                        Etats = "enCours/terminer";
                    }
                    else
                    {
                        ConditionFiltre = ConditionFiltre + " and {Crm_Rapport.NumeroEtat} = '" + etat + "'";
                        Etats = "EnCour";
                    }
                }
                else
                {
                    if (etat == "E01")
                    {
                        ConditionFiltre = ConditionFiltre + " and  {Crm_Rapport.NumeroEtat} = '" + etat + "'";
                        Etats = "terminer";
                    }
                }
            }
            
          
            rd.SetParameterValue("Etat", Etats);
            rd.RecordSelectionFormula = ConditionFiltre;

            // diiferent format

            if (Request["Format"] == "PDF")
            {
                formatFile = ExportFormatType.PortableDocFormat;
                docType = "application/pdf";
                fileName = "CustomerList.pdf";
            }
            if(Request["Format"] == "EXCEL")
            {
                formatFile = ExportFormatType.Excel;
                docType = "application/vnd.ms-excel";
                fileName = "CustomerList.xls";
            }
            if(Request["Format"] == "WORD")
            {
                formatFile = ExportFormatType.WordForWindows;
                docType = "application/doc";
                fileName = "CustomerList.doc";
            }


            Stream stream = rd.ExportToStream(formatFile);
            Response.ContentType = docType;

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            stream.Seek(0, SeekOrigin.Begin);


            return File(stream, docType, fileName);

        }



    }

}

