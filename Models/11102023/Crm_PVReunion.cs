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

    public partial class Crm_PVReunion
    {
        public string NumeroPVReunion { get; set; }
        public string Projet { get; set; }
        public string Module { get; set; }
        public string Createur { get; set; }
        public string Objet { get; set; }
        public System.DateTime DateReunion { get; set; }
        public System.DateTime DatePrevusReunion { get; set; }
        public string CodeClient { get; set; }
        public string CodePersonne { get; set; }
        [AllowHtml]
        public string DescriptionGeneral { get; set; }
        public Nullable<System.TimeSpan> Dure { get; set; }
    }
}