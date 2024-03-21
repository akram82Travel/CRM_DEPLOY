using CRMSTUBSOFT.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace CRMSTUBSOFT.Controllers
{

    public class HomeController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            SecurityServices securityService = new SecurityServices();

            Crm_TacheReclamation Taches = new Crm_TacheReclamation();
            string usernom = Session["UserID"].ToString();
            string coderespensable = Session["codeResponsable"].ToString();

            Crm_EquipeRespensable CodeEquipe = db.Crm_EquipeRespensable.FirstOrDefault(e => e.CodeRespensable == coderespensable);
            Crm_Equipe ChefEquipe = db.Crm_Equipe.FirstOrDefault(e => e.CodeEquipe == CodeEquipe.CodeEquipe);
            List<string> ListeEquipe = db.Crm_EquipeRespensable.Where(e => e.CodeEquipe == CodeEquipe.CodeEquipe).Select(e => e.CodeRespensable).ToList();
            Crm_AccesUtilisateur grade = db.Crm_AccesUtilisateur.FirstOrDefault(e => e.NomUtilisateur == usernom);//grade.CodeAccess



            List<Crm_TacheReclamation> Model = securityService.GetAllListTacheByStatus("E07", coderespensable, ChefEquipe, grade, ListeEquipe).ToList();
            //NOMBRE TACHE EN COURS
            ViewData["NumTacheEnCour"] = securityService.GetListTacheByStatus("E02", usernom);
            // NOMBRE TACHE EN EXECUTION
            ViewData["NumTacheExecuter"] = securityService.GetListTacheByStatus("E07", usernom);
            // NOMBRE TACHE NON COMMENCER
            if (Session["UserRole"].ToString() != "03")
            {
                ViewData["NumTacheNonCommencer"] = securityService.GetALLListTacheNoncommencer(usernom, "E11", coderespensable);
            }
            else
            {
                ViewData["NumTacheNonCommencer"] = securityService.GetALLListTacheNoncommencer1(usernom, "E11", coderespensable);
            }
            // NOMBRE TACHE EN PAUSE
            ViewData["NumTacheEnPause"] = securityService.GetListTacheByStatus("E06", usernom);
            // NOMBRE TACHE  TERMINER
            ViewData["NumTacheTerminer"] = securityService.GetListTacheByStatus("E01", usernom);
            // NOMBRE TACHE  TESTER
            ViewData["NumTacheATester"] = securityService.GetListTacheByStatus("E03", usernom);

            if (Session["UserRole"].ToString() != "03")
            {
                if (securityService.GetListTacheByDateExecution(usernom, "E04") > 0)
                {
                    TempData["TacheAValider"] = "Vous avez " + securityService.GetListTacheByDateExecution(usernom, "E04") + " taches à validées!";
                }

                if (securityService.GetListTacheCommercial(usernom, "C") > 0)
                {
                    TempData["TacheCommercial"] = "Vous avez des taches commercial à traiter (" + securityService.GetListTacheCommercial(usernom, "C") + ") !";
                }
            }
            int year = DateTime.Now.Year;
            var d1 = new DateTime(year, 1, 1);
            var d2 = new DateTime(year, 12, 31);

            ViewData["d1"] = d1;
            ViewData["d2"] = d2;
            return View(Model);
        }


        public ActionResult IndexClient()
        {
            if (Session["UserIDClient"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            SecurityServices securityService = new SecurityServices();


            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            //MessageBox.Show(Session["UserIDClient"].ToString());
            ContactClient contactClient = db.ContactClient.FirstOrDefault(s=>s.CodeClient == Session["UserIDClient"].ToString());
            List<Crm_ReclamationClient> model = securityService.GetAllListReclaationByClient(contactClient.id).ToList();

            return View(model);
        }

        public ActionResult Details(string id)
        {
            //MessageBox.Show(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;


            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            ViewData["ListChatReclamation"] = db.Crm_ChatReclamation.Where(s => s.Id_Reclamation == id).OrderByDescending(s => s.DateCreation).ToList();
            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }
    }
}
