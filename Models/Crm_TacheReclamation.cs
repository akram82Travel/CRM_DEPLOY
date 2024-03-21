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
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    public partial class Crm_TacheReclamation
    {
        public string NumeroTache { get; set; }
        public int NumeroOrdre { get; set; }
        public string NumDossier { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string TacheTitre { get; set; }
        [Required(AllowEmptyStrings = true)]
        [AllowHtml] 
        public string DescriptionTache { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Nature { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Type { get; set; }
        public int Degres { get; set; }
        public string NomPlanificateur { get; set; }
        public string NomValidateur { get; set; }
        public string NomTesteur { get; set; }
        public string CodeRespensable { get; set; }
        [Required (AllowEmptyStrings = true)]
        public string Duree { get; set; }
        public decimal SalaireNet { get; set; }
        public decimal Coef { get; set; }
        public decimal MontantCharge { get; set; }
        public string NumeroEtat { get; set; }
        public string Status { get; set; }
        public string EtatValidation { get; set; }
        public System.DateTime DateCreation { get; set; }
        public System.DateTime DateCreationFin { get; set; }
        public string UtilisateurCreateur { get; set; }
        public bool Paye { get; set; }
        public string TypePiece { get; set; }
        public string NumPiece { get; set; }
        public string ValiderPar { get; set; }
        public bool AExpliquer { get; set; }
        public int IndicateurACorriger { get; set; }
        public int IndicateurAValider { get; set; }
        public int IndicateurAExpliquer { get; set; }
        public string CodeClient { get; set; }
        public string RaisonSociale { get; set; }
        public string AnuulerPar { get; set; }
        public string DateAnnulation { get; set; }
        public string ConfirmerPar { get; set; }
        public string DateConfirmation { get; set; }
        public string NonConfirmerPar { get; set; }
        public string DateNonConfirmation { get; set; }
        public System.DateTime DatePrevus { get; set; }
        public string DateDebutExecution { get; set; }
        public string DateFinExecution { get; set; }
        public int DurerExecution { get; set; }
        public string DurerExcutionValider { get; set; }
        public int DureeReel { get; set; }
        public string NumReclamation { get; set; }
        public IEnumerable<Respensable> ListeRespensables { get; set; }

        public IEnumerable<Crm_ReclamationClient> ListeCrm_ReclamationClient { get; set; }
        public IEnumerable<Crm_Rapport> ListeRapport { get; set; }
        public IEnumerable<string> SelectedresponsableId { get; set; }
    }
}