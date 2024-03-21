using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;

namespace CRMSTUBSOFT.Services.Data
{
    public class SecurityDAO
    {
        private CrmModelEntities db = new CrmModelEntities();
        public Boolean authResponse;
        internal string FindByUser(Utilisateur utilisateur, string check)
        {
            //Utilisateur utilisateurAuth = db.Utilisateur.Find(utilisateur.NomUtilisateur);
            if (check == "Client")
            {
                var utilisateurAuthPassword = db.ClientCRM.Where(s => s.MotPasse == utilisateur.MotDePasse && s.CodeClientCommercial == utilisateur.NomUtilisateur);

                var result = "errorauthConnectionClient";
                if (utilisateurAuthPassword.Any())
                {
                    result = "authConnectionClient";
                }

                return result;
            }
            else
            {
                //MessageBox.Show(utilisateur.NomUtilisateur);
                var utilisateurAuthPassword = db.Utilisateur.Where(s => s.MotDePasse == utilisateur.MotDePasse && s.NomUtilisateur == utilisateur.NomUtilisateur);

                var result = "errorauthConnection";
                if (utilisateurAuthPassword.Any())
                {
                    result = "authConnection";
                }

                return result;
            }

        }
        internal CRMSTUBSOFT.Utilisateur FindByUserName(Utilisateur utilisateur)
        {
            Utilisateur utilisateurAuth = db.Utilisateur.Find(utilisateur.NomUtilisateur);
            return (utilisateurAuth);

        }
        internal CRMSTUBSOFT.Utilisateur FindByUserPassword(Utilisateur utilisateur)
        {
            Utilisateur utilisateurIsAuth = new Utilisateur();
           var utilisateurAuth = db.Utilisateur.Where(s=>s.MotDePasse == utilisateur.MotDePasse);
            if(utilisateurAuth != null)
            {
               utilisateurIsAuth = db.Utilisateur.Find(utilisateur.NomUtilisateur);
            }
            return (utilisateurIsAuth);

        }
        internal CRMSTUBSOFT.Crm_AccesUtilisateur FindByUserRole(Utilisateur utilisateur)
        {
            Crm_AccesUtilisateur crm_AccesUtilisateur = db.Crm_AccesUtilisateur.Find(utilisateur.NomUtilisateur);
            if (crm_AccesUtilisateur is object)
            {
                authResponse = true;

            }
            else
            {
                authResponse = false;
            }
            return (crm_AccesUtilisateur);

        }
        internal CRMSTUBSOFT.Fonction FindByUserFonction(string codFonction)
        {
            Fonction fonction = db.Fonction.Find(codFonction);
            if (fonction is object)
            {
                authResponse = true;

            }
            else
            {
                authResponse = false;
            }
            return (fonction);

        }
        
        internal CRMSTUBSOFT.Respensable FindByUserResponsable(string codResponsable)
        {
            Respensable reponsable = db.Respensable.Find(codResponsable);
            //var reponsable = db.Respensable.First(s => s.CodeRespensable.Contains(codResponsable));
            return (reponsable);

        }
        internal CRMSTUBSOFT.Service FindByUserService(string codServices)
        {
            Service service = db.Service.Find(codServices);
            if (service is object)
            {
                authResponse = true;

            }
            else
            {
                authResponse = false;
            }
            return (service);

        }
        internal System.Linq.IQueryable<CRMSTUBSOFT.Crm_TacheReclamation> AllListTacheByStatus(string Status, string codeResponsable, Crm_Equipe ChefEquipe, Crm_AccesUtilisateur grade, List<string> ListeEquipe)
        {

            var model = db.Crm_TacheReclamation.Where(s => s.Status.Contains(Status));

            // si chef de projet : afficher les tache planifier par le chef de projet de lui et par les membres equipe eux même
            if (grade.CodeAcces == "02")
            {
                model = model.Where(s => ListeEquipe.Contains(s.NomPlanificateur) || s.NomPlanificateur == codeResponsable || s.NomPlanificateur == ChefEquipe.ChefEquipe);
            }


            // si memebre equipe : affiche les tache planifier par eux et par leur chef equipe
            if (grade.CodeAcces == "03")
            {
                model = model.Where(s => s.NomPlanificateur == codeResponsable || ListeEquipe.Contains(s.NomPlanificateur));
            }


            return model;

        }
        internal int ListTacheByStatus(string Status, string usernom)
        {
            var d1 = DateTime.Now.AddMonths(-1).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);
            var model = db.Crm_TacheReclamation.Where(s => s.Status.Contains(Status) && (s.UtilisateurCreateur.Contains(usernom)) ).Where(s => s.DateCreation >= d11 && s.DateCreation <= d22).Count();
            return (model);

        }
       
        internal System.Linq.IQueryable<string> GetIdByCompteur(string Table, string Year)
        {
            //var model = db.CompteurPiece.Where(s => s.PrefixPiece.Contains(Table));
            var model = from r in db.CompteurPiece
                        where r.PrefixPiece == Table && r.Annee == Year
                        select r.Compteur;
            return model;

        }


        internal CRMSTUBSOFT.ClientCRM FindByUserClient(Utilisateur utilisateur)
        {
         
            ClientCRM clientCRMAuth  = db.ClientCRM.First(s => s.CodeClientCommercial == utilisateur.NomUtilisateur);
            return (clientCRMAuth);

        }
        internal System.Linq.IQueryable<CRMSTUBSOFT.Crm_ReclamationClient> AllListReclamationByClient(int CodeClient)
        {
            var model = db.Crm_ReclamationClient.Where(s => s.NomContact == CodeClient);
            return model;

        }
        internal int ListTacheDateExecutions(string usernom, string Status)
        {
            DateTime Date = DateTime.Now.AddDays(-2);
            var model = db.Crm_TacheReclamation.Where(s => s.DateCreation == Date && s.UtilisateurCreateur.Contains(usernom) && s.Status.Contains(Status)).Count();
            return (model);

        }
        internal int ALLListTacheNoncommencer(string usernom, string Status, string coderesponsable)
        {
            var d1 = DateTime.Now.AddMonths(-1).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);

            var result =
            from a in db.Crm_TacheReclamation
            join b in db.Crm_TacheLiee on a.NumeroTache equals b.NumeroTache into ct
            from b in ct.DefaultIfEmpty()
            where ((a.UtilisateurCreateur == usernom || a.NomPlanificateur == coderesponsable || a.NomValidateur == coderesponsable || b.CodeRespensable == coderesponsable) && a.Status == Status)
            select new {
                      a.UtilisateurCreateur,
                      a.Status,
                      a.DateCreation,

            };
            var model = result.Where(s => s.DateCreation >= d11 && s.DateCreation <= d22).Count();
            return (model);

        }
        internal int ALLListTacheNoncommencer1(string usernom, string Status, string coderesponsable)
        {
               var result =
                       from a in db.Crm_TacheReclamation
                       join b in db.Crm_TacheLiee on a.NumeroTache equals b.NumeroTache into ct
                       from b in ct.DefaultIfEmpty()
                       where ((a.UtilisateurCreateur == usernom || b.CodeRespensable == coderesponsable) && a.Status == Status)
                       select new
                       {
                           a.UtilisateurCreateur,
                           a.Status
                       };
            var model = result.Count();

            return (model);

        }
        internal int ListTacheCommercial(string usernom, string Status)
        {

            var model = db.Crm_TacheReclamation.Where(s => s.UtilisateurCreateur.Contains(usernom) && s.Nature.Contains(Status)).Count();
            return (model);

        }
    }

}