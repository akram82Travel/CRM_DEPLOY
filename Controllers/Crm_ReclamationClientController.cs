using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using CRMSTUBSOFT;
using CRMSTUBSOFT.Services.Business;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using MailKit.Security;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Office.Interop.Outlook;
using System.Xml;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using Exception = System.Exception;
using System.Security.Policy;
using System.Text;
//using System.Web.Mail;

namespace CRMSTUBSOFT.Controllers
{
    public class Crm_ReclamationClientController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();
        public async Task<ActionResult> RetrieveEmailAndStoreInDatabaseAsync()
        {
            string jsonUrl = "https://mail.mas-alu.com/home/recadmin/inbox?fmt=json&query=is:unread";
            string ipAddress = "192.168.10.191"; // Replace with the IP address of your email server
            int port = 443; // Replace with the appropriate port if needed
            //string jsonUrl = $"https://{ipAddress}:{port}/home/recadmin/inbox?fmt=json&query=is:unread"; // Construct the URL

            string username = "recadmin@mas-alu.com"; // Replace with your mailbox username
            string password = "REC12345"; // Replace with your mailbox password


            //string username = "hakram@mas-alu.com"; // Replace with your mailbox username
            //string password = "Mas@2022"; // Replace with your mailbox password


            List<MailItem> mailItems = new List<MailItem>();

            // Create HttpClient with Basic Authentication
            using (HttpClient httpClient = new HttpClient())
            {
                var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Send GET request and get the response
                HttpResponseMessage response = await httpClient.GetAsync(jsonUrl);
                response.EnsureSuccessStatusCode();

                // Read the JSON content
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Deserialize JSON using Newtonsoft.Json or System.Text.Json
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                if (rootObject != null)
                {
                    foreach (var mail in rootObject.m.Where(m => m.e.Any(e => e.a == "support@mas-alu.com" || e.a == "Support@mas-alu.com")))
                    {
                        
                        string id = mail.id;
                        string title = mail.su;
                        long timestamp = mail.d;
                        // Convert the timestamp to a DateTime object
                        DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

                        // Format the date as dd/MM/yyyy
                        string dateString = date.ToString("dd/MM/yyyy");
                        string description = mail.fr;
                        string author = string.Join(", ", mail.e.Select(e => e.a));
                        Crm_ReclamationClient isUsed = db.Crm_ReclamationClient.FirstOrDefault(s=>s.OutilCommunication==id);
                        if (isUsed == null)
                        {
                            if (!string.IsNullOrEmpty(title) && date > DateTime.Now.AddDays(-15) && !title.Contains("RE:") && !title.Contains("FW:") && !title.Contains("Fwd:") && !title.Contains("TR:") && !title.Contains("Re:") && !title.Contains("Tr:") && !title.Contains("Tr :"))
                            {

                                mailItems.Add(new MailItem
                                {
                                    Id = id,
                                    Title = title,
                                    Date = dateString,
                                    Description = description,
                                    Author = author
                                });
                            }
                        }
                    }
                }
            }

            return Json(new { success = true, data = mailItems });
        }
        public async Task<ActionResult> RetrieveEmailAndStoreInDatabaseAsyncEnCours()
        {
            string jsonUrl = "https://mail.mas-alu.com/home/recadmin/inbox?fmt=json&query=is:unread";
            string ipAddress = "192.168.10.191"; // Replace with the IP address of your email server
            int port = 443; // Replace with the appropriate port if needed
            //string jsonUrl = $"https://{ipAddress}:{port}/home/recadmin/inbox?fmt=json&query=is:unread"; // Construct the URL

            string username = "recadmin@mas-alu.com"; // Replace with your mailbox username
            string password = "REC12345"; // Replace with your mailbox password


            //string username = "hakram@mas-alu.com"; // Replace with your mailbox username
            //string password = "Mas@2022"; // Replace with your mailbox password


            List<MailItem> mailItems = new List<MailItem>();

            // Create HttpClient with Basic Authentication
            using (HttpClient httpClient = new HttpClient())
            {
                var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Send GET request and get the response
                HttpResponseMessage response = await httpClient.GetAsync(jsonUrl);
                response.EnsureSuccessStatusCode();

                // Read the JSON content
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Deserialize JSON using Newtonsoft.Json or System.Text.Json
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                if (rootObject != null)
                {
                    foreach (var mail in rootObject.m.Where(m => m.e.Any(e => e.a == "support@mas-alu.com" || e.a == "Support@mas-alu.com")))
                    {

                        string id = mail.id;
                        string title = mail.su;
                        long timestamp = mail.d;
                        // Convert the timestamp to a DateTime object
                        DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

                        // Format the date as dd/MM/yyyy
                        string dateString = date.ToString("dd/MM/yyyy");
                        string description = mail.fr;
                        string author = string.Join(", ", mail.e.Select(e => e.a));
                        Crm_ReclamationClient isUsed = db.Crm_ReclamationClient.FirstOrDefault(s => s.OutilCommunication == id);
                        if (isUsed != null)
                        {
                            if (!string.IsNullOrEmpty(title) && date > DateTime.Now.AddDays(-30) && !title.Contains("RE:") && !title.Contains("FW:") && !title.Contains("Fwd:") && !title.Contains("TR:") && !title.Contains("Re:") && !title.Contains("Tr:") && !title.Contains("Tr :"))
                            {

                                mailItems.Add(new MailItem
                                {
                                    Id = id,
                                    Title = title,
                                    Date = dateString,
                                    Description = description,
                                    Author = author
                                });
                            }
                        }






                    }
                }
            }

            return Json(new { success = true, data = mailItems });
        }
        public async Task<ActionResult> RetrieveEmailAndStoreInDatabaseAsyncTraiter()
        {
            string jsonUrl = "https://mail.mas-alu.com/home/recadmin/inbox?fmt=json&query=is:unread";
            string ipAddress = "192.168.10.191"; // Replace with the IP address of your email server
            int port = 443; // Replace with the appropriate port if needed
            //string jsonUrl = $"https://{ipAddress}:{port}/home/recadmin/inbox?fmt=json&query=is:unread"; // Construct the URL

            string username = "recadmin@mas-alu.com"; // Replace with your mailbox username
            string password = "REC12345"; // Replace with your mailbox password


            //string username = "hakram@mas-alu.com"; // Replace with your mailbox username
            //string password = "Mas@2022"; // Replace with your mailbox password


            List<MailItem> mailItems = new List<MailItem>();

            // Create HttpClient with Basic Authentication
            using (HttpClient httpClient = new HttpClient())
            {
                var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Send GET request and get the response
                HttpResponseMessage response = await httpClient.GetAsync(jsonUrl);
                response.EnsureSuccessStatusCode();

                // Read the JSON content
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Deserialize JSON using Newtonsoft.Json or System.Text.Json
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                if (rootObject != null)
                {
                    foreach (var mail in rootObject.m.Where(m => m.e.Any(e => e.a == "support@mas-alu.com" || e.a == "Support@mas-alu.com")))
                    {

                        string id = mail.id;
                        string title = mail.su;
                        long timestamp = mail.d;
                        // Convert the timestamp to a DateTime object
                        DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

                        // Format the date as dd/MM/yyyy
                        string dateString = date.ToString("dd/MM/yyyy");
                        string description = mail.fr;
                        string author = string.Join(", ", mail.e.Select(e => e.a));
                        Crm_ReclamationClient isUsed = db.Crm_ReclamationClient.FirstOrDefault(s => s.OutilCommunication == id);
                        if (isUsed != null && isUsed.Status == "E01")
                        {
                            if (!string.IsNullOrEmpty(title) && date > DateTime.Now.AddDays(-30) && !title.Contains("RE:") && !title.Contains("FW:") && !title.Contains("Fwd:") && !title.Contains("TR:") && !title.Contains("Re:") && !title.Contains("Tr:") && !title.Contains("Tr :"))
                            {

                                mailItems.Add(new MailItem
                                {
                                    Id = id,
                                    Title = title,
                                    Date = dateString,
                                    Description = description,
                                    Author = author
                                });
                            }
                        }






                    }
                }
            }

            return Json(new { success = true, data = mailItems });
        }
        public async Task<ActionResult> RetrieveEmailById()
        {
            var IdEmail = Request["IDEmail"];
            string ipAddress = "192.168.10.191"; // Replace with the IP address of your email server
            int port = 443; // Replace with the appropriate port if needed

            Crm_ReclamationClient crmReclamation = db.Crm_ReclamationClient.FirstOrDefault(s=>s.OutilCommunication == IdEmail);
                if (crmReclamation == null)
            {
                string jsonUrl = "https://mail.mas-alu.com/home/recadmin/inbox?id=" + IdEmail + "&fmt=json";
                string jsonUrlAttachement = "https://mail.mas-alu.com/home/recadmin/inbox?id=" + IdEmail + "&part=2"; // Construct the URL
                
                string jsonUrlBody = "https://mail.mas-alu.com/home/recadmin/inbox.json?id=" + IdEmail; // Construct the URL
                string jsonUrlBody2 = "https://mail.mas-alu.com/home/recadmin/inbox?id=" + IdEmail + "&part=2.1";

                string username = "recadmin@mas-alu.com"; // Replace with your mailbox username
                string password = "REC12345"; // Replace with your mailbox password
                
                string emailBody = "";
                string description = "";
                string sender = "";
                List<MailItem> mailItems = new List<MailItem>();
                
                // Get RetrieveEmailById
                using (HttpClient httpClient = new HttpClient())
                {
                    var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                    
                    HttpResponseMessage response = await httpClient.GetAsync(jsonUrl);
                    response.EnsureSuccessStatusCode();

                    // Read the JSON content
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON using Newtonsoft.Json or System.Text.Json
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                    if (rootObject != null)
                    {
                        foreach (var mail in rootObject.m)
                        {
                            string title = mail.su;
                            long timestamp = mail.d;
                            // Convert the timestamp to a DateTime object
                            DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

                            // Format the date as dd/MM/yyyy
                            string dateString = date.ToString("dd/MM/yyyy");
                            description = mail.fr;
                            string author = string.Join(", ", mail.e.Select(e => e.a));

                            if (!string.IsNullOrEmpty(title))
                            {
                                sender = author;
                                mailItems.Add(new MailItem
                                {
                                    Title = title,
                                    Id = IdEmail,
                                    Date = dateString,
                                    Description = description,
                                    Author = author
                                });
                            }
                        }
                    }



                }
                // get email body
                using (HttpClient httpClient = new HttpClient())
                {
                    var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    // Send GET request and get the response
                    HttpResponseMessage response = await httpClient.GetAsync(jsonUrlBody);
                    HttpResponseMessage responseBody = await httpClient.GetAsync(jsonUrlBody2);
                    response.EnsureSuccessStatusCode();

                    // Read the JSON content
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    string jsonContent2 = await responseBody.Content.ReadAsStringAsync();
                    if (jsonContent != null)
                    {
                        string emailSource = jsonContent.ToString(); // Paste the provided email source here

                        // Use a regular expression to extract the email body text from the source
                        string pattern = @"Content-Type: text/(html|plain);.*?(?<body>[\s\S]+?)--=_";
                        Match match = Regex.Match(emailSource, pattern);

                        if (match.Success)
                        {
                            string contentType = match.Groups[1].Value.ToLower();
                            emailBody = match.Groups["body"].Value;
                            //MessageBox.Show(emailBody);
                            if (contentType == "html")
                            {
                                // If the content type is HTML, you may want to further process the HTML content
                                // For example, you can use a library like HtmlAgilityPack to parse and manipulate the HTML
                                // Here, I'll just replace some common encoded characters as an example
                                emailBody = WebUtility.HtmlDecode(emailBody);
                            }
                            else
                            {
                                // If the content type is plain text, you can directly use the email body
                                // Replace any special characters if needed
                                // If the content type is plain text, consider the charset
                                string charsetPattern = @"charset=""(.*?)""";
                                Match charsetMatch = Regex.Match(emailSource, charsetPattern);

                                if (charsetMatch.Success)
                                {
                                    string charset = charsetMatch.Groups[1].Value;

                                    // Decode the content using the specified charset
                                    Encoding encoding = Encoding.GetEncoding(charset);
                                    emailBody = encoding.GetString(Encoding.Default.GetBytes(emailBody));
                                }
                            }

                        }
                        else
                        {
                            emailBody = "Email body not found in the provided source.";
                        }
                    }
                    if (responseBody.IsSuccessStatusCode)
                    {
                        if (responseBody.Content.Headers.ContentLength > 0)
                        {
                            byte[] emailBodyOrigin = await responseBody.Content.ReadAsByteArrayAsync();
                            emailBody = Encoding.UTF8.GetString(emailBodyOrigin);
                        }

                    }

                }
                // get attachement files
                using (HttpClient httpClient = new HttpClient())
                {
                    var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    //try
                    //{
                    for (var i = 2; i <= 12; i++)
                    {
                        //jsonUrlAttachement = "https://mail.mas-alu.com/home/recadmin/inbox?id=" + IdEmail + "&part=" + i + "." + i;
                        string jsonUrlAttachement2 = "https://mail.mas-alu.com/home/recadmin/inbox?id=" + IdEmail + "&part=" + i;
                        //try
                        //{
                        //    HttpResponseMessage response = await httpClient.GetAsync(jsonUrlAttachement);
                        //    if (response.IsSuccessStatusCode)
                        //    {
                        //        if (response.Content.Headers.ContentLength > 0)
                        //        {
                        //            //MessageBox.Show("Longeur reponse: " + response.Content.Headers.ContentLength);
                        //            byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
                        //            string contentType = response.Content.Headers.ContentType.ToString();
                        //            string fileName = "";
                        //            string fileExtension = contentType.Split('/')[1];

                        //            fileExtension = fileExtension.Split(';')[0];

                        //            if (fileExtension != "html" && fileExtension != "related")
                        //            {
                        //                string imageName = "_";
                        //                fileName = $"{IdEmail}.{imageName}.{fileExtension}";
                        //            }


                        //            string folderPath = @"D:\ReclamationJoint\";

                        //            if (!Directory.Exists(folderPath))
                        //            {
                        //                Directory.CreateDirectory(folderPath);
                        //            }

                        //            if (fileName != "")
                        //            {
                        //                string fullFilePath = Path.Combine(folderPath, fileName);
                        //                System.IO.File.WriteAllBytes(fullFilePath, fileContent);
                        //            }

                        //        }
                        //        else
                        //        {
                        //            Console.WriteLine("Insufficient content written. No attachment found.");
                        //        }
                        //    }
                        //    // Use response2 as needed
                        //}
                        //catch (HttpRequestException ex)
                        //{
                        //    // Handle the exception
                        //    // For example, you can log the error, show a message to the user, or retry the request
                        //}
                        try
                        {
                            HttpResponseMessage response2 = await httpClient.GetAsync(jsonUrlAttachement2);
                            // Use response2 as needed
                            if (response2.IsSuccessStatusCode)
                            {
                                if (response2.Content.Headers.ContentLength > 0)
                                {
                                    //MessageBox.Show("Longeur reponse: " + response.Content.Headers.ContentLength);
                                    byte[] fileContent = await response2.Content.ReadAsByteArrayAsync();
                                    string contentType = response2.Content.Headers.ContentType.ToString();
                                    string fileName = "";
                                    string fileExtension = contentType.Split('/')[1];

                                    fileExtension = fileExtension.Split(';')[0];
                                    if (fileExtension.Contains("sheet"))
                                    {
                                        fileExtension = "xlsx";

                                    }
                                    if (fileExtension.Contains("plain"))
                                    {
                                        fileExtension = "txt";

                                    }
                                    if (fileExtension != "html" && fileExtension != "related")
                                    {
                                        string imageName = "_";
                                        fileName = $"{IdEmail}.{imageName}.{fileExtension}";
                                    }


                                    string folderPath = @"C:\inetpub\wwwroot\ReclamationJoint\";

                                    if (!Directory.Exists(folderPath))
                                    {
                                        Directory.CreateDirectory(folderPath);
                                    }

                                    if (fileName != "")
                                    {
                                        string fullFilePath = Path.Combine(folderPath, fileName);
                                        System.IO.File.WriteAllBytes(fullFilePath, fileContent);
                                    }
                                   
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient content written. No attachment found.");
                                }
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            // Handle the exception
                            // For example, you can log the error, show a message to the user, or retry the request
                        }
                    }
                   
                    return Json(new { success = true, data = mailItems, body = emailBody });
                    //}
                    //catch (HttpRequestException ex)
                    //{
                    //    return Json(new { success = true, data = mailItems, body = emailBody });
                    //}
                }
                
            }
            else
            {
                return Json(new { success = false, data = "", body = "" });
            }
        }

        public ActionResult Email()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["UserNom"].ToString().Trim() + @Session["UserPrenom"].ToString().Trim());
            ViewData["ListResponsable"] = ListResponsable;

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("REC", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;
            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();
            crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur.ToString("00000");

            int pageSize = 200; // Nombre d'éléments par page
            int pageNumber = 1; // Numéro de la page souhaitée
            var ListEmailWithReclamation = db.Crm_ReclamationClient
                .Where(x => x.OutilCommunication != "")
                .OrderByDescending(x => x.Id_Reclamation) // Remplacez SomeProperty par le champ que vous souhaitez trier
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewData["ListEmailWithReclamation"] = ListEmailWithReclamation;

            return View(crm_ReclamationClient);
        }
        public ActionResult EmailEnCours()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["UserNom"].ToString().Trim() + @Session["UserPrenom"].ToString().Trim());
            ViewData["ListResponsable"] = ListResponsable;

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("REC", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;
            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();
            crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur.ToString("00000");

            int pageSize = 200; // Nombre d'éléments par page
            int pageNumber = 1; // Numéro de la page souhaitée
            var ListEmailWithReclamation = db.Crm_ReclamationClient
                .Where(x => x.OutilCommunication != "")
                .OrderByDescending(x => x.Id_Reclamation) // Remplacez SomeProperty par le champ que vous souhaitez trier
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewData["ListEmailWithReclamation"] = ListEmailWithReclamation;
            return View(crm_ReclamationClient);
        }
        public ActionResult EmailTraiter()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var ListResponsable = new SelectList(db.Respensable.ToList(), "CodeRespensable", "Nom", @Session["UserNom"].ToString().Trim() + @Session["UserPrenom"].ToString().Trim());
            ViewData["ListResponsable"] = ListResponsable;

            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;
            var ListProjet = new SelectList(db.Crm_Projet.ToList(), "CodeProjet", "Libelle");
            ViewData["ListProjet"] = ListProjet;
            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;

            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;
            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            /*Primary key */
            SecurityServices securityService = new SecurityServices();
            var CompteurId = securityService.GetIdByCompteur("REC", "210").ToList();
            foreach (var item in CompteurId)
            {
                ViewData["Compteur"] = item;
            }
            decimal valeur;
            valeur = Convert.ToDecimal(ViewData["Compteur"]) + 1;
            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();
            crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur.ToString("00000");

            int pageSize = 200; // Nombre d'éléments par page
            int pageNumber = 1; // Numéro de la page souhaitée
            var ListEmailWithReclamation = db.Crm_ReclamationClient
                .Where(x => x.OutilCommunication != "")
                .OrderByDescending(x => x.Id_Reclamation) // Remplacez SomeProperty par le champ que vous souhaitez trier
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewData["ListEmailWithReclamation"] = ListEmailWithReclamation;

            return View(crm_ReclamationClient);
        }
        // Define a class to store the mail item data
        private class MailItem
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Author { get; set; }
            public string Date { get; set; }
        }
        // GET: Crm_ReclamationClient/index/d1/d2
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
            return View(db.Crm_ReclamationClient.ToList());
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

        // GET: Crm_ReclamationClient/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            if (id == null)
            {
                return RedirectToAction("Dashbord", "Crm_TacheReclamation", new { returnUrl = "" });
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
            ViewData["ListChatReclamation"] = db.Crm_ChatReclamation.Where(s=>s.Id_Reclamation == id).OrderByDescending(s=>s.DateCreation).ToList();

            string folderPath = @"C:\inetpub\wwwroot\ReclamationJoint";

            // Vérifier si le dossier existe
            if (Directory.Exists(folderPath))
            {
                List<FileInformation> fileList = new List<FileInformation>();

                // Obtenir la liste de tous les fichiers dans le dossier
                string[] files = Directory.GetFiles(folderPath);

                // Parcourir tous les fichiers
                foreach (string filePath in files)
                {
                    // Utiliser la classe FileInfo pour obtenir des informations sur le fichier
                    FileInfo fileInfo = new FileInfo(filePath);

                    // Ajouter les informations du fichier à la liste
                    fileList.Add(new FileInformation
                    {
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        FilePath = filePath // Ajouter l'emplacement complet du fichier
                        // Ajoutez d'autres propriétés au besoin
                    });
                }

                // Ajouter la liste des fichiers à ViewData
                ViewData["listPieceJointe"] = fileList;
            }
            else
            {
                ViewData["listPieceJointe"] = null;
            }



            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }
        // GET: Crm_ReclamationClient/Create
        public ActionResult Create()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;

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
            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();
            crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur.ToString("00000");
            return View(crm_ReclamationClient);
        }

        // POST: Crm_ReclamationClient/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Reclamation,Titre,Description,DateReclamation,CodeClient,RaisonSociale,NomContact,CodeMoyenCommunication,Observation,Createur,DateCreation,Nature, ObjetReclamation, UtilisateurContact, PrisEnChargePar, NumeroDossier, OutilCommunication")] Crm_ReclamationClient crm_ReclamationClient, IEnumerable<string> SelectedresponsableId, IEnumerable<string> ListeRespensables)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;

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
            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();
            crm_ReclamationClient.Id_Reclamation = "REC" + "210" + valeur.ToString("00000");
            if (ModelState.IsValid)
            {
                try
                {
                    // Your code to save entities here
                    string fileName = "";
                    if (Request.Files.AllKeys.Contains("Image"))
                    {
                        HttpPostedFileBase file = Request.Files["Image"];
                        /*Image/root/rename*/
                        Guid guid = Guid.NewGuid();
                        string newfileName = guid.ToString();
                        string fileextention = Path.GetExtension(file.FileName);
                        fileName = newfileName + fileextention;
                        string uploadpath = Path.Combine(Server.MapPath("C:\\inetpub\\wwwroot\\ReclamationJoint"), Path.GetFileName(fileName));
                        file.SaveAs(uploadpath);
                        /*End/Image/root/rename*/
                    }
                    /*Begin intialize data*/
                    if (crm_ReclamationClient.OutilCommunication == "")
                    {
                        crm_ReclamationClient.OutilCommunication = " ";
                    }
                    if (crm_ReclamationClient.UtilisateurContact == "")
                    {
                        crm_ReclamationClient.UtilisateurContact = " ";
                    }
                    if (crm_ReclamationClient.PrisEnChargePar == null)
                    {
                        crm_ReclamationClient.PrisEnChargePar = " ";
                    }
                    crm_ReclamationClient.TypeReclamation = " ";
                    crm_ReclamationClient.DatePrisEnCharge = " ";
                    
                    crm_ReclamationClient.Paye = false;
                    crm_ReclamationClient.logo = fileName;
                    crm_ReclamationClient.Status = "E05";
                    crm_ReclamationClient.Createur = Session["UserID"].ToString();
                    crm_ReclamationClient.DateCreation = DateTime.Now;
                    crm_ReclamationClient.DateReclamation = DateTime.Now;
                   
                    /*Initialize Data to avoid NULL Exception*/
                    if (crm_ReclamationClient.ObjetReclamation == null)
                    {
                        crm_ReclamationClient.ObjetReclamation = " ";
                    }
                    if (crm_ReclamationClient.Titre == null)
                    {
                        crm_ReclamationClient.Titre = " ";
                    }
                        
                    if (crm_ReclamationClient.Description == null)
                    {
                        crm_ReclamationClient.Description = " ";
                    }

                    if (crm_ReclamationClient.Observation == null)
                    {
                        crm_ReclamationClient.Observation = " ";
                    }
                    
                    if (SelectedresponsableId != null)
                    {
                        string listeResp = string.Join(",", SelectedresponsableId.ToArray());
                        crm_ReclamationClient.UtilisateurContact = listeResp;
                    }
                    else
                    {
                        crm_ReclamationClient.UtilisateurContact = " ";
                    }



                    if (ListeRespensables != null)
                    {
                        string ListeCc = string.Join(",", ListeRespensables.ToArray());
                         crm_ReclamationClient.NumeroDossier = ListeCc;
                    }
                    else
                    {
                        crm_ReclamationClient.NumeroDossier = " ";
                    }
                       
                        
                    

                    CompteurPiece compteurPiece = db.CompteurPiece.Find("SOC001", "Crm_ReclamationClient");
                    decimal valeur1 = Convert.ToDecimal(compteurPiece.Compteur); 
                    decimal valeur2 = Convert.ToDecimal(crm_ReclamationClient.Id_Reclamation.Substring(5));

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
                    db.Crm_ReclamationClient.Add(crm_ReclamationClient);
                    db.SaveChanges();
                    TempData["SuccessMesage"] = "Ticket N°" + @crm_ReclamationClient.Id_Reclamation + " est enregistrée! ";

                   


                    //send email
                    int idContact = int.Parse(crm_ReclamationClient.NomContact.ToString());
                    //get contact email 
                    ContactClient ContactClientInfos = db.ContactClient.Where(s => s.id == idContact).FirstOrDefault();
                   

                    //EmailCreate(ContactClientInfos.Mail, SelectedresponsableId, crm_ReclamationClient, ContactClientInfos);

                    return RedirectToAction("CreateTacheReclamation", "ContactClient", new { id = crm_ReclamationClient.Id_Reclamation });
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // Log or display the validation error details
                            MessageBox.Show($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }
                    // Handle the exception or take appropriate action
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var errorProperties = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    if (errors.Count > 0)
                    {
                        errorProperties.Add(key);
                        MessageBox.Show("Le champ "+key + " est Obligatoire");
                    }
                }
            }

            return View(crm_ReclamationClient);
        }
        public void EmailCreate(string To, IEnumerable<string> SelectedresponsableId, Crm_ReclamationClient crm_ReclamationClient, ContactClient ContactClientInfos)
        {
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("support@mas-alu.com", "Support MAS")
            };
            mailMessage.To.Add(To);


            if (SelectedresponsableId != null)
            {
                foreach (var item in SelectedresponsableId)
                {
                    // get utilsateurNom from utilisateur
                    Utilisateur ResponsableReclamation = db.Utilisateur.Where(s => s.CodeRepresentant == item).FirstOrDefault();
                    // get nom utilsateur pour récupére l'email de contactclient
                    ContactClient NomRespReclamation = db.ContactClient.Where(s => s.CodeClient == ResponsableReclamation.NomUtilisateur).FirstOrDefault();
                    mailMessage.CC.Add(NomRespReclamation.Mail);
                }

            }




            mailMessage.CC.Add("bkamel@mas-alu.com");
            mailMessage.CC.Add("achokri@mas-alu.com");
            mailMessage.CC.Add("ahedi@mas-alu.com");

            mailMessage.Subject = "Votre Ticket est bien reçue ! " + crm_ReclamationClient.Id_Reclamation + " " + crm_ReclamationClient.Titre;
            mailMessage.Body = "Cher/Chère " + ContactClientInfos.Contact;
            mailMessage.Body += "<br> Nous avons le plaisir de vous informer que votre Ticket a été créé avec succès avec le numéro " + crm_ReclamationClient.Id_Reclamation + " ! <br><br>Vous pouvez suivre son évolution en utilisant les liens ci-dessous :";
            
            mailMessage.Body += "<br><br> <a href='http://192.168.10.199/Authentification' target='blank'>1- Suivie Ticket (Accès Local)</a>";
            mailMessage.Body += "<br><br> <a href='http://196.179.228.36:5700/Authentification' target='blank'>2- Suivie Ticket (Accès Distant)</a>";
            
            mailMessage.Body += "<br><br> <b>Objet Ticket: </b>" + crm_ReclamationClient.Titre;
            mailMessage.Body += "<br><br> <b>Description: </b>" + crm_ReclamationClient.Description;
            mailMessage.Body += "<br><br> N'hésitez pas à consulter régulièrement ce lien pour être informé(e) de toutes les mises à jour concernant votre Ticket.";
            mailMessage.Body += "<br><br> Merci pour votre confiance. <br><br>Nous mettons tout en œuvre pour répondre à votre Ticket rapidement et efficacement.";
            mailMessage.Body += "<br><br><br> Cordialement,";
            mailMessage.Body += "<br><br><br> Group MAS";
            mailMessage.Body += "<br> Service Informatique";
            mailMessage.IsBodyHtml = true;
            new SmtpClient("192.168.10.191", 25).Send(mailMessage);
        }
        // GET: Crm_ReclamationClient/Edit/5
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

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            var ListeContactClient = new SelectList(db.ContactClient.ToList(), "id", "Contact");
            ViewData["ListeContactClient"] = ListeContactClient;


            var ListNature = new SelectList(db.Crm_TacheNature.ToList(), "Nature", "Libelle");
            ViewData["ListNature"] = ListNature;

            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }

        // POST: Crm_ReclamationClient/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Reclamation,Titre,Description,DateReclamation,CodeClient,RaisonSociale,NomContact,CodeMoyenCommunication,Observation,Createur,DateCreation,Nature")] Crm_ReclamationClient crm_ReclamationClient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase file = Request.Files["EditimageElement"];
                    //var path = Path.Combine(Server.MapPath("~/UploadedFiles"), crm_ReclamationClient.logo);
                    if (crm_ReclamationClient.logo != null)
                    {
                        var path = Path.Combine(Server.MapPath("C:\\inetpub\\wwwroot\\ReclamationJoint"), crm_ReclamationClient.logo);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    //ADD NEW file from the file system
                    string uploadpath = "";
                    Guid guid = Guid.NewGuid();
                    string newfileName = guid.ToString();
                    string fileextention = Path.GetExtension(file.FileName);
                    string fileName = newfileName + fileextention;
                    if (Request.Files["logo"] != null && Request.Files["logo"].ContentLength > 0)
                    {
                        fileName = Request.Files["logo"].ToString();
                    }
                    else
                    {
                        uploadpath = Path.Combine(Server.MapPath("C:\\inetpub\\wwwroot\\ReclamationJoint"), Path.GetFileName(fileName));
                        file.SaveAs(uploadpath);
                    }
                    /*Begin intialize data*/
                    crm_ReclamationClient.OutilCommunication = " ";
                    crm_ReclamationClient.TypeReclamation = " ";
                    crm_ReclamationClient.PrisEnChargePar = " ";
                    crm_ReclamationClient.DatePrisEnCharge = DateTime.Now.ToString();
                    crm_ReclamationClient.ObjetReclamation = " ";
                    crm_ReclamationClient.NumeroDossier = " ";
                    crm_ReclamationClient.Paye = false;
                    crm_ReclamationClient.logo = fileName;
                    crm_ReclamationClient.Status = "E05";
                    crm_ReclamationClient.Createur = Session["UserID"].ToString();
                    crm_ReclamationClient.DateCreation = DateTime.Now;
                    crm_ReclamationClient.UtilisateurContact = " ";
                    /*End intialize data*/
                    /*Initialize Data to avoid NULL Exception*/
                    if (crm_ReclamationClient.Titre == null)
                        crm_ReclamationClient.Titre = " ";
                    if (crm_ReclamationClient.Description == null)
                        crm_ReclamationClient.Description = " ";
                    if (crm_ReclamationClient.Observation == null)
                        crm_ReclamationClient.Observation = " ";
                    if (crm_ReclamationClient.NomContact == 0)
                        crm_ReclamationClient.NomContact = 0;
                    if (crm_ReclamationClient.CodeMoyenCommunication == null)
                        crm_ReclamationClient.CodeMoyenCommunication = " ";
                    db.Entry(crm_ReclamationClient).State = EntityState.Modified;
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
                TempData["SuccessMesage"] = "Ticket N°" + @crm_ReclamationClient.Id_Reclamation + " est modifiée! ";
                return RedirectToAction("Index");
            }
            return View(crm_ReclamationClient);
        }

        // GET: Crm_ReclamationClient/Delete/5
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
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            if (crm_ReclamationClient == null)
            {
                return HttpNotFound();
            }
            return View(crm_ReclamationClient);
        }

        // POST: Crm_ReclamationClient/Delete/5
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            Crm_ReclamationClient crm_ReclamationClient = db.Crm_ReclamationClient.Find(id);
            db.Crm_ReclamationClient.Remove(crm_ReclamationClient);
            db.SaveChanges();
            TempData["SuppressionMessage"] = "Ticket N°" + @crm_ReclamationClient.Id_Reclamation + " est supprimée! ";
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
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);
            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var model = from r in db.Crm_ReclamationClient
                        where r.Status == Status && r.DateCreation > d11 && r.DateCreation < d22
                        select r;
            return View("Index", model);
        }
        public ActionResult SearchDate(string d1, string d2)
        {
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);
            ViewData["d1"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var model = from r in db.Crm_ReclamationClient
                        where r.DateCreation > d11 && r.DateCreation < d22
                        select r;
            return View("Index", model);
        }
        // GET: Crm_ReclamationClient/Create
        public ActionResult Reclamation()
        {
            if (Session["UserIDClient"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            string IdClient = Session["UserIDClient"].ToString();
            var contratclient = db.ContratClient.Where(s => s.CodeClient == IdClient && s.Actif == true);
            if (contratclient.Any())
            {
                TempData["ContratMsgActif"] = "Merci d'être notre précieux client. Nous sommes très reconnaissants du plaisir de vous servir et espérons avoir répondu à vos attentes!";
            }
            else
            {
                TempData["ContratMsginActif"] = "GROUP MAS vous remercions de nous avoir choisis. En raison d'être parmi nos clients fidèles, vous devez attribuer un contrat pour valider votre Ticket a priori !";
            }

            Crm_ReclamationClient crm_ReclamationClient = new Crm_ReclamationClient();
            //DropdownList
            var ListClients = new SelectList(db.Client.ToList(), "CodeClient", "RaisonSociale");
            ViewData["ListClient"] = ListClients;

            var ListeMoyenCommunication = new SelectList(db.Crm_MoyenCommunication.ToList(), "CodeMoyenCommunication", "Libelle");
            ViewData["ListeMoyenCommunication"] = ListeMoyenCommunication;

            crm_ReclamationClient.ListContactClient = new List<ContactClient>();
            crm_ReclamationClient.ListContactClient = ContactClientData();

            if (crm_ReclamationClient.ListContactClient.Count() > 0)
            {
                ViewData["Empty"] = "False";
            }
            else
            {
                ViewData["Empty"] = "True";
            }
            crm_ReclamationClient.ListModuleNature = new List<Crm_ModuleNature>();
            crm_ReclamationClient.ListModuleNature = ModuleNatureData();

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

        // POST: Crm_ReclamationClient/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reclamation([Bind(Include = "Id_Reclamation,Titre,Description,DateReclamation,CodeClient,RaisonSociale,NomContact,CodeMoyenCommunication,Observation,Createur,DateCreation, Nature, IdNatureType")] Crm_ReclamationClient crm_ReclamationClient, string Newnomcontact)
        {
            if (ModelState.IsValid)
            {
                /*Begin intialize data*/
                crm_ReclamationClient.OutilCommunication = "";
                crm_ReclamationClient.TypeReclamation = "";
                crm_ReclamationClient.PrisEnChargePar = "";
                crm_ReclamationClient.DatePrisEnCharge = "";
                crm_ReclamationClient.ObjetReclamation = "";
                crm_ReclamationClient.NumeroDossier = "";
                crm_ReclamationClient.Paye = false;
                crm_ReclamationClient.logo = "";
                crm_ReclamationClient.Status = "E05";
                crm_ReclamationClient.Createur = Session["UserIDClient"].ToString();
                crm_ReclamationClient.DateCreation = DateTime.Now;
                crm_ReclamationClient.UtilisateurContact = "";
                //crm_ReclamationClient.CodeMoyenCommunication = "";
                crm_ReclamationClient.CodeClient = Session["UserIDClient"].ToString();
                crm_ReclamationClient.RaisonSociale = Session["UserLogin"].ToString();
                if (Newnomcontact != "")
                    crm_ReclamationClient.NomContact = int.Parse(Newnomcontact);
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
                db.Crm_ReclamationClient.Add(crm_ReclamationClient);
                db.SaveChanges();
                TempData["SuccessMesage"] = "Ticket N°" + @crm_ReclamationClient.Id_Reclamation + " est enregistrée! ";
                return RedirectToAction("Success");
            }

            return View(crm_ReclamationClient);
        }

        // Action for downloading a file
        public ActionResult Download(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }
            // Read the file contents into a byte array
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            // Set the content type and file name
            string contentType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
            string fileName = Path.GetFileName(filePath);
            // Return the file as a content result
            return File(fileBytes, contentType, fileName);
        }

        //create chat reclamation
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ChatReclalamtion([Bind(Include = "Id_Reclamation,CodeClient,Description,Createur")] Crm_ChatReclamation chatReclamation, string Status)
        {
            chatReclamation.RaisonSociale = "";
            chatReclamation.DateCreation = DateTime.Now;
            db.Crm_ChatReclamation.Add(chatReclamation);
            db.SaveChanges();
            Crm_ReclamationClient ReclamationClient = db.Crm_ReclamationClient.FirstOrDefault(a => a.Id_Reclamation == chatReclamation.Id_Reclamation);
            int id_contact = ReclamationClient.NomContact;
            ContactClient contactClients = db.ContactClient.FirstOrDefault(a => a.id == id_contact);
            if (Status == "E01")
            {
                ReclamationClient.Status = "E01";
            }
            else
            {
                ReclamationClient.Status = "E07";
            }
            ReclamationClient.DatePrisEnCharge = DateTime.Now.ToString();
            db.Entry(ReclamationClient).State = EntityState.Modified;
            db.SaveChanges();
            //send email to destination
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("support@mas-alu.com", "Support MAS")
            };
            mailMessage.To.Add(contactClients.Mail);

           
            if (ReclamationClient.NumeroDossier != "" && ReclamationClient.NumeroDossier != " " && ReclamationClient.NumeroDossier != "10")
            {
                foreach (var item in ReclamationClient.NumeroDossier.Split(','))
                {
                    // get utilsateurNom from utilisateur
                    Utilisateur ResponsableReclamation = db.Utilisateur.Where(s => s.CodeRepresentant == item).FirstOrDefault();
                    // get nom utilsateur pour récupére l'email de contactclient
                    ContactClient NomRespReclamation = db.ContactClient.Where(s => s.CodeClient == ResponsableReclamation.NomUtilisateur).FirstOrDefault();
                    mailMessage.CC.Add(NomRespReclamation.Mail);
                }

            }

            mailMessage.CC.Add("bkamel@mas-alu.com");
            mailMessage.CC.Add("achokri@mas-alu.com");
            mailMessage.CC.Add("ahedi@mas-alu.com");
            if (ReclamationClient.Status == "E01")
            {
                mailMessage.Subject = "Clôture Ticket N° " + chatReclamation.Id_Reclamation + " " + ReclamationClient.Titre + " !";
                mailMessage.Body = "Cher/Chère " + contactClients.Contact;
                mailMessage.Body += "<br> Nous avons le plaisir de vous informer que votre Ticket est actuellement considérée comme résolue !";
                mailMessage.Body += "<br><br> N° Ticket: " + chatReclamation.Id_Reclamation;
                mailMessage.Body += "<br><br> <b>Objet Ticket:</b> " + ReclamationClient.Titre;
                mailMessage.Body += "<br><br> <b>Description: </b>" + ReclamationClient.Description;

                if (chatReclamation.Description != "")
                {
                    mailMessage.Body += "<br><br> Pour plus de détails voici la réponse du responsable";
                    mailMessage.Body += chatReclamation.Description;
                }
                mailMessage.Body += "<br><br> Merci pour votre collaboration.";
                mailMessage.Body += "<br><br><br> Cordialement,";
                mailMessage.Body += "<br><br><br> Group MAS";
                mailMessage.Body += "<br> Service Informatique";
            }
            else
            {
                mailMessage.Subject = "Réponse Ticket N°:" + chatReclamation.Id_Reclamation + " " + ReclamationClient.Titre;
                mailMessage.Body = "Cher/Chère " + contactClients.Contact;
                mailMessage.Body += "<br> Nous avons contactons à propos de votre Ticket N°: " + chatReclamation.Id_Reclamation;
                if (chatReclamation.Description != "")
                {
                    mailMessage.Body += "<br><br> Pour plus de détails voici la réponse du responsable";
                    mailMessage.Body += chatReclamation.Description;
                }
                mailMessage.Body += "<br><br> Rappel:<br>";
                mailMessage.Body += "<br><br> <b>Objet Ticket:</b> " + ReclamationClient.Titre;
                mailMessage.Body += "<br><br> <b>Description: </b>" + ReclamationClient.Description;

                mailMessage.Body += "<br><br><br> Cordialement,";
                mailMessage.Body += "<br><br><br> Group MAS";
                mailMessage.Body += "<br> Service Informatique";
            }
            
            mailMessage.IsBodyHtml = true;

            new SmtpClient("192.168.10.191", 25).Send(mailMessage);

            //redirection
            if (Request["Createur"] == "Client")
            {
                return RedirectToAction("Details", "Home", new { id = Request["Id_Reclamation"] });
            }
            return RedirectToAction("Details", "Crm_ReclamationClient", new { id = Request["Id_Reclamation"] });
        }
        public ActionResult Success()
        {

            return View("Success");
        }

        public List<Client> ClientData()
        {
            return db.Client.ToList();
        }
        public List<ContactClient> ContactClientData()
        {
            var code = Session["UserIDClient"].ToString();
            return db.ContactClient.Where(s => s.CodeClient == code).ToList();
        }
        public List<Crm_ModuleNature> ModuleNatureData()
        {
            return db.Crm_ModuleNature.ToList();
        }
        public List<Respensable> ResponsableData()
        {
            return db.Respensable.ToList();
        }

    }
    public class FileInformation
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        // Ajoutez d'autres propriétés au besoin
    }
    public class Element
    {
        public string a { get; set; }
        public string d { get; set; }
        public string p { get; set; }
        public string t { get; set; }
    }
    public class MailItem
    {
        public int s { get; set; }
        public long d { get; set; }
        public string l { get; set; }
        public string cid { get; set; }
        public int rev { get; set; }
        public string id { get; set; }
        public List<Element> e { get; set; }
        public string su { get; set; }
        public string fr { get; set; }
    }
    public class RootObject
    {
        public List<MailItem> m { get; set; }
    }
}