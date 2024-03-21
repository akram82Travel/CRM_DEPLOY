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
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
//using System.Net;
using System.Net.Mail;

using System.Threading;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity.Validation;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_TacheReclamationController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();
        // Global variable to store the start datetime
        private static DateTime startDatetime;

        // Function to fetch start datetime from API
        private async Task FetchStartDatetime()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://your-api-url.com/start-datetime");
                var data = DateTime.Now.ToString();// await response.Content.ReadAsStringAsync();
                startDatetime = DateTime.Now; // Assuming the API response is in DateTime format
            }
        }

        // Function to periodically update the start datetime
        private async Task UpdateStartDatetime()
        {
            await FetchStartDatetime(); // Fetch initial start datetime
            while (true)
            {
                await FetchStartDatetime(); // Update start datetime periodically
                await Task.Delay(1000); // Delay for 5 minutes (300,000 milliseconds)
            }
        }
        // Start the UpdateStartDatetime function when the application starts
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Task.Run(() => UpdateStartDatetime());
            base.OnActionExecuting(filterContext);
        }

        public ActionResult GetStartDatetime()
        {
            return Content(DateTime.Now.ToString(), "text/plain");
        }
        // GET: Crm_TacheReclamation
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Now.AddDays(-15).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d1"] = d1;
            ViewData["d2"] = d2;
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);


            string codeuser = Session["UserID"].ToString();
            if (Session["UserRole"].ToString() != "03")
            {
                return View(db.Crm_TacheReclamation.Where(s => s.DateCreation >= d11 && s.DateCreation <= d22).ToList());
            }

            else
            {
                codeuser = Session["UserID"].ToString();
                return View(db.Crm_TacheReclamation.Where(s => s.UtilisateurCreateur.Contains(codeuser)).Where(s => s.DateCreation >= d11 && s.DateCreation <= d22).ToList());
            }
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

            string codeuser = Session["UserID"].ToString();

            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation >= d1 && r.DateCreation <= d2 && r.UtilisateurCreateur == codeuser
                        select r;


            if (Session["UserRole"].ToString() != "03")
            {
                model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d1 && r.DateCreation < d2
                        select r;
            }


            return View("Index", model.ToList());
        }
        // GET: Crm_TacheReclamation/Details/5
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

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["UserNom"].ToString().Trim() + @Session["UserPrenom"].ToString().Trim());

            ViewData["ListResponsable"] = ListResponsable;


            var ListReclamation = new SelectList(db.Crm_ReclamationClient.ToList(), "Id_Reclamation", "Id_Reclamation");
            ViewData["ListReclamation"] = ListReclamation;

            var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumeroRapport", "NumeroRapport");
            ViewData["ListRapport"] = ListRapport;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle");
            ViewData["ListMode"] = ListMode;

            Crm_TacheReclamation crm_TacheReclamation = db.Crm_TacheReclamation.Find(id);
            if (crm_TacheReclamation == null)
            {
                return HttpNotFound();
            }


            /**/
            /** FIND ATTACHED TACHE **/
            /**/
            var attachedTachesTacheliee = (from a in db.Crm_TacheReclamation
                                 join b in db.Crm_TacheCommercialSoft on a.NumeroTache equals b.NumeroTacheSoft_Tecnique into bb
                                 from prod1 in bb
                                 join c in db.Crm_TacheLiee on a.NumeroTache equals c.NumeroTache into cc
                                 from prod2 in cc
                                 join d in db.Respensable on prod2.CodeRespensable equals d.CodeRespensable
                                 /*join e in db.Crm_TacheLiee on a.NumeroTache equals e.NumeroTache into ee
                                 from prod3 in ee*/
                                 where prod2.NumeroTache == id
                                                      select prod2).ToList();
            var attachedTachesCResponsable = (from a in db.Crm_TacheReclamation
                                 join b in db.Crm_TacheCommercialSoft on a.NumeroTache equals b.NumeroTacheSoft_Tecnique into bb
                                 from prod1 in bb
                                 join c in db.Crm_TacheLiee on a.NumeroTache equals c.NumeroTache into cc
                                 from prod2 in cc
                                 join d in db.Respensable on prod2.CodeRespensable equals d.CodeRespensable
                                 /*join e in db.Crm_TacheLiee on a.NumeroTache equals e.NumeroTache into ee
                                 from prod3 in ee*/
                                 where prod1.NumeroTacheCommercial == id
                                              select d).ToList();
            var attachedTachesToComm = (from a in db.Crm_TacheReclamation
                                        join b in db.Crm_TacheCommercialSoft on a.NumeroTache equals b.NumeroTacheSoft_Tecnique into bb
                                        from prod1 in bb
                                        join c in db.Crm_TacheLiee on a.NumeroTache equals c.NumeroTache into cc
                                        from prod2 in cc
                                        where prod1.NumeroTacheCommercial == id
                                        select new 
                                        {
                                            NumeroTache = a.NumeroTache,
                                            CodeRespensable = prod2.CodeRespensable,
                                            TacheTitre = a.TacheTitre,
                                            Duree = a.Duree
                                        }).ToList(); 
            var CoutResponsable = (from a in db.PersonnelGRH
                                              select a).ToList();
            var DureeTacheResponsable = (from a in db.Crm_CycleExecTache
                                              select a).ToList();
            
            List<Crm_TacheLiee> listTacheLiee = new List<Crm_TacheLiee>();
            foreach (var item in attachedTachesTacheliee)
            {
                listTacheLiee.Add(db.Crm_TacheLiee.Find(item.NumeroTache, item.CodeRespensable));
            } 
            
            List<Respensable> listTacheRespComm = new List<Respensable>();
            foreach (var items in attachedTachesCResponsable)
            {
                listTacheRespComm.Add(db.Respensable.Find(items.CodeRespensable));
            }  
            
            List<Crm_TacheReclamation> listTacheLieeToComm = new List<Crm_TacheReclamation>();
            List<Crm_TacheLiee> listTacheLieeToTache = new List<Crm_TacheLiee>();
            List<Crm_CycleExecTache> listTacheLieeToTacheDureeReel = new List<Crm_CycleExecTache>();
            
            foreach (var items in attachedTachesToComm)
            {
                listTacheLieeToComm.Add(db.Crm_TacheReclamation.Find(items.NumeroTache));
                listTacheLieeToTache.Add(db.Crm_TacheLiee.Find(items.NumeroTache, items.CodeRespensable));
                Crm_CycleExecTache CycleExecTache = db.Crm_CycleExecTache.Where(e => e.NumeroTache == items.NumeroTache).FirstOrDefault();
                if(CycleExecTache != null)
                {
                    listTacheLieeToTacheDureeReel.Add(db.Crm_CycleExecTache.Find(CycleExecTache.idTacheExec));
                }
                else
                {
                    listTacheLieeToTacheDureeReel.Add(new Crm_CycleExecTache());
                }
                
            } 
            
            List<PersonnelGRH> listCoutResp = new List<PersonnelGRH>();
            foreach (var items in CoutResponsable)
            {
                listCoutResp.Add(db.PersonnelGRH.Find(items.MatriculePersonnel));
            }
            ViewData["attachedTaches"] = listTacheLiee;
            ViewData["attachedTachesRsp"] = listTacheRespComm;
            ViewData["listTacheLieeToComm"] = listTacheLieeToComm;
            ViewData["listTacheLieeToTache"] = listTacheLieeToTache;
            ViewData["listTacheLieeToTacheDureeReel"] = listTacheLieeToTacheDureeReel;
            ViewData["CoutResponsable"] = CoutResponsable;
            /*Listes */
            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
            crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();
            crm_TacheReclamation.ListeCrm_ReclamationClient = Crm_ReclamationData();
            crm_TacheReclamation.ListeRapport = RapportData();

            var model = (from c in db.Crm_TacheLiee
                         where c.NumeroTache == id
                         select c.CodeRespensable).ToList();

            crm_TacheReclamation.SelectedresponsableId = model;

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
            ViewData["ListEtat"] = ListEtat;

            var d1 = DateTime.Parse(crm_TacheReclamation.DatePrevus.ToString());
            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            ViewData["d2"] = DateTime.Parse(crm_TacheReclamation.DateCreation.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            // get list etat execution by tache
            var listCrm_TacheExcution = db.Crm_TacheExcution.Where(c => c.IdTache == id).ToList();



            ViewData["listCrm_TacheExcution"] = listCrm_TacheExcution;
            var ListType = new SelectList(db.Crm_TypeTache.Where(e => e.mode == "creation").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
            ViewData["ListType"] = ListType;
            
            var ListTypeEvalutation = new SelectList(db.Crm_TypeTache.Where(e => e.mode == "sanction").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
            ViewData["ListTypeEvalutation"] = ListTypeEvalutation;
            
            
            var ListSanction = db.Crm_Sanction.ToList();
            ViewData["ListSanction"] = ListSanction; 
            var ListDegresSanction = db.Crm_Degres_Sanction.ToList();
            ViewData["ListDegresSanction"] = ListDegresSanction;

            return View(crm_TacheReclamation);
        }
        // GET: Crm_TacheReclamation/Create
        public ActionResult Create()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            Crm_TacheReclamation crm_TacheReclamation = new Crm_TacheReclamation();
            // var userName = Session["UserPrenom"].ToString();
            string codeuser = Session["UserID"].ToString();
            string codeResponsable = Session["codeResponsable"].ToString();

            Crm_EquipeRespensable CodeEquipe = db.Crm_EquipeRespensable.FirstOrDefault(e => e.CodeRespensable == codeResponsable);
            Crm_Equipe ChefEquipe = db.Crm_Equipe.FirstOrDefault(e => e.CodeEquipe == CodeEquipe.CodeEquipe);
            var ListeEquipe = db.Crm_EquipeRespensable.Where(e => e.CodeEquipe == CodeEquipe.CodeEquipe).Select(e => e.CodeRespensable).ToList();
            Crm_AccesUtilisateur grade = db.Crm_AccesUtilisateur.FirstOrDefault(e => e.NomUtilisateur == codeuser);//grade.CodeAccess
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListClientsP = db.Client.ToList();
            ViewData["ListClientP"] = ListClientsP;

            var ListClientParents = new SelectList(db.Crm_ClientParent.ToList(), "CodeClientParent", "Libelle");
            ViewData["ListClientParents"] = ListClientParents;
            
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;

            //var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur", Session["UserPrenom"].ToString().Trim());
            //ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.Where(s => s.CodeRespensable == ChefEquipe.ChefEquipe).ToList(), "CodeRespensable", "Nom", ChefEquipe.ChefEquipe.ToString());
            if (grade.CodeAcces == "03")//developpeur
            {
                 ListResponsable = new SelectList(db.Respensable.Where(s => ListeEquipe.Contains(s.CodeRespensable)).ToList(), "CodeRespensable", "Nom", ChefEquipe.ChefEquipe.ToString());
            }

            ViewData["ListResponsable"] = ListResponsable;



            var ListResponsablePlanificateur = new SelectList(db.Respensable.Where(s => ListeEquipe.Contains(s.CodeRespensable)).ToList(), "CodeRespensable", "Nom", ChefEquipe.ChefEquipe.ToString());

            if (codeuser == "BWAJDI")
            {
                ListResponsablePlanificateur = new SelectList(db.Respensable.Where(s => ListeEquipe.Contains(s.CodeRespensable)).ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            }

            ViewData["ListResponsablePlanificateur"] = ListResponsablePlanificateur;


            var ListReclamation = new SelectList(db.Crm_ReclamationClient.ToList(), "Id_Reclamation", "Id_Reclamation");

            ViewData["ListReclamation"] = ListReclamation;

            var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumeroRapport", "NumeroRapport");
            ViewData["ListRapport"] = ListRapport;

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle");
            ViewData["ListMode"] = ListMode;

            var ListType = new SelectList(db.Crm_TypeTache.Where(e=>e.mode=="creation").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
            ViewData["ListType"] = ListType;

            var ListUtilisateurContact = new SelectList(db.Respensable.Where(s => s.CodeRespensable == codeResponsable || ListeEquipe.Contains(s.CodeRespensable)).ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new List<ContactClient>();
            ListeContactClient = ContactClientData();
            ViewData["ListeContactClient"] = ListeContactClient;


            var Listclient = new List<Client>();
            Listclient = ClientData();
            ViewData["ListeClient"]= Listclient;


            crm_ReclamationClient.ListeUtilisateur = new List<Utilisateur>();
            crm_ReclamationClient.ListeUtilisateur = UtilisateurData();
            ViewData["ListeUtilisateur"] = crm_ReclamationClient.ListeUtilisateur;

            IEnumerable<Crm_TacheReclamation> ListTacheCommercial = db.Crm_TacheReclamation.Where(s => s.Nature == "C").ToList();
            ViewData["ListTacheCommercial"] = ListTacheCommercial;            //crm_ReclamationClient.ListeRespensables = new List<Respensable>();
            //crm_ReclamationClient.ListeRespensables = ResponsableData();
            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("TD", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

            crm_TacheReclamation.NumeroTache = "TD" + "210" + valeur.ToString("00000");


            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
            crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();
            crm_TacheReclamation.ListeCrm_ReclamationClient = Crm_ReclamationData();
            crm_TacheReclamation.ListeRapport = RapportData();

            //Initialize Required variables
            crm_TacheReclamation.NumeroOrdre = 0;
            crm_TacheReclamation.Duree = DateTime.Now.ToString("t");
            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            crm_TacheReclamation.DatePrevus = DateTime.Now;
            crm_TacheReclamation.Degres = 0;
            crm_TacheReclamation.Type = "A";
            crm_TacheReclamation.TypePiece = "1";


            return View(crm_TacheReclamation);
        }

        // Liste UtilisateurCreateur

        // POST: Crm_TacheReclamation/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "NumeroTache,TacheTitre,DescriptionTache,Nature,Type,Degres,NomPlanificateur,NomValidateur,NomTesteur,CodeRespensable,Duree,Status,EtatValidation,DateCreation,UtilisateurCreateur,Paye,ValiderPar,CodeClient,ConfirmerPar,NonConfirmerPar,DateNonConfirmation,DatePrevus,DureeReel,NumDossier,NumReclamation,TypePiece,NumPiece")] Crm_TacheReclamation crm_TacheReclamation, IEnumerable<string> SelectedresponsableId, string CheckText, string CommercialTache, string DureeSprint)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "Crm_TacheReclamation/Create" });
            }
            if (ModelState.IsValid)
            {

                try
                {
                    if (CheckText == "true")
                    {
                        crm_TacheReclamation.NomValidateur = "";
                        crm_TacheReclamation.NomTesteur = "";
                        SelectedresponsableId = Enumerable.Empty<string>();
                    }

                    
                    if (SelectedresponsableId.Any())
                    {
                        crm_TacheReclamation.SelectedresponsableId = SelectedresponsableId;
                        crm_TacheReclamation.Status = "E11";
                    }
                    else
                    {
                        crm_TacheReclamation.SelectedresponsableId = Enumerable.Empty<string>();
                        crm_TacheReclamation.Status = "E10";
                    }


                    if(DureeSprint != "")
                    {
                        crm_TacheReclamation.Duree = DureeSprint;
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
                    crm_TacheReclamation.CodeRespensable = "";
                    crm_TacheReclamation.DateConfirmation = "";
                    crm_TacheReclamation.DateDebutExecution = "";
                    crm_TacheReclamation.DurerExcutionValider = "";
                    crm_TacheReclamation.DateFinExecution = "";
                    //crm_TacheReclamation.NumPiece = "";

                    if (crm_TacheReclamation.NumPiece == null)
                    {
                        crm_TacheReclamation.NumPiece = -1;
                    }
                    if (SelectedresponsableId.Any())
                    {
                        crm_TacheReclamation.NumeroEtat = "E08";
                    }
                    else
                    {
                        crm_TacheReclamation.NumeroEtat = "E05";
                    }

                    //crm_TacheReclamation.TypePiece = "";

                    crm_TacheReclamation.NumeroOrdre = 0;
                    if (crm_TacheReclamation.NumDossier != null)
                    {
                        var model = (from c in db.Crm_TacheReclamation
                                     select c.NumeroOrdre).ToList();

                        crm_TacheReclamation.NumeroOrdre = model.Max() + 1;
                    }


                    if (crm_TacheReclamation.NumReclamation == null)
                    {
                        crm_TacheReclamation.NumReclamation = "";
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

                    crm_TacheReclamation.RaisonSociale = db.Client.Find(crm_TacheReclamation.CodeClient).RaisonSociale;
                    /*Add selected new tache to base*/
                    db.Crm_TacheReclamation.Add(crm_TacheReclamation);
                    db.SaveChanges();

                    /*Add list respensable to base*/
                    Crm_TacheLiee crm_TacheLiee = new Crm_TacheLiee();
                    if (SelectedresponsableId != null)
                    {
                        foreach (var item in SelectedresponsableId)
                        {
                            crm_TacheLiee = new Crm_TacheLiee();
                            crm_TacheLiee.CodeRespensable = item;
                            crm_TacheLiee.NumeroTache = crm_TacheReclamation.NumeroTache;
                            db.Crm_TacheLiee.Add(crm_TacheLiee);
                            db.SaveChanges();
                        }

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

                    /*ADD tacheCOMMERCIAL*/
                    Crm_TacheCommercialSoft crm_TacheCommercialSoft = new Crm_TacheCommercialSoft();
                    crm_TacheCommercialSoft.NumeroTacheSoft_Tecnique = crm_TacheReclamation.NumeroTache;
                    crm_TacheCommercialSoft.NumeroTacheCommercial = CommercialTache;
                    db.Crm_TacheCommercialSoft.Add(crm_TacheCommercialSoft);
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
                return RedirectToAction("Dashbord");
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


                crm_TacheReclamation.ListeRespensables = new List<Respensable>();
                crm_TacheReclamation.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
                crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
                crm_TacheReclamation.ListeRespensables = ResponsableData();
                crm_TacheReclamation.ListeCrm_ReclamationClient = Crm_ReclamationData();
                crm_TacheReclamation.ListeRapport = RapportData();

                return View(crm_TacheReclamation);

            }

        }
        // GET: Crm_TacheReclamation/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "Crm_TacheReclamation/Edit/"+ id });
            }
            

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListClientsP = db.Client.ToList();
            ViewData["ListClientP"] = ListClientsP;

           
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["UserNom"].ToString().Trim() + @Session["UserPrenom"].ToString().Trim());
            ViewData["ListResponsable"] = ListResponsable;


            var ListReclamation = new SelectList(db.Crm_ReclamationClient.ToList(), "Id_Reclamation", "Id_Reclamation");
            ViewData["ListReclamation"] = ListReclamation;

            var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumeroRapport", "NumeroRapport");
            ViewData["ListRapport"] = ListRapport;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle");
            ViewData["ListMode"] = ListMode;

            var ListType = new SelectList(db.Crm_TypeTache.Where(e => e.mode == "creation").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
            ViewData["ListType"] = ListType;
            var ListTypeEvalutation = new SelectList(db.Crm_TypeTache.Where(e => e.mode == "sanction").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
            ViewData["ListTypeEvalutation"] = ListTypeEvalutation;

            var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
            ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new List<ContactClient>();
            ListeContactClient = ContactClientData();
            ViewData["ListeContactClient"] = ListeContactClient;


            var Listclient = new List<Client>();
            Listclient = ClientData();
            ViewData["ListeClient"] = Listclient;

            IEnumerable<Crm_TacheReclamation> ListTacheCommercial = db.Crm_TacheReclamation.Where(s => s.Nature == "C").ToList();
            ViewData["ListTacheCommercial"] = ListTacheCommercial;


            Crm_TacheReclamation crm_TacheReclamation = db.Crm_TacheReclamation.Find(id);

            ViewData["d1"] = DateTime.Parse(crm_TacheReclamation.DatePrevus.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(crm_TacheReclamation.DateCreation.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");


            /*Listes */
            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
            crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();
            crm_TacheReclamation.ListeCrm_ReclamationClient = Crm_ReclamationData();
            crm_TacheReclamation.ListeRapport = RapportData();

            var model = (from c in db.Crm_TacheLiee
                         where c.NumeroTache == id
                         select c.CodeRespensable).ToList();


            crm_TacheReclamation.SelectedresponsableId = model;


            var ClientParent = (from c in db.Crm_ClientParentAssociation
                         where c.CodeClientFils == crm_TacheReclamation.CodeClient
                         select c.CodeClientParent).ToList();

            if (ClientParent.Count == 0)
            {
                ViewData["codeparent"] = "Sélectionner Client Parent";
            }
            else
            {
                foreach (var item in ClientParent)
                {
                    ViewData["codeparent"] = db.Crm_ClientParent.Find(item).Libelle;

                }
            }
            var ListClientParents = new SelectList(db.Crm_ClientParent.ToList(), "CodeClientParent", "Libelle");
            ViewData["ListClientParents"] = ListClientParents;

            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;

            /*Liste tachecommercial liee au tache soft*/
            var model1 = (from c in db.Crm_TacheCommercialSoft
                         where c.NumeroTacheSoft_Tecnique == id
                         select c.NumeroTacheCommercial).ToList();
            if (model1.Count == 0)
            {
                ViewData["Empty"] = "vide";
                ViewData["NtacheCommercial"] = "";
            }
            else
            {
                ViewData["Empty"] = "Nonvide";
            
                ViewData["NtacheCommercial"] = model1.ElementAt(0);
               
            }
            var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
            ViewData["ListEtat"] = ListEtat;

            
            if (crm_TacheReclamation == null)
            {
                return HttpNotFound();
            }
            return View(crm_TacheReclamation);
        }
        // POST: Crm_TacheReclamation/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        
        public ActionResult Edit([Bind(Include = "NumeroTache,TacheTitre,DescriptionTache,Nature,Type,Degres,NomPlanificateur,NomValidateur,NomTesteur,CodeRespensable,Duree,Status,EtatValidation,DateCreation,UtilisateurCreateur,Paye,ValiderPar,CodeClient,ConfirmerPar,NonConfirmerPar,DateNonConfirmation,DatePrevus,DureeReel,NumDossier,NumReclamation,TypePiece,NumPiece")] Crm_TacheReclamation crm_TacheReclamation, IEnumerable<string> SelectedresponsableId, string Status, string NumeroEtat, string UtilisateurCreateur, string CommercialTache)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            if (ModelState.IsValid)
            {
                
                try
                {
                    // Votre code d'opérations avec Entity Framework ici
                    if (SelectedresponsableId != null)
                    {
                        crm_TacheReclamation.SelectedresponsableId = SelectedresponsableId;
                    }
                    else
                    {
                        crm_TacheReclamation.SelectedresponsableId = Enumerable.Empty<string>();
                    }
                    
                    if(crm_TacheReclamation.EtatValidation == "")
                    {
                        crm_TacheReclamation.EtatValidation = "";
                    }
                    crm_TacheReclamation.Status = Status;
                    //crm_TacheReclamation.EtatValidation = "abcd";
                    crm_TacheReclamation.UtilisateurCreateur = UtilisateurCreateur;
                    crm_TacheReclamation.ValiderPar = "";
                    crm_TacheReclamation.ConfirmerPar = "";
                    crm_TacheReclamation.NonConfirmerPar = "";
                    crm_TacheReclamation.DateNonConfirmation = "";
                    crm_TacheReclamation.DureeReel = 0;
                    crm_TacheReclamation.AnuulerPar = "";
                    crm_TacheReclamation.DateAnnulation = "";
                    crm_TacheReclamation.CodeRespensable = "";
                    crm_TacheReclamation.DateConfirmation = "";
                    crm_TacheReclamation.DateDebutExecution = "";
                    crm_TacheReclamation.DurerExcutionValider = "";
                    crm_TacheReclamation.DateFinExecution = "";
                    //crm_TacheReclamation.CodeClient = "";
                    // crm_TacheReclamation.NumPiece = "";
                    crm_TacheReclamation.NumeroEtat = NumeroEtat;
                    //crm_TacheReclamation.TypePiece = "";
                    if (crm_TacheReclamation.NumPiece == 0)
                    {
                        crm_TacheReclamation.NumPiece = -1;
                    }
                    if (crm_TacheReclamation.NumReclamation == null)
                    {
                        crm_TacheReclamation.NumReclamation = "";
                    }

                    if (crm_TacheReclamation.NumDossier == null)
                    {
                        crm_TacheReclamation.NumDossier = "";
                    }
                    /*Initialize required variable to avoid NULL exception */
                    if (crm_TacheReclamation.DescriptionTache == null)
                    { crm_TacheReclamation.DescriptionTache = ""; }


                    /*Update selected responsable to base*/
                    Crm_TacheLiee crm_TacheLiee = new Crm_TacheLiee();

                    var model = (from c in db.Crm_TacheLiee
                                 where c.NumeroTache == crm_TacheReclamation.NumeroTache
                                 select c.CodeRespensable).ToList();

                    foreach (var item in model)
                    {
                        crm_TacheLiee = db.Crm_TacheLiee.Find(crm_TacheReclamation.NumeroTache, item);
                        if (crm_TacheLiee.NumeroTache == crm_TacheReclamation.NumeroTache && crm_TacheLiee.CodeRespensable == item)
                            db.Crm_TacheLiee.Remove(crm_TacheLiee);
                        db.SaveChanges();
                    }
                    if (SelectedresponsableId != null)
                    {
                        foreach (var item in SelectedresponsableId)
                        {
                            crm_TacheLiee = new Crm_TacheLiee(); ;
                            crm_TacheLiee.CodeRespensable = item;
                            crm_TacheLiee.NumeroTache = crm_TacheReclamation.NumeroTache;
                            db.Crm_TacheLiee.Add(crm_TacheLiee);
                            db.SaveChanges();
                        }

                    }

                    /*Update selected tacheCommercial to base*/
                    Crm_TacheCommercialSoft crm_TacheCommercialSoft = new Crm_TacheCommercialSoft();

                    var model1 = (from c in db.Crm_TacheCommercialSoft
                                  where c.NumeroTacheSoft_Tecnique == crm_TacheReclamation.NumeroTache
                                  select c.NumeroTacheCommercial).ToList();

                    foreach (var item in model1)
                    {
                        crm_TacheCommercialSoft = db.Crm_TacheCommercialSoft.Find(item, crm_TacheReclamation.NumeroTache);
                        if (crm_TacheCommercialSoft.NumeroTacheSoft_Tecnique == crm_TacheReclamation.NumeroTache && crm_TacheCommercialSoft.NumeroTacheCommercial == item)
                            db.Crm_TacheCommercialSoft.Remove(crm_TacheCommercialSoft);
                        db.SaveChanges();
                    }


                    crm_TacheCommercialSoft.NumeroTacheSoft_Tecnique = crm_TacheReclamation.NumeroTache;
                    crm_TacheCommercialSoft.NumeroTacheCommercial = CommercialTache;
                    db.Crm_TacheCommercialSoft.Add(crm_TacheCommercialSoft);

                    crm_TacheReclamation.RaisonSociale = db.Client.Find(crm_TacheReclamation.CodeClient).RaisonSociale;

                    db.Entry(crm_TacheReclamation).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMesage"] = "Tâche N°" + @crm_TacheReclamation.NumeroTache + " est modifiée! ";
                    
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }
                }

                return RedirectToAction("Index");

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

                var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumeroRapport", "NumeroRapport");
                ViewData["ListRapport"] = ListRapport;

                var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
                ViewData["ListNature"] = ListNature;
                var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
                ViewData["ListProjet"] = ListProjet;
                var ListClientsP = db.Client.ToList();
                ViewData["ListClientP"] = ListClientsP;
                var ListType = new SelectList(db.Crm_TypeTache.Where(e => e.mode == "creation").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
                ViewData["ListType"] = ListType;
                var ListTypeEvalutation = new SelectList(db.Crm_TypeTache.Where(e => e.mode == "sanction").OrderByDescending(item => item.Libelle).ToList(), "CodeTypeTache", "Libelle");
                ViewData["ListTypeEvalutation"] = ListTypeEvalutation;
                var ListUtilisateurContact = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["codeResponsable"]);
                ViewData["ListUtilisateurContact"] = ListUtilisateurContact;

                Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
                var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
                ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

                var ListeContactClient = new List<ContactClient>();
                ListeContactClient = ContactClientData();
                ViewData["ListeContactClient"] = ListeContactClient;

                var ListMode = new SelectList(db.crm_ModeTache.ToList(), "CodeModeTache", "Libelle");
                ViewData["ListMode"] = ListMode;
                /*Liste tachecommercial liee au tache soft*/
                var model1 = (from c in db.Crm_TacheCommercialSoft
                              where c.NumeroTacheSoft_Tecnique == crm_TacheReclamation.NumeroTache
                              select c.NumeroTacheCommercial).ToList();
                if (model1.Count == 0)
                {
                    ViewData["Empty"] = "vide";
                    ViewData["NtacheCommercial"] = "";
                }
                else
                {
                    ViewData["Empty"] = "Nonvide";

                    ViewData["NtacheCommercial"] = model1.ElementAt(0);

                }

                IEnumerable<Crm_TacheReclamation> ListTacheCommercial = db.Crm_TacheReclamation.Where(s => s.Nature == "C").ToList();
                ViewData["ListTacheCommercial"] = ListTacheCommercial;
                var Listclient = new List<Client>();
                Listclient = ClientData();
                ViewData["ListeClient"] = Listclient;

                /*Listes*/
                crm_TacheReclamation.ListeRespensables = new List<Respensable>();
                crm_TacheReclamation.ListeCrm_ReclamationClient = new List<Crm_ReclamationClient>();
                crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
                crm_TacheReclamation.ListeRespensables = ResponsableData();
                crm_TacheReclamation.ListeCrm_ReclamationClient = Crm_ReclamationData();
                crm_TacheReclamation.ListeRapport = RapportData();

                return View(crm_TacheReclamation);
            }
        }
        // GET: Crm_TacheReclamation/Delete/5
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
            Crm_TacheReclamation crm_TacheReclamation = db.Crm_TacheReclamation.Find(id);

            if (crm_TacheReclamation == null)
            {
                return HttpNotFound();
            }
            return View(crm_TacheReclamation);
        }
        // POST: Crm_TacheReclamation/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            Crm_TacheReclamation crm_TacheReclamation = db.Crm_TacheReclamation.Find(id);

            Crm_TacheLiee crm_TacheLiee = new Crm_TacheLiee();
            Crm_TacheExcution crm_TacheExcution = new Crm_TacheExcution();

            var model = (from c in db.Crm_TacheLiee
                         where c.NumeroTache == crm_TacheReclamation.NumeroTache
                         select c.CodeRespensable).ToList();
            var modelListRspName = (from e in db.Crm_TacheExcution
                                    where e.IdTache == crm_TacheReclamation.NumeroTache
                         select e.IdtacheExce).ToList();

            foreach (var item in model)
            {
                crm_TacheLiee = db.Crm_TacheLiee.Find(crm_TacheReclamation.NumeroTache, item);
                if (crm_TacheLiee.NumeroTache == crm_TacheReclamation.NumeroTache && crm_TacheLiee.CodeRespensable == item)
                    db.Crm_TacheLiee.Remove(crm_TacheLiee);
                db.SaveChanges();
            }
            foreach (var itemList in modelListRspName)
            {
                crm_TacheExcution = db.Crm_TacheExcution.Find(itemList);
                if (crm_TacheExcution.IdtacheExce == itemList)
                    db.Crm_TacheExcution.Remove(crm_TacheExcution);
                db.SaveChanges();
            }

            Crm_TacheCommercialSoft crm_TacheCommercialSoft = new Crm_TacheCommercialSoft();

            var model1 = (from c in db.Crm_TacheCommercialSoft
                          where c.NumeroTacheSoft_Tecnique == crm_TacheReclamation.NumeroTache
                         select c.NumeroTacheCommercial).ToList();

            foreach (var item in model1)
            {
                crm_TacheCommercialSoft = db.Crm_TacheCommercialSoft.Find(item, crm_TacheReclamation.NumeroTache);
                if (crm_TacheCommercialSoft.NumeroTacheSoft_Tecnique == crm_TacheReclamation.NumeroTache && crm_TacheCommercialSoft.NumeroTacheCommercial == item)
                    db.Crm_TacheCommercialSoft.Remove(crm_TacheCommercialSoft);
                db.SaveChanges();
            }
            db.Crm_TacheReclamation.Remove(crm_TacheReclamation);
            db.SaveChanges();
            //TempData["SuppressionMessage"] = "Tache N°" + @crm_TacheReclamation.NumeroTache + " est supprimée! ";
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
        public ActionResult Dashbord()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            string codeuser = Session["UserID"].ToString();
            string codeResponsable = Session["codeResponsable"].ToString();

            Crm_EquipeRespensable CodeEquipe = db.Crm_EquipeRespensable.FirstOrDefault(e => e.CodeRespensable == codeResponsable);
            Crm_Equipe ChefEquipe = db.Crm_Equipe.FirstOrDefault(e => e.CodeEquipe == CodeEquipe.CodeEquipe);
            var ListeEquipe = db.Crm_EquipeRespensable.Where(e => e.CodeEquipe == CodeEquipe.CodeEquipe).Select(e => e.CodeRespensable).ToList();
            Crm_AccesUtilisateur grade = db.Crm_AccesUtilisateur.FirstOrDefault(e => e.NomUtilisateur == codeuser);//grade.CodeAccess


            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");

            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.Where(s => s.CodeRespensable == codeResponsable || ListeEquipe.Contains(s.CodeRespensable)).ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            Crm_TacheReclamation crm_TacheReclamation = new Crm_TacheReclamation();
            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();



            var d1 = DateTime.Now.AddDays(-15).ToString("yyyy'-'MM'-'dd");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd");

            ViewData["d1"] = d1;
            ViewData["d2"] = d2;
            return View(crm_TacheReclamation);


        }
        // serche date Dashbord
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchDateDashbord()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);
            var CodeClient = Request["CodeClient"];
            var NomPlanificateur = Request["NomPlanificateur"];
            var CodeRespensable = Request["CodeRespensable"];
            var NumPiece = Request["NumPiece"];
            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["CodeClient"] = CodeClient;
            ViewData["NomPlanificateur"] = NomPlanificateur;
            ViewData["CodeRespensable"] = CodeRespensable;
            ViewData["NumPiece"] = NumPiece;
            
            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d1 && r.DateCreation < d2 && r.CodeClient == CodeClient && r.NomPlanificateur == NomPlanificateur && r.CodeRespensable == CodeRespensable && r.NumPiece == int.Parse(NumPiece)
                        select r;
            return View("Dashbord", model.ToList());
        }
        // serche data from html to controlleur 
        public ActionResult DateRange()
        {
            List<DateTime> DateList = new List<DateTime>();
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d1 && r.DateCreation < d2
                        select r;
            return View("Index", model.ToList());
        }
        //Filtrage
        public ActionResult Search(string Status, string d1, string d2)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d11 = DateTime.Parse(d1).AddMonths(-1);
            var d22 = DateTime.Parse(d2);

            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");



            string codeuser = Session["UserID"].ToString();
            if (Session["UserRole"].ToString() != "03")
            {

                return View("Index", db.Crm_TacheReclamation.Where(s => s.DateCreation > d11 && s.DateCreation < d22 && s.DateCreation < d22 && s.Status == Status).ToList());
            }

            else
            {
                return View("Index", db.Crm_TacheReclamation.Where(s => s.UtilisateurCreateur.Contains(codeuser)).Where(s => s.DateCreation > d11 && s.DateCreation < d22 && s.Status == Status).ToList());
            }

        }
        public ActionResult SearchDate(string d1, string d2)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d11 = DateTime.Parse(d1).AddMonths(-1);
            var d22 = DateTime.Parse(d2);

            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");



            string codeuser = Session["UserID"].ToString();
            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d11 && r.DateCreation < d22 && r.UtilisateurCreateur.Contains(codeuser)
                        select r;

            if (Session["UserRole"].ToString() != "03")
            {


                model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d11 && r.DateCreation < d22
                        select r;
            }

            return View("Index", model);
        }
        [HttpPost]
        public ActionResult listeTache(Crm_TacheReclamation reclamation)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var json = JsonConvert.SerializeObject(reclamation);
            
            //initialiser date de liste de taches 1 mois
            var d1 = DateTime.Parse(DateTime.Now.AddDays(-15).ToString("yyyy'-'MM'-'dd"));
            var d2 = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd"));

           
            //return Json(d11);
            string codeuser = Session["UserID"].ToString();
            string codeResponsable = Session["codeResponsable"].ToString();

            Crm_EquipeRespensable CodeEquipe  = db.Crm_EquipeRespensable.FirstOrDefault(e => e.CodeRespensable == codeResponsable);
            Crm_Equipe ChefEquipe  = db.Crm_Equipe.FirstOrDefault(e => e.CodeEquipe ==  CodeEquipe.CodeEquipe);
            var ListeEquipe  = db.Crm_EquipeRespensable.Where(e => e.CodeEquipe ==  CodeEquipe.CodeEquipe).Select(e => e.CodeRespensable).ToList();
            Crm_AccesUtilisateur grade = db.Crm_AccesUtilisateur.FirstOrDefault(e=> e.NomUtilisateur == codeuser);//grade.CodeAccess


            var result = from a in db.Crm_TacheReclamation

                          //|| a.NomValidateur == codeResponsable || a.NomTesteur == codeResponsable || a.UtilisateurCreateur == codeuser
                         join b in db.Crm_TacheLiee on a.NumeroTache equals b.NumeroTache into tl
                         from b in tl.DefaultIfEmpty()
                         join e in db.Crm_CycleExecTache on a.NumeroTache equals e.NumeroTache into ct
                         from e in ct.DefaultIfEmpty()
                         join c in db.Respensable on b.CodeRespensable equals c.CodeRespensable into rsp
                         from c in rsp.DefaultIfEmpty()
                         join d in db.Crm_ReclamationClient on a.NumReclamation equals d.Id_Reclamation into Recla
                         from d in Recla.DefaultIfEmpty()
                       
                         orderby b.NumeroTache descending
                         select new
                         {
                             a.AExpliquer,
                             a.Duree,
                             DureeReel = e == null ? 0 : e.Duree,
                             a.NumeroTache,
                             a.UtilisateurCreateur,
                             a.CodeClient,
                             CodeRespensable = b == null ? "" : b.CodeRespensable.ToString(),
                             CodeResponsable = b == null ? "" : b.CodeRespensable.ToString(),
                             Nom = c == null ? "" : c.Nom.ToString(),
                             a.DescriptionTache,
                             a.TacheTitre,
                             a.NomPlanificateur,
                             a.NomTesteur,
                             a.NomValidateur,
                             a.NumDossier,
                             a.NumeroEtat,
                             a.NumeroOrdre,
                             a.RaisonSociale,
                             a.Status,
                             a.DateCreation,
                             a.Nature,
                             a.NumPiece,
                             a.NumReclamation,
                             StatusReclamation = (d.Status == "E05") ? "<span class='badge badge-warning'>Nouveau</span>" : (d.Status == "E07") ? "<span class='badge badge-success'>En Cours</span>" : "<span class='badge badge-striped'>Cloturée</span>",
                             a.Degres,
                         };


            // si chef de projet : afficher les tache planifier par le chef de projet de lui et par les membres equipe eux même
            if (grade.CodeAcces == "02")
            {
                result = result.Where(s => ListeEquipe.Contains(s.CodeRespensable) || s.CodeRespensable == codeResponsable || s.NomPlanificateur == ChefEquipe.ChefEquipe);
            }


            // si memebre equipe : affiche les tache planifier par eux et par leur chef equipe
            if (grade.CodeAcces == "03")
            {
                result = result.Where(s => s.CodeRespensable == codeResponsable || s.NomTesteur == codeResponsable || ListeEquipe.Contains(s.NomPlanificateur));
            }
            if (reclamation.SelectedresponsableId != null && reclamation.SelectedresponsableId.Any())

            {
                IEnumerable<string> selectedResponsableIds = reclamation.SelectedresponsableId;
                if (reclamation.SelectedresponsableId.FirstOrDefault().ToString() != "{}")
                {
                    result = result.Where(s => selectedResponsableIds.Contains(s.CodeRespensable));
                }
            }


            //filter result

            if (reclamation.CodeClient != null)
            {
                result = result.Where(s => s.CodeClient.Contains(reclamation.CodeClient.ToString()));
            }
            
            if (reclamation.NumPiece > 0)
            {
                result = result.Where(s => s.NumPiece == reclamation.NumPiece);
            }


            if (reclamation.NumeroTache != null)
            {
                result = result.Where(s => s.NumeroTache.Contains(reclamation.NumeroTache.ToString()));
            }
            if (reclamation.CodeRespensable != null)
            {
                result = result.Where(s => s.CodeRespensable.Contains(reclamation.CodeRespensable.ToString()));
            }
            if (reclamation.DateCreation.ToString() != "")
            {
                d1 = DateTime.Parse(reclamation.DateCreation.ToString());
                d2 = DateTime.Parse(reclamation.DateCreationFin.ToString()).AddDays(1);
            }
            if (reclamation.DescriptionTache != null)
            {
                result = result.Where(s => s.DescriptionTache.Contains(reclamation.DescriptionTache.ToString()) || s.TacheTitre.Contains(reclamation.DescriptionTache.ToString()));
            }
            if (reclamation.NomPlanificateur != null)
            {
                result = result.Where(s => s.NomPlanificateur.Contains(reclamation.NomPlanificateur.ToString()));
            }
            if (reclamation.NomTesteur != null)
            {
                result = result.Where(s => s.NomTesteur.Contains(reclamation.NomTesteur.ToString()));
            }
            if (reclamation.NomValidateur != null)
            {
                result = result.Where(s => s.NomValidateur.Contains(reclamation.NomValidateur.ToString()));
            }
            // get list all tache by user connected
            if (Session["UserRole"].ToString() != "03")
            {
                result = result.Where(s => s.DateCreation >= d1 && s.DateCreation <= d2);
            }
            else
            {
                result = result.Where(s => s.CodeRespensable.Contains(codeResponsable) || s.NomTesteur == codeResponsable ).Where(s => s.DateCreation >= d1 && s.DateCreation <= d2);
            }




            // group and set durée total for each line tache
            var groupedResults = result.GroupBy(entry => entry.NumeroTache)
                           .Select(group => new
                           {
                               GroupKey = group.Key,
                               Entries = group.ToList(),
                               TotalDureeReel = group.Sum(entry => entry.DureeReel),
                           }).ToList();

            List<dynamic> finalResults = new List<dynamic>();
            foreach (var group in groupedResults)
            {
                foreach (var entry in group.Entries)
                {
                    var finalEntry = new 
                    {
                        AExpliquer = entry.AExpliquer,
                        Duree = entry.Duree,
                        // Other properties...
                        entry.NumeroTache,
                        entry.UtilisateurCreateur,
                        entry.CodeClient,
                        entry.CodeRespensable,
                        entry.CodeResponsable,
                        entry.Nom,
                        entry.DescriptionTache,
                        entry.TacheTitre,
                        entry.NomPlanificateur,
                        entry.NomTesteur,
                        entry.NomValidateur,
                        entry.NumDossier,
                        entry.NumeroEtat,
                        entry.NumeroOrdre,
                        entry.RaisonSociale,
                        entry.Status,
                        entry.DateCreation,
                        entry.Nature,
                        entry.NumPiece,
                        entry.NumReclamation,
                        entry.StatusReclamation,
                        entry.Degres,
                        DureeReel = group.TotalDureeReel.ToString(),
                    };
                    finalResults.Add(finalEntry);
                }
            }

            List<dynamic> convertedFinalResults = finalResults.OrderByDescending(a=>a.NumeroTache).ToList();

            json = JsonConvert.SerializeObject(convertedFinalResults);

            
            return Json(json);
        }
        public List<Respensable> ResponsableData()
        {
            string codeuser = Session["UserID"].ToString();
            string codeResponsable = Session["codeResponsable"].ToString();

            Crm_EquipeRespensable CodeEquipe = db.Crm_EquipeRespensable.FirstOrDefault(e => e.CodeRespensable == codeResponsable);
            Crm_Equipe ChefEquipe = db.Crm_Equipe.FirstOrDefault(e => e.CodeEquipe == CodeEquipe.CodeEquipe);
            var ListeEquipe = db.Crm_EquipeRespensable.Where(e => e.CodeEquipe == CodeEquipe.CodeEquipe).Select(e => e.CodeRespensable).ToList();
            Crm_AccesUtilisateur grade = db.Crm_AccesUtilisateur.FirstOrDefault(e => e.NomUtilisateur == codeuser);//grade.CodeAccess

            return db.Respensable.Where(s => s.CodeRespensable == codeResponsable || ListeEquipe.Contains(s.CodeRespensable)).ToList();
        }
        public List<Crm_ReclamationClient> Crm_ReclamationData()
        {
            return db.Crm_ReclamationClient.ToList();
        }
        public List<Crm_Rapport> RapportData()
        {
            var d1 = DateTime.Now.AddMonths(-1).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d11 = DateTime.Parse(d1);

            return db.Crm_Rapport.Where(s => s.DateRapport >= d11).ToList();
        }
        public List<EtatCRM> EtatData()
        {
            return db.EtatCRM.ToList();
        }
        public ActionResult ImpressionTache()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;
            Crm_TacheReclamation crm_TacheReclamation = new Crm_TacheReclamation();


            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }


            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");

            ViewData["ListResponsable"] = ListResponsable;


            crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
            crm_TacheReclamation.ListeRapport = RapportData();

            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;


            return View("ImpressionTache", crm_TacheReclamation);

        }
        public ActionResult Kanban()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            return View();
        }
        [HttpPost]
        public void UpdateStateTache(Crm_TacheReclamation reclamation)
        {

            var json = JsonConvert.SerializeObject(reclamation);

            Crm_TacheReclamation tache = new Crm_TacheReclamation();
            Crm_CycleExecTache tacheCycleToUpdate = new Crm_CycleExecTache();
            //List<Crm_TacheLiee> tacheLierResponsable = new Crm_TacheLiee();
            Respensable tacheResponsable = new Respensable();

            tache = db.Crm_TacheReclamation.Find(reclamation.NumeroTache);
            // affecter tache pour les responsables
            List<Crm_TacheLiee> tacheLierResponsable = db.Crm_TacheLiee.Where(s => s.NumeroTache == reclamation.NumeroTache && s.CodeRespensable != "").ToList();
            foreach (var tacheLiee in tacheLierResponsable)
            {
                tacheResponsable = db.Respensable.Find(tacheLiee.CodeRespensable);
                //string status = reclamation.Status;
                string status = reclamation.Status.ToString();

                if (status == "1")// en attente
                {
                    tache.Status = "E11";
                }
                if (status == "2")// En Exécution
                {


                    // add line to execution table
                    /*Add tache execution to base*/
                    Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                    Crm_TacheExcution.IdTache = tache.NumeroTache;
                    Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                    // old infos etat/status
                    Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                    Crm_TacheExcution.OldStatus = tache.Status;
                    // new infos etat/status
                    Crm_TacheExcution.NewEtat = "E02";
                    Crm_TacheExcution.NewStatus = "E07";

                    Crm_TacheExcution.DateExcution = DateTime.Now;
                    Crm_TacheExcution.DateFinExcution = DateTime.Now;
                    Crm_TacheExcution.Duree = 0;
                    Crm_TacheExcution.IdtacheExce = 0;
                    Crm_TacheExcution.DescriptionAcorriger = "";
                    Crm_TacheExcution.DescriptionAExpliquer = "";
                    Crm_TacheExcution.DescriptionAValider = "";
                    db.Crm_TacheExcution.Add(Crm_TacheExcution);
                    db.SaveChanges();


                    // add line to cycle table
                    /*Add tache execution to base*/
                    Crm_CycleExecTache CrmCycleExecTache = new Crm_CycleExecTache();
                    CrmCycleExecTache.NumeroTache = tache.NumeroTache;
                    CrmCycleExecTache.Nomutilisateur = tacheResponsable.Nom;
                    // old infos etat/status
                    CrmCycleExecTache.OldStatus = tache.Status;
                    CrmCycleExecTache.NewStatus = "E07";

                    CrmCycleExecTache.DateDebutExecution = DateTime.Now;
                    CrmCycleExecTache.DateFinExecution = DateTime.Now;
                    CrmCycleExecTache.Duree = 0;
                    CrmCycleExecTache.idTacheExec = 0;
                    CrmCycleExecTache.Encours = true;

                    db.Crm_CycleExecTache.Add(CrmCycleExecTache);
                    db.SaveChanges();

                    // update tache status in kanban
                    tache.Status = "E07";
                    tache.NumeroEtat = "E02";

                }
                if (status == "3") //En Pause
                {


                    // add line to execution table
                    /*Add tache execution to base*/
                    Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                    Crm_TacheExcution.IdTache = tache.NumeroTache;
                    Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                    // old infos etat/status
                    Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                    Crm_TacheExcution.OldStatus = tache.Status;
                    // new infos etat/status
                    Crm_TacheExcution.NewEtat = "E02";
                    Crm_TacheExcution.NewStatus = "E06";

                    Crm_TacheExcution.DateExcution = DateTime.Now;
                    Crm_TacheExcution.DateFinExcution = DateTime.Now;
                    Crm_TacheExcution.Duree = 0;
                    Crm_TacheExcution.IdtacheExce = 0;
                    Crm_TacheExcution.DescriptionAcorriger = "";
                    Crm_TacheExcution.DescriptionAExpliquer = "";
                    Crm_TacheExcution.DescriptionAValider = "";
                    db.Crm_TacheExcution.Add(Crm_TacheExcution);
                    db.SaveChanges();


                    // update line to cycle table
                    tacheCycleToUpdate = db.Crm_CycleExecTache.First(s => s.NumeroTache == tache.NumeroTache && s.Encours);

                    // old infos etat/status
                    tacheCycleToUpdate.OldStatus = tache.Status;
                    tacheCycleToUpdate.NewStatus = "E06";

                    /******************/
                    /** calcul duree en minute **/
                    /*******************/
                    TimeSpan diff = TimeSpan.FromTicks(0);
                    diff = DateTime.Now - tacheCycleToUpdate.DateDebutExecution;

                    /***************/
                    /**** Fin calcul duree *******/
                    /***************/
                    tacheCycleToUpdate.DateFinExecution = DateTime.Now;
                    tacheCycleToUpdate.Duree = Convert.ToInt32(diff.TotalMinutes); ;
                    tacheCycleToUpdate.idTacheExec = tacheCycleToUpdate.idTacheExec;
                    tacheCycleToUpdate.Encours = false;

                    db.Entry(tacheCycleToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    tache.Status = "E06";
                    tache.NumeroEtat = "E02";
                }
                if (status == "4")//A Valider
                {
                    if (tache.Status == "E07")
                    {
                        // add line to execution table
                        /*Add tache execution to base*/
                        Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                        Crm_TacheExcution.IdTache = tache.NumeroTache;
                        Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                        // old infos etat/status
                        Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                        Crm_TacheExcution.OldStatus = tache.Status;
                        // new infos etat/status
                        Crm_TacheExcution.NewEtat = "E02";
                        Crm_TacheExcution.NewStatus = "E04";

                        Crm_TacheExcution.DateExcution = DateTime.Now;
                        Crm_TacheExcution.DateFinExcution = DateTime.Now;
                        Crm_TacheExcution.Duree = 0;
                        Crm_TacheExcution.IdtacheExce = 0;
                        Crm_TacheExcution.DescriptionAcorriger = "";
                        Crm_TacheExcution.DescriptionAExpliquer = "";
                        Crm_TacheExcution.DescriptionAValider = "";
                        db.Crm_TacheExcution.Add(Crm_TacheExcution);
                        db.SaveChanges();


                        // update line to cycle table
                        tacheCycleToUpdate = db.Crm_CycleExecTache.First(s => s.NumeroTache == tache.NumeroTache && s.Encours);

                        // old infos etat/status
                        tacheCycleToUpdate.OldStatus = tache.Status;
                        tacheCycleToUpdate.NewStatus = "E04";

                        /******************/
                        /** calcul duree en minute **/
                        /*******************/
                        TimeSpan diff = TimeSpan.FromTicks(0);
                        diff = DateTime.Now - tacheCycleToUpdate.DateDebutExecution;

                        /***************/
                        /**** Fin calcul duree *******/
                        /***************/
                        tacheCycleToUpdate.DateFinExecution = DateTime.Now;
                        tacheCycleToUpdate.Duree = Convert.ToInt32(diff.TotalMinutes); ;
                        tacheCycleToUpdate.idTacheExec = tacheCycleToUpdate.idTacheExec;
                        tacheCycleToUpdate.Encours = false;

                        db.Entry(tacheCycleToUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else if (tache.Status == "E06")
                    {
                        // add line to execution table
                        /*Add tache execution to base*/
                        Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                        Crm_TacheExcution.IdTache = tache.NumeroTache;
                        Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                        // old infos etat/status
                        Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                        Crm_TacheExcution.OldStatus = tache.Status;
                        // new infos etat/status
                        Crm_TacheExcution.NewEtat = "E02";
                        Crm_TacheExcution.NewStatus = "E04";

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
                    tache.Status = "E04";
                    tache.NumeroEtat = "E02";
                }
                if (status == "5")//A Tester
                {
                    if (tache.Status == "E07")
                    {
                        // add line to execution table
                        /*Add tache execution to base*/
                        Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                        Crm_TacheExcution.IdTache = tache.NumeroTache;
                        Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                        // old infos etat/status
                        Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                        Crm_TacheExcution.OldStatus = tache.Status;
                        // new infos etat/status
                        Crm_TacheExcution.NewEtat = "E02";
                        Crm_TacheExcution.NewStatus = "E03";

                        Crm_TacheExcution.DateExcution = DateTime.Now;
                        Crm_TacheExcution.DateFinExcution = DateTime.Now;
                        Crm_TacheExcution.Duree = 0;
                        Crm_TacheExcution.IdtacheExce = 0;
                        Crm_TacheExcution.DescriptionAcorriger = "";
                        Crm_TacheExcution.DescriptionAExpliquer = "";
                        Crm_TacheExcution.DescriptionAValider = "";
                        db.Crm_TacheExcution.Add(Crm_TacheExcution);
                        db.SaveChanges();


                        // update line to cycle table
                        tacheCycleToUpdate = db.Crm_CycleExecTache.First(s => s.NumeroTache == tache.NumeroTache && s.Encours);

                        // old infos etat/status
                        tacheCycleToUpdate.OldStatus = tache.Status;
                        tacheCycleToUpdate.NewStatus = "E03";

                        /******************/
                        /** calcul duree en minute **/
                        /*******************/
                        TimeSpan diff = TimeSpan.FromTicks(0);
                        diff = DateTime.Now - tacheCycleToUpdate.DateDebutExecution;

                        /***************/
                        /**** Fin calcul duree *******/
                        /***************/
                        tacheCycleToUpdate.DateFinExecution = DateTime.Now;
                        tacheCycleToUpdate.Duree = Convert.ToInt32(diff.TotalMinutes); ;
                        tacheCycleToUpdate.idTacheExec = tacheCycleToUpdate.idTacheExec;
                        tacheCycleToUpdate.Encours = false;

                        db.Entry(tacheCycleToUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        // add line to execution table
                        /*Add tache execution to base*/
                        Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                        Crm_TacheExcution.IdTache = tache.NumeroTache;
                        Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                        // old infos etat/status
                        Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                        Crm_TacheExcution.OldStatus = tache.Status;
                        // new infos etat/status
                        Crm_TacheExcution.NewEtat = "E02";
                        Crm_TacheExcution.NewStatus = "E03";

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




                    tache.Status = "E03";
                    tache.NumeroEtat = "E02";
                }
                if (status == "6")
                {
                    if (tache.Status == "E07")
                    {
                        // add line to execution table
                        /*Add tache execution to base*/
                        Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                        Crm_TacheExcution.IdTache = tache.NumeroTache;
                        Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                        // old infos etat/status
                        Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                        Crm_TacheExcution.OldStatus = tache.Status;
                        // new infos etat/status
                        Crm_TacheExcution.NewEtat = "E01";
                        Crm_TacheExcution.NewStatus = "E01";

                        Crm_TacheExcution.DateExcution = DateTime.Now;
                        Crm_TacheExcution.DateFinExcution = DateTime.Now;
                        Crm_TacheExcution.Duree = 0;
                        Crm_TacheExcution.IdtacheExce = 0;
                        Crm_TacheExcution.DescriptionAcorriger = "";
                        Crm_TacheExcution.DescriptionAExpliquer = "";
                        Crm_TacheExcution.DescriptionAValider = "";
                        db.Crm_TacheExcution.Add(Crm_TacheExcution);
                        db.SaveChanges();


                        // update line to cycle table
                        tacheCycleToUpdate = db.Crm_CycleExecTache.First(s => s.NumeroTache == tache.NumeroTache && s.Encours);

                        // old infos etat/status
                        tacheCycleToUpdate.OldStatus = tache.Status;
                        tacheCycleToUpdate.NewStatus = "E01";

                        /******************/
                        /** calcul duree en minute **/
                        /*******************/
                        TimeSpan diff = TimeSpan.FromTicks(0);
                        diff = DateTime.Now - tacheCycleToUpdate.DateDebutExecution;

                        /***************/
                        /**** Fin calcul duree *******/
                        /***************/
                        tacheCycleToUpdate.DateFinExecution = DateTime.Now;
                        tacheCycleToUpdate.Duree = Convert.ToInt32(diff.TotalMinutes); ;
                        tacheCycleToUpdate.idTacheExec = tacheCycleToUpdate.idTacheExec;
                        tacheCycleToUpdate.Encours = false;

                        db.Entry(tacheCycleToUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {

                        // add line to execution table
                        /*Add tache execution to base*/
                        Crm_TacheExcution Crm_TacheExcution = new Crm_TacheExcution();
                        Crm_TacheExcution.IdTache = tache.NumeroTache;
                        Crm_TacheExcution.Nomutilisateur = tacheResponsable.Nom;
                        // old infos etat/status
                        Crm_TacheExcution.OldEtat = tache.NumeroEtat;
                        Crm_TacheExcution.OldStatus = tache.Status;
                        // new infos etat/status
                        Crm_TacheExcution.NewEtat = "E01";
                        Crm_TacheExcution.NewStatus = "E01";

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


                    tache.Status = "E01";// terminer
                    tache.NumeroEtat = "E01";
                }
            }
            db.Entry(tache).State = EntityState.Modified;
            db.SaveChanges();
        }
        //Liste des Interventions
        public ActionResult ListeIntervention()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d1"] = d1;
            ViewData["d2"] = d2;

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            

            var model = from r in db.Crm_TacheReclamation
                        where r.Nature == "T"
                        select r;
            return View("ListeIntervention", model.ToList());
        }
        public ActionResult SearchListeIntervention(string NumeroEtat, string d1, string d2)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);
            
           
           /* var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
            ViewData["ListEtat"] = ListEtat;*/

            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["NumeroEtat"] = NumeroEtat;
            var model = from r in db.Crm_TacheReclamation
                        where r.NumeroEtat == NumeroEtat && r.DateCreation > d11 && r.DateCreation < d22 && r.Nature == "T"
                        select r;

            return View("ListeIntervention", model);
        }
        public ActionResult SearchTousInterventions(string d1, string d2)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);

            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "Status", "Status");
            ViewData["ListEtat"] = ListEtat;

            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d11 && r.DateCreation < d22 && r.Nature == "T"
                        select r;

            return View("ListeIntervention", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchIntervention()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Parse(Request["d1"]).AddDays(-1);
            var d2 = DateTime.Parse(Request["d2"]);
            
            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation >= d1 && r.DateCreation < d2 && r.Nature == "T"
                        select r;
            return View("ListeIntervention", model.ToList());
        }
        //Impression Rapport Liste D'interventions
        public ActionResult Impression(string d1, string d2, string NumeroEtat)
        {
            
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TacheServiceTechnique.rpt"));

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            var d11 = DateTime.Parse(d1).AddDays(1);
            var d22 = DateTime.Parse(d2);

            string @DateDebut = d11.ToString("dd/MM/yyyy");
            string @DateFin = d22.ToString("dd/MM/yyyy");
            


            rd.SetParameterValue("NomSociete", "GROUP MAS");
            rd.SetParameterValue("@DateDebut", @DateDebut);
            rd.SetParameterValue("@DateFin", @DateFin);


            string ConditionFiltre =  "  {EtatCRM.NumeroEtat} = '" + NumeroEtat + "'  and ";

            ConditionFiltre = ConditionFiltre +  "{Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "'  ) ";
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

            Response.BinaryWrite(buffer);

            Response.End();

            // stream.Seek(0, SeekOrigin.Begin);

            // retour téléchargement pdf
            return File(stream, "application/pdf", "CustomerList.pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");
        }
        public ActionResult TacheJournaliere()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;



            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            return View("TacheJournaliere");
        }
        public ActionResult Reclamations()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;



            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            return View("Reclamations");
        }
        public ActionResult TacheParProjet()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;



            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            return View("TacheParProjet");
        }
        public ActionResult TachePrevisionnel()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            return View("TachePrevisionnel");
        }
        public ActionResult TacheGlobale()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;


            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            return View("TacheGlobal");
        }
        public ActionResult TacheResponsable()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            return View("TacheResponsable");
        }
        public ActionResult TacheParClient()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;

            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;


            return View("TacheParClient");
        }
      
        public ActionResult ImpressionTacheJournaliere(IEnumerable<string> CodeRespensable, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DurerReel = "";
            string CodeClient = "";
            string DurerExecution = "";
            var isrespensable_1 = false;
            if (Request["DurerReel"] != "")
            {
                DurerReel = Request["DurerReel"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriode.rpt"));
            //if (CodeClient != "")
            //{
            //    rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeClient.rpt"));
            //}
            //else if (DurerReel == "on")
            //{


            //}
            //else if (DurerExecution == "on")
            //{
            //    rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeExecution.rpt"));
            //}
            //else
            //{
            //    isrespensable_1 = true;
            //    rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeETAT.rpt"));
            //}






            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);




            // condition filtre
            //string Etat = "";
           // string Status = "";
           //string NumeroRapport = "";
           // string CodeRespensable = "";
            
            
            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string NumPiece = "";
            
            string Duree = "";
            string NameFile = "Rapport Journalié";
            /* if (Request["NumeroRapport"] != "")
             {
                 NumeroRapport = Request["NumeroRapport"];
             }*/
            /*if (Request["Status"] != "")
            {
                Status = Request["Status"];
            }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
            
            //if (Request["CodeRespensable"] != "")
            //{
            //    CodeRespensable = Request["CodeRespensable"];
            //}
            
            
            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }



            // rd.SetParameterValue("Etat", Etat);
            rd.SetParameterValue("NomSociete", "GROUP MAS");

            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);
            // rd.SetParameterValue("DateExecutionTache", "09/03/2022");



            //{Respensable_1.CodeRespensable} ="022" and {Crm_CycleExecTache.DateDebutExecution} in CDATE({?@DateDebut}) to CDATE({?@DateFin} )
            
            if (DurerReel == "on")
            {
                ConditionFiltre = "  {Crm_CycleExecTache.DateDebutExecution} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }
            else
            {
                ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
            }
                
            
            



            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = orcodition + " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }

                        
                    }
                    else
                    {
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }
                        
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


              //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (Nature != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + int.Parse(NumPiece) + " ";
            }
            
            if (CodeClient != "")
            {
                
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }


            if (CodeRespensable != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in CodeRespensable)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        if (isrespensable_1)
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        
                    }
                    else
                    {
                        if (isrespensable_1)
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " )";


            }

            //    if (DurerExecution == "on")
            //{
            //    ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.DurerExecution} = 1 ";
            //}
            rd.RecordSelectionFormula = ConditionFiltre;
            // rd.RecordSelectionFormula = "{Respensable.CodeRespensable} = '007' ";
            //  rd.RecordSelectionFormula = ConditionFiltre;

            // choisir quel type de document à télécharger
           // rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "E:\\Rapport\\" + NameFile + "_" + DateTime.Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm'_'ss") + ".pdf");
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
            return File(stream, "application/pdf", NameFile +".pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionTacheParProjet(IEnumerable<string> CodeRespensable, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DurerReel = "";
            string CodeClient = "";
            string DurerExecution = "";
            if (Request["DurerReel"] != "")
            {
                DurerReel = Request["DurerReel"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_Projet_Tache_Execution.rpt"));

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);


            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string NumPiece = "";

            string Duree = "";
            string NameFile = "Rapport Journalié";
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }


            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }




            ConditionFiltre = "  {Crm_CycleExecTache.DateDebutExecution} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            if (CodeRespensable != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in CodeRespensable)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                       

                    }
                    else
                    {
                      
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        

                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " )";


            }


            rd.RecordSelectionFormula = ConditionFiltre;

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
            return File(stream, "application/pdf", NameFile + ".pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionReclamations(IEnumerable<string> CodeRespensable, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DurerReel = "";
            string CodeClient = "";
            string DurerExecution = "";
            var isrespensable_1 = false;
            if (Request["DurerReel"] != "")
            {
                DurerReel = Request["DurerReel"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Reclamations.rpt"));
           
            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);


            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string NumPiece = "";

            string Duree = "";
            string NameFile = "Rapport Journalié";
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }


            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }



            
                ConditionFiltre = "  {Crm_CycleExecTache.DateDebutExecution} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            


            rd.RecordSelectionFormula = ConditionFiltre;

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
            return File(stream, "application/pdf", NameFile + ".pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionTachePrevisionnel(IEnumerable<string> CodeRespensable, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DurerReel = "";
            string CodeClient = "";
            string DurerExecution = "";
            var isrespensable_1 = false;
            if (Request["DurerReel"] != "")
            {
                DurerReel = Request["DurerReel"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            ReportDocument rd = new ReportDocument();

            if (CodeClient != "")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeExecutionPrevisionnel.rpt"));
            }
            else if (DurerReel == "on")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeExecutionPrevisionnel.rpt"));
            }
            else if (DurerExecution == "on")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeExecutionPrevisionnel.rpt"));
            }
            else
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriodeExecutionPrevisionnel.rpt"));
            }


           
            


            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);




            // condition filtre
            //string Etat = "";
           // string Status = "";
           //string NumeroRapport = "";
           // string CodeRespensable = "";
            
            
            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string NumPiece = "";
            string Duree = "";
            string NameFile = "Rapport Journalié";
            /* if (Request["NumeroRapport"] != "")
             {
                 NumeroRapport = Request["NumeroRapport"];
             }*/
            /*if (Request["Status"] != "")
            {
                Status = Request["Status"];
            }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
            
            //if (Request["CodeRespensable"] != "")
            //{
            //    CodeRespensable = Request["CodeRespensable"];
            //}
            
            
            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }


            // rd.SetParameterValue("Etat", Etat);
            rd.SetParameterValue("NomSociete", "GROUP MAS");

            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);
            // rd.SetParameterValue("DateExecutionTache", "09/03/2022");



            //{Respensable_1.CodeRespensable} ="022" and {Crm_CycleExecTache.DateDebutExecution} in CDATE({?@DateDebut}) to CDATE({?@DateFin} )
            
            if (DurerReel == "on")
            {
                ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }
            else
            {
                ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
            }
                
            
            



            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = orcodition + " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }

                        
                    }
                    else
                    {
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }
                        
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


              //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (Nature != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + NumPiece + " ";
            }
          
            if (CodeClient != "")
            {
                
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }


            if (CodeRespensable != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in CodeRespensable)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        if (isrespensable_1)
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }

                    }
                    else
                    {
                        if (isrespensable_1)
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }

                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


            }

            //    if (DurerExecution == "on")
            //{
            //    ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.DurerExecution} = 1 ";
            //}
            rd.RecordSelectionFormula = ConditionFiltre;
            // rd.RecordSelectionFormula = "{Respensable.CodeRespensable} = '007' ";
            //  rd.RecordSelectionFormula = ConditionFiltre;

            // choisir quel type de document à télécharger
           // rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "E:\\Rapport\\" + NameFile + "_" + DateTime.Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm'_'ss") + ".pdf");
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
            return File(stream, "application/pdf", NameFile +".pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionTacheGlobale(IEnumerable<string> CodeRespensable, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DurerReel = "";
            string CodeClient = "";
            string DurerExecution = "";
            var isrespensable_1 = false;
            if (Request["DurerReel"] != "")
            {
                DurerReel = Request["DurerReel"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            ReportDocument rd = new ReportDocument();

            if (CodeClient != "")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjet.rpt"));
            }
            else if (DurerReel == "on")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjet.rpt"));
            }
            else if (DurerExecution == "on")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjet.rpt"));
            }
            else
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjet.rpt"));
            }


           
            


            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);




            // condition filtre
            //string Etat = "";
           // string Status = "";
           //string NumeroRapport = "";
           // string CodeRespensable = "";
            
            
            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string NumPiece = "";
            string Duree = "";
            string NameFile = "Rapport Journalié";
            /* if (Request["NumeroRapport"] != "")
             {
                 NumeroRapport = Request["NumeroRapport"];
             }*/
            /*if (Request["Status"] != "")
            {
                Status = Request["Status"];
            }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
            
            //if (Request["CodeRespensable"] != "")
            //{
            //    CodeRespensable = Request["CodeRespensable"];
            //}
            
            
            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }


            // rd.SetParameterValue("Etat", Etat);
            rd.SetParameterValue("NomSociete", "GROUP MAS");

            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);
            // rd.SetParameterValue("DateExecutionTache", "09/03/2022");



            //{Respensable_1.CodeRespensable} ="022" and {Crm_CycleExecTache.DateDebutExecution} in CDATE({?@DateDebut}) to CDATE({?@DateFin} )
            
            if (DurerReel == "on")
            {
                ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }
            else
            {
                ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
            }
                
            
            



            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = orcodition + " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }

                        
                    }
                    else
                    {
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }
                        
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


              //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (Nature != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + NumPiece + " ";
            }
            if (CodeClient != "")
            {
                
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }


            if (CodeRespensable != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in CodeRespensable)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        if (isrespensable_1)
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }

                    }
                    else
                    {
                        if (isrespensable_1)
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }

                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


            }

            //    if (DurerExecution == "on")
            //{
            //    ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.DurerExecution} = 1 ";
            //}
            rd.RecordSelectionFormula = ConditionFiltre;
            // rd.RecordSelectionFormula = "{Respensable.CodeRespensable} = '007' ";
            //  rd.RecordSelectionFormula = ConditionFiltre;

            // choisir quel type de document à télécharger
           // rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "E:\\Rapport\\" + NameFile + "_" + DateTime.Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm'_'ss") + ".pdf");
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
            return File(stream, "application/pdf", NameFile +".pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionTacheResponsable(IEnumerable<string> CodeRespensable, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DurerReel = "";
            string CodeClient = "";
            string DurerExecution = "";
            var isrespensable_1 = false;
            if (Request["DurerReel"] != "")
            {
                DurerReel = Request["DurerReel"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            ReportDocument rd = new ReportDocument();

            if (CodeClient != "")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjetResponsable.rpt"));
            }
            else if (DurerReel == "on")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjetResponsable.rpt"));
            }
            else if (DurerExecution == "on")
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjetResponsable.rpt"));
            }
            else
            {
                isrespensable_1 = true;
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParProjetResponsable.rpt"));
            }






            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);




            // condition filtre
            //string Etat = "";
            // string Status = "";
            //string NumeroRapport = "";
            // string CodeRespensable = "";


            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string NumPiece = "";
            string Duree = "";
            string NameFile = "Rapport Journalié";
            /* if (Request["NumeroRapport"] != "")
             {
                 NumeroRapport = Request["NumeroRapport"];
             }*/
            /*if (Request["Status"] != "")
            {
                Status = Request["Status"];
            }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }

            //if (Request["CodeRespensable"] != "")
            //{
            //    CodeRespensable = Request["CodeRespensable"];
            //}


            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }


            // rd.SetParameterValue("Etat", Etat);
            rd.SetParameterValue("NomSociete", "GROUP MAS");

            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);
            // rd.SetParameterValue("DateExecutionTache", "09/03/2022");



            //{Respensable_1.CodeRespensable} ="022" and {Crm_CycleExecTache.DateDebutExecution} in CDATE({?@DateDebut}) to CDATE({?@DateFin} )

            if (DurerReel == "on")
            {
                ConditionFiltre = "  {Crm_Projet.DateDeclenchement} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }
            else
            {
                ConditionFiltre = "  {Crm_Projet.DateDeclenchement} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
            }






            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = orcodition + " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }


                    }
                    else
                    {
                        // status non commencer
                        if (value == "E11")
                        {
                            orcodition = " {Crm_TacheReclamation.Status} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                        }

                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (Nature != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + NumPiece + " ";
            }
            if (CodeClient != "")
            {

                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }


            if (CodeRespensable != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in CodeRespensable)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        if (isrespensable_1)
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                        }

                    }
                    else
                    {
                        if (isrespensable_1)
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }
                        else
                        {
                            orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                        }

                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


            }

            //    if (DurerExecution == "on")
            //{
            //    ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.DurerExecution} = 1 ";
            //}
            rd.RecordSelectionFormula = ConditionFiltre;
            // rd.RecordSelectionFormula = "{Respensable.CodeRespensable} = '007' ";
            //  rd.RecordSelectionFormula = ConditionFiltre;

            // choisir quel type de document à télécharger
            // rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "E:\\Rapport\\" + NameFile + "_" + DateTime.Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm'_'ss") + ".pdf");
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
            return File(stream, "application/pdf", NameFile + ".pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionTacheParClient(IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string Duree = Request["Duree"];
            string Estimatif = Request["Estimatif"];
            string EstimatifGLobale = Request["EstimatifGLobale"];
            string ReclamationTacheGLobale = Request["ReclamationTacheGLobale"];
            string ReclamationClientETTache = Request["ReclamationClientETTache"];
            string fileName = "Etat_TacheParClient.rpt";
            ReportDocument rd = new ReportDocument();

            if (Duree == "on")
            {
                fileName = "Etat_TacheParClientParPeriode.rpt";
            }
            if (Estimatif == "on")
            {
                fileName = "Etat_TacheParClientParPeriodeDureeEstimatif.rpt";
            }
            if (EstimatifGLobale == "on")
            {
                fileName = "Etat_TacheParClientParPeriodeDureeEstimatifGlobale.rpt";
            }
            if (ReclamationTacheGLobale == "on")
            {
                fileName = "Etat_ClientReclamationTacheGlobale.rpt";
            }
             if (ReclamationClientETTache == "on")
            {
                fileName = "Etat_ReclamationClientEtTaches.rpt";
            }



            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), fileName));
            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);




            // condition filtre
            //string Etat = "";
           // string Status = "";
            //string NumeroRapport = "";
            string CodeRespensable = "";
            string DurerExecution = "";
            string ConditionFiltre = "";
            string DateDebut = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string CodeClient = "";
            string NumPiece = "";
            // string Duree = "";
            /* if (Request["NumeroRapport"] != "")
             {
                 NumeroRapport = Request["NumeroRapport"];
             }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
           /* if (Request["Status"] != "")
            {
                Status = Request["Status"];
            }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["CodeRespensable"] != "")
            {
                CodeRespensable = Request["CodeRespensable"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }
            // rd.SetParameterValue("Etat", Etat);
            rd.SetParameterValue("NomSociete", "GROUP MAS");

            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);
            // rd.SetParameterValue("DateExecutionTache", "09/03/2022");

            

            //{Respensable_1.CodeRespensable} ="022" and {Crm_CycleExecTache.DateDebutExecution} in CDATE({?@DateDebut}) to CDATE({?@DateFin} )
            ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
            if (ReclamationTacheGLobale == "on")
            {
                ConditionFiltre = "  {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }
            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {

                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (Nature != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheNature.Nature}  = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }

            if (CodeRespensable != "")
            {
                
                ConditionFiltre = ConditionFiltre + " and {Crm_TacheLiee.CodeRespensable} = '" + CodeRespensable + "' ";
            }
          /*  if (Status != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }*/
            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + NumPiece + " ";
            }
            if (CodeClient != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }

          
            rd.RecordSelectionFormula = ConditionFiltre;


            // choisir quel type de document à télécharger

            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            // type de document à construire
            Response.ContentType = "application/pdf";


            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, (int)stream.Length);

            stream.Read(buffer, 0, (int)stream.Length);

            Response.BinaryWrite(buffer);

            Response.End();

            // stream.Seek(0, SeekOrigin.Begin);

            // retour téléchargement pdf
            return File(stream, "application/pdf", "TacheParClient.pdf");
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
                //logoninfo.TableName = "Crm_TacheReclamation";
                tbl.ApplyLogOnInfo(logoninfo);
                //tbl.Location = "Crm_TacheReclamation";
            }
        }
        // téléchargement pdf excel word
        public ActionResult ImpressionIntervention(string d1, string d2)
        {
            string docType = "";
            string fileName = "";
            ExportFormatType formatFile = new ExportFormatType();


            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TacheServiceTechnique.rpt"));


            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);


            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);

            //string @DateDebut = d11.ToString("dd/MM/yyyy");
            //string @DateFin = d22.ToString("dd/MM/yyyy");

            string @DateDebut = d11.Day + "/" + d11.Month + "/" + d11.Year;

            string @DateFin = d11.Day + "/" + d11.Month + "/" + d11.Year;



            string ConditionFiltre = "";
            rd.SetParameterValue("NomSociete", "GROUP MAS");
            rd.SetParameterValue("@DateDebut", @DateDebut);
            rd.SetParameterValue("@DateFin", @DateFin);

            ConditionFiltre = "{Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "'  ) ";
            rd.RecordSelectionFormula = ConditionFiltre;


            // diferent format

            if (Request["Format"] == "PDF")
            {
                formatFile = ExportFormatType.PortableDocFormat;
                docType = "application/pdf";
                fileName = "ListeInterventions.pdf";
            }
            if (Request["Format"] == "EXCEL")
            {
                formatFile = ExportFormatType.Excel;
                docType = "application/vnd.ms-excel";
                fileName = "ListeInterventions.xls";
            }
            if (Request["Format"] == "WORD")
            {
                formatFile = ExportFormatType.WordForWindows;
                docType = "application/doc";
                fileName = "ListeInterventions.doc";
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
        public ActionResult TacheParRapport()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;
            Crm_TacheReclamation crm_TacheReclamation = new Crm_TacheReclamation();

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;


            var ListRapport = new SelectList(db.Crm_TacheReclamation.ToList(), "NumDossier", "NumDossier");
            ViewData["ListRapport"] = ListRapport;
            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
            crm_TacheReclamation.ListeRapport = RapportData();

            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;


            return View("TacheParRapport", crm_TacheReclamation);
        }
        public ActionResult TacheEstimatif()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;
            Crm_TacheReclamation crm_TacheReclamation = new Crm_TacheReclamation();

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;

            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom");
            ViewData["ListResponsable"] = ListResponsable;


            var ListRapport = new SelectList(db.Crm_Rapport.ToList(), "NumDossier", "Nom");
            ViewData["ListResponsable"] = ListResponsable;



            //var ListRapport = new SelectList(db.Crm_TacheReclamation.ToList(), "NumDossier", "NumDossier");
            //ViewData["ListRapport"] = ListRapport;


            crm_TacheReclamation.ListeRapport = new List<Crm_Rapport>();
            crm_TacheReclamation.ListeRapport = RapportData();

            crm_TacheReclamation.ListeRespensables = new List<Respensable>();
            crm_TacheReclamation.ListeRespensables = ResponsableData();

            var ListEtat = new SelectList(db.EtatCRM.ToList(), "NumeroEtat", "Libelle");
            ViewData["ListEtat"] = ListEtat;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            return View("ImpressionTache", crm_TacheReclamation);
        }
        public ActionResult ImpressionTacheEstimatif(IEnumerable<string> SelectedresponsableId, IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            string DureeReel = Request["DureeReel"];


            string Duree = Request["Duree"];
            // string signature = Request["signature"];
            string fileName = "Etat_TachesParPeriodeParRapportEstim.rpt";

            ReportDocument rd = new ReportDocument();

            if (DureeReel == "on")
            {
                fileName = "Etat_TachesParPeriodeParRapportDureeREstim.rpt";
            }
            if (Duree == "on")
            {
                fileName = "Etat_TachesParPeriodeParRapportDureeEstimeReelEstim.rpt";
            }
            
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), fileName));
            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            // condition filtre
           // string NumeroEtat = "";
            string CodeRespensable = "";
            string DurerExecution = "";
            //string DurerEstimee = "";
            //string DureeReel = "";
            // string SignatureClient = "";
            string ConditionFiltre = "";
            string DateDebut = "";
            string isDossier = "";
          //  string Status = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string CodeClient = "";
            string NumPiece = "";
            //   string Duree = "";
            string[] NumDossier = Array.Empty<string>();
            if (Request["NumDossier"] != null)
            {
                NumDossier = Request["NumDossier"].ToString().Split(',');
                //NumDossier = Request["NumDossier"].explode(",", "blah1|blah2|blah3");
            }
            
            if (Request["isDossier"] != "")
            {
                isDossier = Request["isDossier"];
            }
      
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["CodeRespensable"] != "")
            {
                CodeRespensable = Request["CodeRespensable"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            if (Request["[DureeReel]"] != "")
            {
                DureeReel = Request["DureeReel"];
            }

            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }

            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }

            rd.SetParameterValue("Etat", "Tous");
            rd.SetParameterValue("NomSociete", "GROUP MAS");
            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);

            if (NumDossier.Count() > 0  && isDossier == "on")
            {
                string orcodition = "";
                for (int i = 0; i< NumDossier.Count(); i++)
                {

                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheReclamation.NumDossier} = '" + NumDossier[i] + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheReclamation.NumDossier} = '" + NumDossier[i] + "' ";
                    }

                    

                }
                ConditionFiltre = ConditionFiltre + "  ( " + orcodition + " ) ";

            }
            
            else if(NumDossier.Count() > 0)
            {

                string orcodition = "";
                for (int i = 0; i < NumDossier.Count(); i++)
                {

                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheReclamation.NumDossier} = '" + NumDossier[i] + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheReclamation.NumDossier} = '" + NumDossier[i] + "' ";
                    }

                    

                }
                ConditionFiltre = ConditionFiltre + "  ( " + orcodition + " ) ";
                ConditionFiltre = ConditionFiltre + " and {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }else
            {
                
                ConditionFiltre = ConditionFiltre + " {Crm_TacheReclamation.DateCreation} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

            }

            if (SelectedresponsableId != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in SelectedresponsableId)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Respensable.CodeRespensable} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Respensable.CodeRespensable} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


            }
            /*if (NumeroEtat != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {EtatCRM.NumeroEtat} = '" + NumeroEtat + "' ";
            }*/
            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


            }

            if (Nature != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheNature.Nature} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


            }
            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                //ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + NumPiece + " ";
            }
            if (CodeClient != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }
          
            /* if (Duree == "on")
             {
                 ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.DurerExecution} = 1 ";
             } 
            /* if (DureeReel == "on")
             {
                 ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.DureeReel} = 1 ";
             }*/



            rd.RecordSelectionFormula = ConditionFiltre;
            //rd.RecordSelectionFormula = "{Respensable.CodeRespensable} = '007' ";
            //  rd.RecordSelectionFormula = ConditionFiltre;

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
            return File(stream, "application/pdf", "TacheEstimatif.pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult ImpressionTacheParRapport(IEnumerable<string> SelectedresponsableId,IEnumerable<string> NumeroEtat,IEnumerable<string> Nature)
        {
            string DureeReel = Request["DureeReel"];
            string Duree = Request["Duree"];
            string signature = Request["signature"];
            string fileName = "Etat_TacheParClient.rpt";

            // ExportFormatType formatFile = new ExportFormatType();


            ReportDocument rd = new ReportDocument();


            
            /*if (Duree == "on")
            {
                fileName = "Etat_TachesParPeriodeParRapportDureeE.rpt";
            }
            if (signature == "on")
            {
                fileName = "Etat_TachesParPeriodeParRapportDureeESignature.rpt";
            }

            if (DureeReel == "on")
            {
                fileName = "Etat_TachesParPeriodeParRapportDureeR.rpt";
            }*/



            if (DureeReel == "on")
            {
                if (Duree == "on")
                {
                    fileName = "Etat_TachesParPeriodeParRapportDureeEstimeReel.rpt";
                }
                else
                {
                    fileName = "Etat_TachesParPeriodeParRapportDureeR.rpt";
                }
            }
            else
            {
                if (Duree == "on")
                {
                    if (signature == "on")
                    {
                        fileName = "Etat_TachesParPeriodeParRapportDureeESignature.rpt";
                    }
                    else
                    {
                        fileName = "Etat_TachesParPeriodeParRapportDureeE.rpt";
                    }
                }
                else
                {
                    //fileName = "Etat_TachesParPeriodeParRapport.rpt";
                    fileName = "Etat_TacheParClient.rpt";
                }
            }

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), fileName));

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            // condition filtre
            string Etat = "";
            //string NumeroEtat = "";
            string CodeRespensable = "";
            string DurerExecution = "";
            string ConditionFiltre = "";
            string DateDebut = "";
            string NumDossier = "";
            string DateFin = "";
            string NomPlanificateur = "";
            string NomValidateur = "";
            string NomTesteur = "";
            string CodeClient = "";
            string NumPiece = "";
           
           
           // string Nature = "";

            //   string Duree = "";
            if (Request["NumDossier"] != "")
            {
                NumDossier = Request["NumDossier"];
            }
           /* if (Request["NumeroEtat"] != "")
            {
                NumeroEtat = Request["NumeroEtat"];
            }*/
            if (Request["Duree"] != "")
            {
                Duree = Request["Duree"];
            }
            if (Request["CodeClient"] != "")
            {
                CodeClient = Request["CodeClient"];
            }
            if (Request["CodeRespensable"] != "")
            {
                CodeRespensable = Request["CodeRespensable"];
            }
            if (Request["DurerExecution"] != "")
            {
                DurerExecution = Request["DurerExecution"];
            }
            if (Request["[DureeReel]"] != "")
            {
                DureeReel = Request["DureeReel"];
            }
            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            if (Request["NomPlanificateur"] != "")
            {
                NomPlanificateur = Request["NomPlanificateur"];
            }
            if (Request["NomValidateur"] != "")
            {
                NomValidateur = Request["NomValidateur"];
            }
            if (Request["NomTesteur"] != "")
            {
                NomTesteur = Request["NomTesteur"];
            }
            if (Request["NumPiece"] != "")
            {
                NumPiece = Request["NumPiece"];
            }
            /* if (Request["Nature"] != "")
             {
                 Nature = Request["Nature"];

             }*/

            //DateDebut = "01/10/2021";
            //DateFin = "01/03/2022";


            if (DureeReel == "on")
            {
                rd.SetParameterValue("Etat", Etat);
            }
            if (Duree == "on")
            {
                rd.SetParameterValue("Etat", Etat);
            }
            if (signature == "on")
            {
                rd.SetParameterValue("Etat", Etat);
            }


            rd.SetParameterValue("NomSociete", "GROUP MAS");
            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);


            if (NumDossier != "")
            {
                ConditionFiltre = " {Crm_TacheReclamation.NumDossier} = '" + NumDossier + "' ";
            }
            else
            {
             
                ConditionFiltre = " {Crm_TacheReclamation.DateCreation} IN CDATE('" + DateDebut + "' ) to CDATE('" + DateFin + "' ) ";

            }

            if (SelectedresponsableId != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in SelectedresponsableId)
                {



                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        
                        orcodition = orcodition + " {Crm_TacheLiee.CodeRespensable} = '" + value + "' ";
                    }
                    else
                    {
                        
                        orcodition = " {Crm_TacheLiee.CodeRespensable} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";

            }
            if (NumeroEtat != null)
            {
                string orcodition = "";
                int i = 0;

                foreach (string value in NumeroEtat)
                {
                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {EtatCRM.NumeroEtat} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {EtatCRM.NumeroEtat} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";


                //  ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Status} = '" + Status + "' ";
            }
            if (NomPlanificateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomPlanificateur} = '" + NomPlanificateur + "' ";
            }
            if (NomValidateur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomValidateur} = '" + NomValidateur + "' ";
            }
            if (NomTesteur != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NomTesteur} = '" + NomTesteur + "' ";
            }
            if (NumPiece != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.NumPiece} = " + NumPiece + " ";
            }
            if (CodeClient != "")
            {
                ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.CodeClient} = '" + CodeClient + "' ";
            }
            
            if (Nature != null)
            {
             
                string orcodition = "";
                int i = 0;

                foreach (string value in Nature)
                {
                    if (i > 0)
                    {
                        orcodition = orcodition + " or ";
                        orcodition = orcodition + " {Crm_TacheReclamation.Nature} = '" + value + "' ";
                    }
                    else
                    {
                        orcodition = " {Crm_TacheReclamation.Nature} = '" + value + "' ";
                    }

                    i++;

                }

                ConditionFiltre = ConditionFiltre + " and ( " + orcodition + " ) ";

                //ConditionFiltre = ConditionFiltre + " and  {Crm_TacheReclamation.Nature} = '" + Nature + "' ";
            }
            
            rd.RecordSelectionFormula = ConditionFiltre;
            //rd.RecordSelectionFormula = "{Respensable.CodeRespensable} = '007' ";
            //  rd.RecordSelectionFormula = ConditionFiltre;

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
            return File(stream, "application/pdf", "TacheParRapport.pdf");
            //return File(stream, "application/vnd.ms-excel", "CustomerList.xls");
            //return File(stream, "application/doc", "CustomerList.doc");

        }
        public ActionResult SearchCalendrier()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var model = from r in db.Crm_TacheReclamation
                        where r.DateCreation > d1 && r.DateCreation < d2
                        select r;
            return View("Calendrier", model.ToList());
        }
        public ActionResult Calendrier()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            return View("Calendrier");
        }
        public ActionResult listeTacheCalendar(Crm_TacheReclamation reclamation)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var json = JsonConvert.SerializeObject(reclamation);
            //var code = reclamation.CodeRespensable.ToString();
            // if (reclamation.UtilisateurCreateur == null)
            // {
            json = JsonConvert.SerializeObject(db.Crm_TacheReclamation.Where(s => s.UtilisateurCreateur.Contains(reclamation.UtilisateurCreateur)));
            //  }

            /* if (reclamation.UtilisateurCreateur != null)
             {
                 json = JsonConvert.SerializeObject(db.Crm_TacheReclamation.Where(s => s.TacheTitre.Contains(reclamation.TacheTitre)).Where(s => s.UtilisateurCreateur.Contains(reclamation.UtilisateurCreateur)));
             }*/
            //var json = JsonConvert.SerializeObject(db.Crm_TacheReclamation.Where(s => s.UtilisateurCreateur.Contains(reclamation.UtilisateurCreateur)).Where(s => s.NomPlanificateur.Contains(reclamation.CodeRespensable)));
            //return View(reclamation);


            // list utilisateur
            var ListUtilisateur = new SelectList(db.Utilisateur.ToList(), "NomUtilisateur", "NomUtilisateur");
            ViewData["ListUtilisateur"] = ListUtilisateur;



            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Email()
        {
            return View();
        }
        //public void openOutlookemailbox()
        //{
        //    /* MailMessage mail = new MailMessage();
        //     mail.From = new MailAddress("abc.Info@gmail.com");
        //     mail.To.Add(new MailAddress("xyz.Info@gmail.com"));
        //     mail.IsBodyHtml = true;
        //     mail.Body = "Test";
        //     var outlookmail = "mailto:"+mail.To.ToString() + "?subject =" + "Test subject" + "&body =" +mail.Body.ToString();
        //     return outlookmail;*/
        //    try
        //    {
        //        List<string> lstAllRecipients = new List<string>();
        //        //Below is hardcoded - can be replaced with db data
        //        lstAllRecipients.Add("sanjeev.kumar@testmail.com");
        //        lstAllRecipients.Add("chandan.kumarpanda@testmail.com");

        //        Outlook.Application outlookApp = new Outlook.Application();
        //        Outlook._MailItem oMailItem = (Outlook._MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
        //        Outlook.Inspector oInspector = oMailItem.GetInspector;
        //        // Thread.Sleep(10000);

        //        // Recipient
        //        Outlook.Recipients oRecips = (Outlook.Recipients)oMailItem.Recipients;
        //        foreach (String recipient in lstAllRecipients)
        //        {
        //            Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(recipient);
        //            oRecip.Resolve();
        //        }

        //        //Add CC
        //        Outlook.Recipient oCCRecip = oRecips.Add("THIYAGARAJAN.DURAIRAJAN@testmail.com");
        //        oCCRecip.Type = (int)Outlook.OlMailRecipientType.olCC;
        //        oCCRecip.Resolve();

        //        //Add Subject
        //        oMailItem.Subject = "Test Mail";

        //        // body, bcc etc...

        //        //Display the mailbox
        //        oMailItem.Display(true);
        //    }
        //    catch (Exception objEx)
        //    {
        //        Response.Write(objEx.ToString());
        //    }
        //}


        // GET: Home/JqAJAX  
        [HttpPost]
        public ActionResult CreateRapport(Crm_Rapport Crm_Rapport)
        {
            try
            {

                /*Primary key */
                SecurityServices securityService = new SecurityServices();
                var CompteurId = securityService.GetIdByCompteur("RP", "220").ToList();
                foreach (var item in CompteurId)
                {
                    ViewData["Compteur"] = item;
                }
                decimal valeur;
                valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;

                Crm_Rapport.NumeroRapport = "RP" + "220" + valeur.ToString("00000");
                /*Primary key */

                Crm_Rapport.NumeroDossier = " ";
                Crm_Rapport.NumeroEtat = " ";
                Crm_Rapport.Status = "E05";
                Crm_Rapport.UtilisateurCreateur = Session["UserID"].ToString();
                Crm_Rapport.DateCreation = DateTime.Now;
                Crm_Rapport.DateRapport = DateTime.Now;

                /*Control IdBase*/
                CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "crm_rapport");
                decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur); decimal valeur2 = Convert.ToDecimal(Crm_Rapport.NumeroRapport.Substring(4));

                if (valeur2 - valeur1 > 0)
                {
                    Crm_Rapport.NumeroRapport = "RP" + "220" + valeur2.ToString("00000");

                }
                else
                {

                    Crm_Rapport.NumeroRapport = "RP" + "220" + (valeur1 + 1).ToString("00000");

                }

                compteurPiece = db.CompteurPiece.Find("SOC001", "crm_rapport");
                compteurPiece.Compteur = Crm_Rapport.NumeroRapport.Substring(4);
                db.Entry(compteurPiece).State = EntityState.Modified;
                /*Control IdBase*/

                db.Crm_Rapport.Add(Crm_Rapport);
                db.SaveChanges();
                //TempData["SuccessMesage"] = "Rapport N°" + @Crm_Rapport.NumeroRapport + " est enregistrée! ";

                var ListNewRapport = new SelectList(db.Crm_Rapport.Where(r => r.CodeClient.Contains(Crm_Rapport.CodeClient)).ToList(), "NumeroRapport", "NumeroRapport");
                ViewData["ListRapport"] = ListNewRapport;
               



                //return Json(new
                //{
                //    msg = "Rapport N°" + @Crm_Rapport.NumeroRapport + " est enregistrée! ",
                //    ListNewRapport,
                //});
                return Json(ListNewRapport);

                //return View("~/Crm_TacheReclamation/Create", ListRapport) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // GET: Home/JqAJAX  
        [HttpPost]
       public ActionResult CreateReclamation(string CodeClientc, string NomContact, string Description, IEnumerable<string> SelectedUtilisateurId, string Dureec, string UtilisateurContact, string Observation, string Naturec, string Titrec, string CodeMoyenCommunication, string OutilCommunication)
        {
            try
            {
                Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
                crm_ReclamationClient.CodeClient = CodeClientc;
                crm_ReclamationClient.RaisonSociale = db.Client.Find(CodeClientc).RaisonSociale;
                crm_ReclamationClient.NomContact = int.Parse(NomContact);
                crm_ReclamationClient.Description = Description;
                crm_ReclamationClient.SelectedUtilisateurId = SelectedUtilisateurId;
                crm_ReclamationClient.Duree = Int32.Parse(Dureec);
                crm_ReclamationClient.UtilisateurContact = UtilisateurContact;
                crm_ReclamationClient.Observation = Observation;
                crm_ReclamationClient.Nature = Naturec;
                crm_ReclamationClient.Titre = Titrec;
                crm_ReclamationClient.CodeMoyenCommunication = CodeMoyenCommunication;
                crm_ReclamationClient.OutilCommunication = OutilCommunication;
                crm_ReclamationClient.DateReclamation = DateTime.Now;
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
                /*Primary key */

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

                /*Control IdBase*/
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
                /*Control IdBase*/

                db.Crm_ReclamationClient.Add(crm_ReclamationClient);
                
                db.SaveChanges();

                /*Add list respensable to Database*/
                Crm_PersonneConcerner crm_PersonneConcerner = new Crm_PersonneConcerner();
               
                foreach (var item in crm_ReclamationClient.SelectedresponsableId)
                {
                    crm_PersonneConcerner = new Crm_PersonneConcerner();
                    crm_PersonneConcerner.NomUtilisateur = item;
                    crm_PersonneConcerner.NumeroReclamation = crm_ReclamationClient.Id_Reclamation;
                    crm_PersonneConcerner.Vue = false;
                    crm_PersonneConcerner.DateVue = "";
                    db.Crm_PersonneConcerner.Add(crm_PersonneConcerner);
                    db.SaveChanges();
                }

                var ListNewReclamation = new SelectList(db.Crm_ReclamationClient.Where(r => r.CodeClient == crm_ReclamationClient.CodeClient).ToList(), "Id_Reclamation", "Id_Reclamation");
                ViewData["ListNewReclamation"] = ListNewReclamation;




                //return Json(new
                //{
                //    msg = "Rapport N°" + @Crm_Rapport.NumeroRapport + " est enregistrée! ",
                //    ListNewRapport,
                //});
                return Json(ListNewReclamation);

                //return View("~/Crm_TacheReclamation/Create", ListRapport) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult ExporttoEmail(IEnumerable<string> NumeroEtat, IEnumerable<string> Nature)
        {
            //parametrage email avant envoi
            string EmailFrom = "support@mas-alu.com";
            string Subject = "Rapport Journalier";
            string Body = "Ci joint les rapports journaliers du CRM a la date du "+ DateTime.Now.ToString("dd'/'MM'/'yyyy") + "\n - verifier les taches.\n - maitre a jour vos rapports\n\n\nNB: le temps de realisation et l'efficacité des resultats sont prises en consideration dans le calcul du prime de responsabilité. \n Bonne réception";
            string EmailCredential = "hakram@mas-alu.com";
            string PasswordCredential = "Mas@2022";
            //string EmailTo = "";
            string NomResponsable = "";
            // set default params for email
            SmtpClient SmtpServer = new SmtpClient("mail.mas-alu.com");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(EmailFrom);
            mail.Subject = Subject;
            mail.Body = Body;
            //System.Net.Mail.Attachment attachment;

            // equipe soft - eclude (022, 023, 030, 027)
            string[] CodeResponsableToGenerateReport = { "004", "026", "020", "024", "010", "003", "015", "029", "007" };
            //equipe member
            string[] CodeCollaborateurToGenerateReport = { "004", "026", "020", "024", "010", "003", "015", "029", "007" };

            // ### loop liste of responsable chef projet et admin ####
            /* ### get liste of email of responsable by code responsable  */
            //generate all rapport off all responsable

            // liste off all responsable
            var ListeAllResponsable = db.Respensable.ToList();
            foreach (var item in ListeAllResponsable)
            {
                // list of responsable to generate report
                if (CodeResponsableToGenerateReport.Contains(item.CodeRespensable.ToString()))
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriode.rpt"));

                    // set connection to crp file
                    string Serveur = ConfigurationManager.AppSettings["Serveur"];
                    string Db = ConfigurationManager.AppSettings["Db"];
                    string Usr = ConfigurationManager.AppSettings["Usr"];
                    string Pwd = ConfigurationManager.AppSettings["Pwd"];
                    setDbInfo(rd, Serveur, Db, Usr, Pwd);

                    // condition filtre
                    string CodeRespensable = "";
                    string ConditionFiltre = "";
                    string DateDebut = "";
                    string DateFin = "";

                    if (Request["CodeRespensable"] != "")
                    {
                        CodeRespensable = Request["CodeRespensable"];
                    }
                    DateDebut = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    DateFin = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    rd.SetParameterValue("NomSociete", "GROUP MAS");

                    rd.SetParameterValue("@DateDebut", DateDebut);
                    rd.SetParameterValue("@DateFin", DateFin);


                    ConditionFiltre = "  {Crm_CycleExecTache.DateDebutExecution} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
                    
                    string NameFile = "RJ";
                    // get responsable name
                    NomResponsable = db.Respensable.Find(item.CodeRespensable).Nom.ToString();
                    // generate file name
                    NameFile = NameFile + " " + NomResponsable + " - " + item.CodeRespensable;
                    // set condition for crystal report
                    ConditionFiltre = ConditionFiltre + " and {Respensable_1.CodeRespensable} = '" + item.CodeRespensable.ToString() + "' ";
                    rd.RecordSelectionFormula = ConditionFiltre;
                    // record in folder report PDF
                    //string AttachementFileRoot = "E:\\Rapport\\" + NameFile + "_" + DateTime.Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm") + ".pdf";
                    //rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, AttachementFileRoot);
                    // ajouter le fichier pour l'email à envoyer
                   // attachment = new System.Net.Mail.Attachment(AttachementFileRoot);
                   // mail.Attachments.Add(attachment);

                }
            }

            // #### en loop generate rapport ####

            // send all rapport by email

            /*  ### loop liste eamil responsable ### */
            //var ListeAllEmailResponsable = db.Respensable.ToList();
           // foreach (var item in ListeAllEmailResponsable)
            //{
                // email of responsable
               // EmailTo = item.Nom + "@ideal2s.com";
               // email des équipe soft
                //EmailTo = "akram.hamdi@ideal2s.com";
                // add all email to send at once
                mail.To.Add("hakram@mas-alu.com");
                //mail.To.Add("kaymen@mas-alu.com");
               // mail.To.Add("achokri@mas-alu.com");
               // mail.To.Add("hakram@mas-alu.com");

               // mail.CC.Add("bkamel@mas-alu.com");
               // mail.CC.Add("achokri@mas-alu.com");
            //}
            /*  ### end loop liste eamil responsable ### */
            
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential(EmailCredential, PasswordCredential);
           // SmtpServer.EnableSsl = true;
            


            //send email to every member by one
            // liste off all responsable
            /*
            foreach (var item in ListeAllResponsable)
            {
                // list of responsable to generate report
                if (CodeCollaborateurToGenerateReport.Contains(item.CodeRespensable.ToString()))
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_TachesParPeriode.rpt"));

                    // set connection to crp file
                    string Serveur = ConfigurationManager.AppSettings["Serveur"];
                    string Db = ConfigurationManager.AppSettings["Db"];
                    string Usr = ConfigurationManager.AppSettings["Usr"];
                    string Pwd = ConfigurationManager.AppSettings["Pwd"];
                    setDbInfo(rd, Serveur, Db, Usr, Pwd);

                    // condition filtre
                    string CodeRespensable = "";
                    string ConditionFiltre = "";
                    string DateDebut = "";
                    string DateFin = "";

                    if (Request["CodeRespensable"] != "")
                    {
                        CodeRespensable = Request["CodeRespensable"];
                    }
                    DateDebut = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    DateFin = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    rd.SetParameterValue("NomSociete", "GROUP MAS");

                    rd.SetParameterValue("@DateDebut", DateDebut);
                    rd.SetParameterValue("@DateFin", DateFin);


                    ConditionFiltre = "  {Crm_CycleExecTache.DateDebutExecution} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";

                    string NameFile = "RJ";
                    // get responsable name
                    NomResponsable = db.Respensable.Find(item.CodeRespensable).Nom.ToString();
                    // generate file name
                    NameFile = NameFile + " " + NomResponsable + " - " + item.CodeRespensable;
                    // set condition for crystal report
                    ConditionFiltre = ConditionFiltre + " and {Respensable_1.CodeRespensable} = '" + item.CodeRespensable.ToString() + "' ";
                    rd.RecordSelectionFormula = ConditionFiltre;
                    // record in folder report PDF
                    string AttachementFileRoot = "E:\\Rapport\\" + NameFile + "_" + DateTime.Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm") + ".pdf";
                    rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, AttachementFileRoot);
                    // ajouter le fichier pour l'email à envoyer
                    attachment = new System.Net.Mail.Attachment(AttachementFileRoot);
                    mail.Attachments.Add(attachment);

                }
            }
           */
            
            try
            {
                SmtpServer.Send(mail);
                return RedirectToAction("Success", "Crm_TacheReclamation", new { returnUrl = "" });
            }
            catch (SmtpFailedRecipientException ex)
            {
                 //ex.FailedRecipient and ex.GetBaseException() should give you enough info.
                return RedirectToAction("Error", "Crm_TacheReclamation", new { returnUrl = ex.GetBaseException() });
            }
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public List<ContactClient> ContactClientData()
        {
            return db.ContactClient.ToList();
        }

       public List<Client> ClientData()
        {
            return db.Client.ToList();
        }
       public List<Utilisateur> UtilisateurData()
        {
            return db.Utilisateur.ToList();
        }

        public ActionResult ListTachesCommercial()
        {
            return View("Index", db.Crm_TacheReclamation.Where(s => s.Nature.Contains("C")).ToList());
        }

        public ActionResult ListTachesCommercialLieSoftTech()
        {
            Crm_TacheReclamation crm_TacheReclamation = new Crm_TacheReclamation();
              var result =
              from a in db.Crm_TacheReclamation
              join b in db.Crm_TacheCommercialSoft on a.NumeroTache equals b.NumeroTacheSoft_Tecnique into ct
              from b in ct.DefaultIfEmpty()
              where (a.Nature == "C" && a.NumeroTache == b.NumeroTacheSoft_Tecnique)
              select new
              {
                  a.Duree,
                  a.NumeroTache,
                  a.UtilisateurCreateur,
                  a.CodeClient,
                  a.DescriptionTache,
                  a.TacheTitre,
                  a.NomPlanificateur,
                  a.NomTesteur,
                  a.NomValidateur,
                  a.NumDossier,
                  a.NumeroEtat,
                  a.NumeroOrdre,
                  a.RaisonSociale,
                  a.Status,
                  a.DateCreation,
                  a.Nature,
              };
            var model = result.ToList();
            List<Crm_TacheReclamation> listTache = new List<Crm_TacheReclamation>();
            foreach(var item in model)
            {
                listTache.Add(db.Crm_TacheReclamation.Find(item.NumeroTache)); 
            }
            
            return View("Index", listTache);
        }
    }
}

