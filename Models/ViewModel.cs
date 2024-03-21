using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMSTUBSOFT
{
    public class ViewModel
    {
     
        public Client Client { get; set; }
        public Crm_Sanction Crm_Sanction { get; set; }
        public Respensable Respensable { get; set; }
        public Crm_TypeTache TypeTache { get; set; }
        public crm_ModeTache ModeTache { get; set; }
        
    }
}