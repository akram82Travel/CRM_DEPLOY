using CRMSTUBSOFT.Services.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRMSTUBSOFT.Services.Business
{
    public class SecurityServices
    {
        SecurityDAO daoServices = new SecurityDAO();

        public string Authenticate(Utilisateur utilisateur, string check)
        {
            return daoServices.FindByUser(utilisateur, check);
        }
        public CRMSTUBSOFT.Utilisateur AuthenticateName(Utilisateur utilisateur)
        {
            return daoServices.FindByUserName(utilisateur);
        }
        public CRMSTUBSOFT.Utilisateur AuthenticatePassword(Utilisateur utilisateur)
        {
            return daoServices.FindByUserPassword(utilisateur);
        }
        public CRMSTUBSOFT.Crm_AccesUtilisateur GetRole(Utilisateur utilisateur)
        {
            return daoServices.FindByUserRole(utilisateur);
        }
        public CRMSTUBSOFT.Fonction GetFonction(string codFonction)
        {
            return daoServices.FindByUserFonction(codFonction);
        }
        public CRMSTUBSOFT.Respensable GetResponsable(string codResponsable)
        {
            return daoServices.FindByUserResponsable(codResponsable);
        }
        public CRMSTUBSOFT.Service GetService(string codServices)
        {
            return daoServices.FindByUserService(codServices);
        }
        public System.Linq.IQueryable<CRMSTUBSOFT.Crm_TacheReclamation> GetAllListTacheByStatus(string Status, string codeResponsable, Crm_Equipe ChefEquipe, Crm_AccesUtilisateur grade, List<string> ListeEquipe)
        {
            return daoServices.AllListTacheByStatus(Status, codeResponsable, ChefEquipe, grade, ListeEquipe) ;
        }
        public int GetListTacheByStatus(string Status, string usernom)
        {
            return daoServices.ListTacheByStatus(Status, usernom);
        }
       
        public System.Linq.IQueryable<string> GetIdByCompteur(string Table, string Year)
        {
            return daoServices.GetIdByCompteur(Table,Year);
        }


        public CRMSTUBSOFT.ClientCRM AuthenticateClient(Utilisateur utilisateur)
        {
            return daoServices.FindByUserClient(utilisateur);
        }
        public System.Linq.IQueryable<CRMSTUBSOFT.Crm_ReclamationClient> GetAllListReclaationByClient(int CodeClient)
        {
            return daoServices.AllListReclamationByClient(CodeClient);
        }

        public int GetListTacheByDateExecution(string usernom, string Status)
        {
            return daoServices.ListTacheDateExecutions(usernom, Status);
        } public int GetListTacheCommercial(string usernom, string Status)
        {
            return daoServices.ListTacheCommercial(usernom, Status);
        }
        public int GetALLListTacheNoncommencer(string usernom, string Status, string coderespensable)
        {
            return daoServices.ALLListTacheNoncommencer(usernom, Status, coderespensable);
        }
        public int GetALLListTacheNoncommencer1(string usernom, string Status, string coderespensable)
        {
            return daoServices.ALLListTacheNoncommencer1(usernom, Status, coderespensable);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}