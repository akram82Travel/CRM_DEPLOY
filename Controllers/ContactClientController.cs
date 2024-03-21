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
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Windows;
using Microsoft.Owin;
using System.Net.Mail;

namespace CRMSTUBSOFT.Controllers
{
    public class ContactClientController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();

        // GET: ContactClients/index/d1/d2
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

            /*var result = from a in db.Crm_ReclamationClient
                         join b in db.ContratClient on a.CodeClient equals b.CodeClient into CC
                         from b in CC.DefaultIfEmpty()
                         select new
                         {
                             a.CodeClient,
                             Actif = b == null ? "Sans contrat" : "Avec Contrat",
                         };*/
           
            ViewData["ListContrat"] = db.ContratClient.ToList();
           
            return View(db.Crm_ReclamationClient);
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

            var model = from r in db.Crm_ReclamationClient
                        where r.DateCreation > d1 && r.DateCreation < d2
                        select r;
            return View("Index", model.ToList());
        }

        // GET: ContactClient/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            //DropdownList

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClients"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            //var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
           // ViewData["ListeContactClient"] = ListeContactClient;

            var ListCreateur = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListCreateur"] = ListCreateur;

            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            crm_ReclamationClient.Listclient = new List<Client>();
            crm_ReclamationClient.Listclient = ClientData();

            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();

            crm_ReclamationClient.ListeRespensables = new List<Respensable>();
            crm_ReclamationClient.ListeRespensables = ResponsableData();


            crm_ReclamationClient.ListeUtilisateur = new List<Utilisateur>();
            crm_ReclamationClient.ListeUtilisateur = UtilisateurData();

            var model = (from c in db.Crm_PersonneConcerner
                         where c.NumeroReclamation == id
                         select c.NomUtilisateur).ToList();


            crm_ReclamationClient.SelectedUtilisateurId = model;


            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }

        // GET: ContactClient/Create
        public ActionResult Create()
        {

            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClients"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            //var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            //ViewData["ListeContactClient"] = ListeContactClient;

            var ListCreateur = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListCreateur"] = ListCreateur;

            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

            crm_ReclamationClient.Listclient = new List<Client>();
            crm_ReclamationClient.Listclient = ClientData();

            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();

            crm_ReclamationClient.ListeRespensables = new List<Respensable>();
            crm_ReclamationClient.ListeRespensables = ResponsableData();

            crm_ReclamationClient.ListeUtilisateur = new List<Utilisateur>();
            crm_ReclamationClient.ListeUtilisateur = UtilisateurData();

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("REC", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

            crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur.ToString("00000");

            return View(crm_ReclamationClient);


        }

        // POST: ContactClient/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Reclamation,Titre,Description,DateReclamation,CodeClient,RaisonSociale,NomContact,CodeMoyenCommunication,OutilCommunication,Observation,Createur,DateCreation,logo,Status,NumeroDossier,PrisEnChargePar,DatePrisEnCharge,ObjetReclamation,Paye,TypeReclamation,Duree,UtilisateurContact,Nature")] Crm_ReclamationClient crm_ReclamationClient, IEnumerable<string> SelectedUtilisateurId)
        {

            if (ModelState.IsValid)
            {
                /*Begin intialize data*/
                //crm_ReclamationClient.OutilCommunication = "";
                crm_ReclamationClient.TypeReclamation = "";
                crm_ReclamationClient.PrisEnChargePar = "";
                crm_ReclamationClient.DatePrisEnCharge = "";
                crm_ReclamationClient.ObjetReclamation = "";
                crm_ReclamationClient.NumeroDossier = "";
                crm_ReclamationClient.Paye = false;
                crm_ReclamationClient.logo = "";
                crm_ReclamationClient.Status = "E05";
                crm_ReclamationClient.Createur = Session["UserID"].ToString();
                crm_ReclamationClient.DateCreation = DateTime.Now;
                //crm_ReclamationClient.UtilisateurContact = "";
                
                /*End intialize data*/
                /*Initialize Data to avoid NULL Exception*/
                if (crm_ReclamationClient.Titre == null)
                    crm_ReclamationClient.Titre = "";
                if (crm_ReclamationClient.Description == null)
                    crm_ReclamationClient.Description = "";
                if (crm_ReclamationClient.Observation == null)
                    crm_ReclamationClient.Observation = "";
                if (crm_ReclamationClient.NomContact == 0)
                    crm_ReclamationClient.NomContact = 0;

                CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_ReclamationClient");
                decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur); decimal valeur2 = Convert.ToDecimal(crm_ReclamationClient.Id_Reclamation.Substring(5));

                if (valeur2 - valeur1 > 0)
                {
                    crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur2.ToString("00000");

                }
                else
                {

                    crm_ReclamationClient.Id_Reclamation = "REC" + "210" + (valeur1 + 1).ToString("00000");

                }

                compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_ReclamationClient");
                compteurPiece.Compteur = crm_ReclamationClient.Id_Reclamation.Substring(5);
                db.Entry(compteurPiece).State = EntityState.Modified;

                /**/
                TempData["ConfimationMesage"] = "Voulez vous générer une tâche à ce contact?";

                db.Crm_ReclamationClient.Add(crm_ReclamationClient);
                db.SaveChanges();

                /*Add list respensable to Database*/
                Crm_PersonneConcerner crm_PersonneConcerner = new Crm_PersonneConcerner();
                foreach (var item in SelectedUtilisateurId)
                {
                    crm_PersonneConcerner = new Crm_PersonneConcerner();
                    crm_PersonneConcerner.NomUtilisateur = item;
                    crm_PersonneConcerner.NumeroReclamation = crm_ReclamationClient.Id_Reclamation;
                    crm_PersonneConcerner.Vue = false;
                    crm_PersonneConcerner.DateVue = "";
                    db.Crm_PersonneConcerner.Add(crm_PersonneConcerner);
                    db.SaveChanges();
                }



                //Alert messaage d'ajout

                //TempData["SuccessMesage"] = "Contact Client N°" + @crm_ReclamationClient.Id_Reclamation + " est enregistrée! ";
                //return RedirectToAction("Index");

                //DropdownList
                var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
                ViewData["ListClients"] = ListClients;

                var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
                ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

                //var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
                //ViewData["ListeContactClient"] = ListeContactClient;

                var ListCreateur = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
                ViewData["ListCreateur"] = ListCreateur;

                var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
                ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

                var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
                ViewData["ListNature"] = ListNature;

                crm_ReclamationClient.Listclient = new List<Client>();
                crm_ReclamationClient.Listclient = ClientData();

                crm_ReclamationClient.ListContactClient = new List<ContactClient>();
                crm_ReclamationClient.ListContactClient = ContactClientData();

                crm_ReclamationClient.ListeRespensables = new List<Respensable>();
                crm_ReclamationClient.ListeRespensables = ResponsableData();

                crm_ReclamationClient.ListeUtilisateur = new List<Utilisateur>();
                crm_ReclamationClient.ListeUtilisateur = UtilisateurData();
            }

            return View(crm_ReclamationClient);
        }

        // GET: ContactClient/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            //DropdownList

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClients"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;

            var ListCreateur = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListCreateur"] = ListCreateur;

            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            crm_ReclamationClient.Listclient = new List<Client>();
            crm_ReclamationClient.Listclient = ClientData();

            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();

            crm_ReclamationClient.ListeRespensables = new List<Respensable>();
            crm_ReclamationClient.ListeRespensables = ResponsableData();

            crm_ReclamationClient.ListeUtilisateur = new List<Utilisateur>();
            crm_ReclamationClient.ListeUtilisateur = UtilisateurData();

            var model = (from c in db.Crm_PersonneConcerner
                         where c.NumeroReclamation == id
                         select c.NomUtilisateur).ToList();


            crm_ReclamationClient.SelectedUtilisateurId = model;

            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }

        // POST: ContactClient/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Reclamation,Titre,Description,DateReclamation,CodeClient,RaisonSociale,NomContact,CodeMoyenCommunication,OutilCommunication,Observation,Createur,DateCreation,logo,Status,NumeroDossier,PrisEnChargePar,DatePrisEnCharge,ObjetReclamation,Paye,TypeReclamation,Duree,UtilisateurContact,Nature")] Crm_ReclamationClient crm_ReclamationClient, IEnumerable<string> SelectedUtilisateurId)
        {
            if (ModelState.IsValid)
            {
                /*Begin intialize data*/
                crm_ReclamationClient.TypeReclamation = "";
                crm_ReclamationClient.PrisEnChargePar = "";
                crm_ReclamationClient.DatePrisEnCharge = "";
                crm_ReclamationClient.ObjetReclamation = "";
                crm_ReclamationClient.NumeroDossier = "";
                crm_ReclamationClient.Paye = false;
                crm_ReclamationClient.logo = "";
                crm_ReclamationClient.Status = "E05";
                crm_ReclamationClient.Createur = Session["UserID"].ToString();
                //crm_ReclamationClient.DateCreation = DateTime.Now;
                /*Initialize Data to avoid NULL Exception*/
                if (crm_ReclamationClient.Titre == null)
                    crm_ReclamationClient.Titre = "";
                if (crm_ReclamationClient.Description == null)
                    crm_ReclamationClient.Description = "";
                if (crm_ReclamationClient.Observation == null)
                    crm_ReclamationClient.Observation = "";
                if (crm_ReclamationClient.NomContact == 0)
                    crm_ReclamationClient.NomContact = 0;

                if (crm_ReclamationClient.OutilCommunication == null)
                    crm_ReclamationClient.OutilCommunication = "";

                /*Update selected responsable to base*/
                Crm_PersonneConcerner crm_PersonneConcerner = new Crm_PersonneConcerner();

                var model = (from c in db.Crm_PersonneConcerner
                             where c.NumeroReclamation == crm_ReclamationClient.Id_Reclamation
                             select c.NomUtilisateur).ToList();

                foreach (var item in model)
                {
                    crm_PersonneConcerner = db.Crm_PersonneConcerner.Find(crm_ReclamationClient.Id_Reclamation, item);
                    if (crm_PersonneConcerner.NumeroReclamation == crm_ReclamationClient.Id_Reclamation && crm_PersonneConcerner.NomUtilisateur == item)
                        db.Crm_PersonneConcerner.Remove(crm_PersonneConcerner);
                    db.SaveChanges();
                }


                foreach (var item in SelectedUtilisateurId)
                {
                    crm_PersonneConcerner = new Crm_PersonneConcerner();
                    crm_PersonneConcerner.NomUtilisateur = item;
                    crm_PersonneConcerner.NumeroReclamation = crm_ReclamationClient.Id_Reclamation;
                    crm_PersonneConcerner.Vue = false;
                    crm_PersonneConcerner.DateVue = "";
                    db.Crm_PersonneConcerner.Add(crm_PersonneConcerner);
                    db.SaveChanges();
                }

                db.Entry(crm_ReclamationClient).State = EntityState.Modified;
                db.SaveChanges();
                //Alert messaage modification
                TempData["SuccessMesage"] = "Contact Client N°" + @crm_ReclamationClient.Id_Reclamation + " est modifiée! ";
                return RedirectToAction("Index");
            }
            else
            {
                //DropdownList
                var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
                ViewData["ListClients"] = ListClients;

                var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
                ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

                var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
                ViewData["ListeContactClient"] = ListeContactClient;

                var ListCreateur = new SelectList(db.Crm_ReclamationClient.ToList(), "Id_Reclamation", "Createur");
                ViewData["ListCreateur"] = ListCreateur;

                var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
                ViewData["ListNature"] = ListNature;

                crm_ReclamationClient.Listclient = new List<Client>();
                crm_ReclamationClient.Listclient = ClientData();

                crm_ReclamationClient.ListContactClient = new List<ContactClient>();
                crm_ReclamationClient.ListContactClient = ContactClientData();

                crm_ReclamationClient.ListeRespensables = new List<Respensable>();
                crm_ReclamationClient.ListeRespensables = ResponsableData();
            }

            return View(crm_ReclamationClient);
        }

        // GET: ContactClient/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }

        // POST: ContactClient/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            Crm_PersonneConcerner crm_PersonneConcerner = new Crm_PersonneConcerner();

            var model = (from c in db.Crm_PersonneConcerner
                         where c.NumeroReclamation == crm_ReclamationClient.Id_Reclamation
                         select c.NomUtilisateur).ToList();

            foreach (var item in model)
            {
                crm_PersonneConcerner = db.Crm_PersonneConcerner.Find(crm_ReclamationClient.Id_Reclamation, item);
                if (crm_PersonneConcerner.NumeroReclamation == crm_ReclamationClient.Id_Reclamation && crm_PersonneConcerner.NomUtilisateur == item)
                    db.Crm_PersonneConcerner.Remove(crm_PersonneConcerner);
                db.SaveChanges();
            }

            db.Crm_ReclamationClient.Remove(crm_ReclamationClient);
            db.SaveChanges();
            ////Alert messaage modification
            //TempData["SuppressionMessage"] = "ContactClient N°" + @crm_ReclamationClient.Id_Reclamation + " est supprimée! ";
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

        public ActionResult Impression(string d1, string d2)
        {

            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }


            //<Crm_TacheReclamation> allCustomer = new List<Crm_TacheReclamation>();
            //allCustomer = db.Crm_TacheReclamation.ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_ListeContactClient.rpt"));

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);




            // condition filtre
            //string Etat = "";
            //string DateContact = "";

            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);

            string @DateDebut = d11.ToString("dd/MM/yyyy");
            string @DateFin = d22.ToString("dd/MM/yyyy");
            string ConditionFiltre = "";
            //rd.SetDataSource(allCustomer);

            rd.SetParameterValue("NomSociete", "GROUP MAS");
            rd.SetParameterValue("Etat", "Vaider");
            ConditionFiltre = "{Crm_ReclamationClient.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "'  ) ";
            rd.RecordSelectionFormula = ConditionFiltre;


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
            return File(stream, "application/pdf", "ContactList.pdf");
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
                logoninfo.TableName = "Crm_ReclamationClient";
                tbl.ApplyLogOnInfo(logoninfo);
                tbl.Location = "Crm_ReclamationClient";
            }
        }


        public List<Client> ClientData()
        {
            return db.Client.ToList();
        }
        public List<ContactClient> ContactClientData()
        {
            return db.ContactClient.ToList();
        }

        public List<Respensable> ResponsableData()
        {
            return db.Respensable.ToList();
        }
        public List<Utilisateur> UtilisateurData()
        {
            return db.Utilisateur.ToList();
        }



        public ActionResult CreateTacheReclamation(string id)
        {
            
            try
            {
                if (Session["UserNom"] == null)
                {
                    return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
                }
                if (Request.QueryString.AllKeys.Contains("id"))
                {
                    id = Request["id"].ToString();
                }

                Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Where(s => s.Id_Reclamation == id.ToString()).FirstOrDefault();

                Crm_TacheReclamation crm_TacheReclamations = new Crm_TacheReclamation();


                //// crm_TacheReclamation.NumReclamation = id.ToString();
                crm_TacheReclamations.CodeClient = crm_ReclamationClient.CodeClient.ToString();
                crm_TacheReclamations.RaisonSociale = crm_ReclamationClient.RaisonSociale;
                crm_TacheReclamations.TacheTitre = crm_ReclamationClient.Titre;
                crm_TacheReclamations.DescriptionTache = crm_ReclamationClient.Description;
                crm_TacheReclamations.NumPiece = 10;
                crm_TacheReclamations.CodeRespensable = crm_ReclamationClient.UtilisateurContact;
                crm_TacheReclamations.Nature = crm_ReclamationClient.Nature;
                crm_TacheReclamations.TypePiece = "1";


                if (crm_ReclamationClient.Soft == true)
                    crm_TacheReclamations.Nature = "S";
                if (crm_ReclamationClient.Technique == true)
                    crm_TacheReclamations.Nature = "T";
                if (crm_ReclamationClient.Commercial == true)
                    crm_TacheReclamations.Nature = "C";



                ////DropdownList
                var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
                ViewData["ListClient"] = ListClients;

                var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);

                ViewData["ListResponsable"] = ListResponsable;

                //MessageBox.Show(crm_ReclamationClient.UtilisateurContact.ToString());

                var ListResponsableAffecter = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", crm_ReclamationClient.UtilisateurContact);

                ViewData["ListResponsableAffecter"] = ListResponsableAffecter;



                var ListReclamation = new SelectList(db.Crm_ReclamationClient.ToList(), "Id_Reclamation", "Id_Reclamation", crm_ReclamationClient.Id_Reclamation.ToString());

                ViewData["ListReclamation"] = ListReclamation;

                var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumeroRapport", "NumeroRapport");
                ViewData["ListRapport"] = ListRapport;

                var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
                ViewData["ListEtat"] = ListEtat;

                var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
                ViewData["ListNature"] = ListNature;

                var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle");
                ViewData["ListMode"] = ListMode;

                var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
                ViewData["ListProjet"] = ListProjet;

                /*Primary key */
                SecurityServices securityService = new SecurityServices();
                var CompteurId = securityService.GetIdByCompteur("TD", "210").ToList();
                foreach (var item in CompteurId)
                {
                    ViewData["Compteur"] = item;
                }
                decimal valeur;
                valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

                crm_TacheReclamations.NumeroTache = "TD" + "210" + valeur.ToString("00000");

                crm_TacheReclamations.ListeRespensables = new List<Respensable>();
                crm_TacheReclamations.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
                crm_TacheReclamations.ListeRapport = new List<Crm_Rapport>();
                crm_TacheReclamations.ListeRespensables = ResponsableData();
                crm_TacheReclamations.ListeCrm_ReclamationClient = Crm_ReclamationData();
                crm_TacheReclamations.ListeRapport = RapportData();


                IEnumerable<string> result = crm_ReclamationClient.UtilisateurContact.Split(',').Select(s => s.Trim());

                crm_TacheReclamations.SelectedresponsableId = result;

                //Initialize Required variables
                crm_TacheReclamations.NumeroOrdre = 0;
                crm_TacheReclamations.Duree = DateTime.Now.ToString("t");
                ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
                crm_TacheReclamations.DatePrevus = DateTime.Now;
                crm_TacheReclamations.Degres = 0;
                crm_TacheReclamations.NumReclamation = crm_ReclamationClient.Id_Reclamation;

                return View(crm_TacheReclamations);
            }
            catch (SmtpFailedRecipientException ex)
            {
                //ex.FailedRecipient and ex.GetBaseException() should give you enough info.
                return RedirectToAction("Error", "CreateTacheReclamation", new { returnUrl = ex.GetBaseException() });
            }


           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTacheReclamation([Bind(Include = "NumeroTache,TacheTitre,DescriptionTache,Nature,Type,Degres,NomPlanificateur,NomValidateur,NomTesteur,CodeRespensable,Duree,Status,EtatValidation,DateCreation,UtilisateurCreateur,Paye,ValiderPar,CodeClient,RaisonSociale,ConfirmerPar,NonConfirmerPar,DateNonConfirmation,DatePrevus,DureeReel,NumDossier,NumReclamation,TypePiece")] Crm_TacheReclamation crm_TacheReclamation, IEnumerable<string> SelectedresponsableId)
        {
            if (ModelState.IsValid)
            {
               

                try
                {
                    crm_TacheReclamation.SelectedresponsableId = SelectedresponsableId;
                    if (SelectedresponsableId.Any())
                    {
                        crm_TacheReclamation.Status = "E11";
                    }
                    else
                    {
                        crm_TacheReclamation.Status = "E10";
                    }

                    crm_TacheReclamation.EtatValidation = "";
                    crm_TacheReclamation.UtilisateurCreateur = Session["UserID"].ToString();
                    crm_TacheReclamation.ValiderPar = "";
                    crm_TacheReclamation.ConfirmerPar = "";
                    crm_TacheReclamation.NonConfirmerPar = "";
                    crm_TacheReclamation.DateNonConfirmation = "";
                    crm_TacheReclamation.DureeReel = 0;
                    crm_TacheReclamation.AnuulerPar = "";
                    crm_TacheReclamation.DateAnnulation = "";
                    crm_TacheReclamation.DateConfirmation = "";
                    crm_TacheReclamation.DateDebutExecution = "";
                    crm_TacheReclamation.DurerExcutionValider = "";
                    crm_TacheReclamation.DateFinExecution = "";
                    crm_TacheReclamation.NumPiece = 10;
                    crm_TacheReclamation.CodeRespensable = "";
                    crm_TacheReclamation.DateCreation = DateTime.Now;
                    crm_TacheReclamation.DateCreationFin = DateTime.Now;
                    if (SelectedresponsableId.Any())
                    {
                        crm_TacheReclamation.NumeroEtat = "E08";
                    }
                    else
                    {
                        crm_TacheReclamation.NumeroEtat = "E05";
                    }

                    crm_TacheReclamation.TypePiece = "1";

                    crm_TacheReclamation.NumeroOrdre = 0;

                    if (crm_TacheReclamation.NumDossier != null)
                    {
                        var model = (from c in db.Crm_TacheReclamation
                                     select c.NumeroOrdre).ToList();

                        crm_TacheReclamation.NumeroOrdre = model.Max() + 1;
                    }
                    if (crm_TacheReclamation.NumDossier == null)
                    {
                        crm_TacheReclamation.NumDossier = "";
                    }
                    /*Control IdBase*/

                    
                    CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_TacheReclamation");
                    decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur); decimal valeur2 = Convert.ToDecimal(crm_TacheReclamation.NumeroTache.Substring(4));

                    if (valeur2 - valeur1 > 0)
                    {
                        crm_TacheReclamation.NumeroTache = "TD" + "210" + valeur2.ToString("00000");

                    }
                    else
                    {

                        crm_TacheReclamation.NumeroTache = "TD" + "210" + (valeur1 + 1).ToString("00000");

                    }

                    compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_TacheReclamation");
                    compteurPiece.Compteur = crm_TacheReclamation.NumeroTache.Substring(4);
                    db.Entry(compteurPiece).State = EntityState.Modified;

                    /*Add selected new tache to base*/
                    db.Crm_TacheReclamation.Add(crm_TacheReclamation);
                    db.SaveChanges();

                    /*Add list respensable to base*/
                    Crm_TacheLiee crm_TacheLiee = new Crm_TacheLiee();
                    foreach (var item in SelectedresponsableId)
                    {
                        crm_TacheLiee = new Crm_TacheLiee();
                        crm_TacheLiee.CodeRespensable = item;
                        crm_TacheLiee.NumeroTache = crm_TacheReclamation.NumeroTache;
                        db.Crm_TacheLiee.Add(crm_TacheLiee);
                        db.SaveChanges();
                    }


                    /*Add tache execution to base*/
                    Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                    Crm_TacheExcution.IdTache = crm_TacheReclamation.NumeroTache;
                    Crm_TacheExcution.Nomutilisateur = crm_TacheReclamation.UtilisateurCreateur;
                    Crm_TacheExcution.OldEtat = "";
                    Crm_TacheExcution.OldStatus = "";
                    Crm_TacheExcution.NewEtat = crm_TacheReclamation.NumeroEtat;
                    Crm_TacheExcution.NewStatus = crm_TacheReclamation.Status;
                    Crm_TacheExcution.DateExcution = DateTime.Now;
                    Crm_TacheExcution.DateFinExcution = DateTime.Now;
                    Crm_TacheExcution.Duree = 0;
                    Crm_TacheExcution.IdtacheExce = 0;
                    Crm_TacheExcution.DescriptionAcorriger = "";
                    Crm_TacheExcution.DescriptionAExpliquer = "";
                    Crm_TacheExcution.DescriptionAValider = "";
                    db.Crm_TacheExcution.Add(Crm_TacheExcution);
                    db.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }


                TempData["SuccessMesage"] = "Tâche N°" + @crm_TacheReclamation.NumeroTache + " est enregistrée! ";
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
                return View(crm_TacheReclamation);
            }
            else
            {
                //DropdownList
                var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
                ViewData["ListClient"] = ListClients;

                var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
                ViewData["ListUtilisateur"] = ListUtilisateur;

                var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");

                ViewData["ListResponsable"] = ListResponsable;

                var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
                ViewData["ListEtat"] = ListEtat;

                var ListReclamation = new SelectList(db.Crm_ReclamationClient.ToList(), "Id_Reclamation", "Id_Reclamation");
                ViewData["ListReclamation"] = ListReclamation;

                //var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumeroRapport", "NumeroRapport");
                //ViewData["ListRapport"] = ListRapport;

                var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
                ViewData["ListNature"] = ListNature;

                crm_TacheReclamation.ListeRespensables = new List<Respensable>();
                crm_TacheReclamation.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
                crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
                crm_TacheReclamation.ListeRespensables = ResponsableData();
                crm_TacheReclamation.ListeCrm_ReclamationClient = Crm_ReclamationData();
                crm_TacheReclamation.ListeRapport = RapportData();

                return View(crm_TacheReclamation);

            }

        }

        public List<Crm_ReclamationClient> Crm_ReclamationData()
        {
            return db.Crm_ReclamationClient.ToList();
        }
        public List<Crm_Rapport> RapportData()
        {
            return db.Crm_Rapport.ToList();
        }
    }
}
