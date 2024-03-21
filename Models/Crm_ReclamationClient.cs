//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRMSTUBSOFT
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    public partial class Crm_ReclamationClient
    {
        public string Id_Reclamation { get; set; }
        [Required]
        public string Titre { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
        public System.DateTime DateReclamation { get; set; }
        public string CodeClient { get; set; }
        public string RaisonSociale { get; set; }
        public bool Soft { get; set; }
        public bool Technique { get; set; }
        public string NomContact { get; set; }
        public string CodeMoyenCommunication { get; set; }
        public string OutilCommunication { get; set; }
        [Required]
        public string Observation { get; set; }
        public string Createur { get; set; }
        public System.DateTime DateCreation { get; set; }
        public string logo { get; set; }
        public string Status { get; set; }
        public string NumeroDossier { get; set; }
        public string PrisEnChargePar { get; set; }
        public string DatePrisEnCharge { get; set; }
        public string ObjetReclamation { get; set; }
        public bool Paye { get; set; }
        public string TypeReclamation { get; set; }
        public bool Commercial { get; set; }
        public string UtilisateurContact { get; set; }
        public int Duree { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Nature { get; set; }
        public Nullable<int> IdNatureType { get; set; }
        public IEnumerable<Client> Listclient { get; set; }
        public IEnumerable<ContactClient> ListContactClient { get; set; }
        public IEnumerable<Respensable> ListeRespensables { get; set; }
        public IEnumerable<Crm_ModuleNature> ListModuleNature { get; set; }
        public IEnumerable<string> SelectedresponsableId { get; set; }
        public IEnumerable<Utilisateur> ListeUtilisateur { get; set; }
        public IEnumerable<string> SelectedUtilisateurId { get; set; }

    }
}