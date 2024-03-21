using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRMSTUBSOFT.Services.Business;


namespace CRMSTUBSOFT.Controllers
{
    
    public class AuthentificationController : Controller
    {
     

        // GET: /Authentification/Index
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // POST: /Authentification/Index
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Utilisateur utilisateur, string check, string RememberMe)
        {

            if (Session["UserNom"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            if (RememberMe != null)
            {
                
                // Create a cookie with the username or any other relevant data
                // Create a new cookie with the desired name and value
                HttpCookie cookie = new HttpCookie("username", utilisateur.NomUtilisateur);

                // Set additional cookie properties, if needed
                cookie.Expires = DateTime.Now.AddDays(7); // Set the expiration date

                // Add the cookie to the response
                Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie existingCookie = Request.Cookies["username"];
                if (existingCookie != null)
                {
                    existingCookie.Value = null;
                    // Set additional cookie properties, if needed
                    existingCookie.Expires = DateTime.Now.AddDays(7);

                    // Add the cookie to the response
                    Response.Cookies.Set(existingCookie);

                }
                   
              
            }

            SecurityServices securityService = new SecurityServices();
            string success = "";
            Crm_AccesUtilisateur AccesUtilisateur = new Crm_AccesUtilisateur();
            Fonction FonctionUtilisateur = new Fonction();
            Respensable ResponsableInfos = new Respensable();
            Utilisateur utilisateurAuth = new Utilisateur();
            ClientCRM clientCRMAuth = new ClientCRM();

            success = securityService.Authenticate(utilisateur, check);
            
            if (check == "User")
            {
                
                if (success == "authConnection")
                {
                    AccesUtilisateur = securityService.GetRole(utilisateur);
                    FonctionUtilisateur = securityService.GetFonction(utilisateur.CodeFonction);
                    ResponsableInfos = securityService.GetResponsable(utilisateur.CodeRepresentant);
                    utilisateurAuth = securityService.AuthenticateName(utilisateur);
                   
                    // Emitting forms authentication cookie
                    FormsAuthentication.SetAuthCookie(utilisateurAuth.NomUtilisateur, false);

                    Session["UserID"] = utilisateurAuth.NomUtilisateur.ToString();
                    Session["UserRole"] = AccesUtilisateur.CodeAcces.ToString();
                    Session["UserFunction"] = utilisateurAuth.Fonction.Libelle.ToString();
                    Session["UserNom"] = utilisateurAuth.Nom.ToString();
                    Session["UserPrenom"] = utilisateurAuth.Prenom.ToString();
                    Session["codeResponsable"] = utilisateurAuth.CodeRepresentant;


                    Session["UserIDClient"] = utilisateurAuth.NomUtilisateur.ToString();




                    return RedirectToAction("Index", "Home");
                }
            }
            
            if (check == "Client")
            {
                if (success == "authConnectionClient")

                {
                    clientCRMAuth = securityService.AuthenticateClient(utilisateur);
                   
                    FormsAuthentication.SetAuthCookie(clientCRMAuth.MotPasse, false);

                    Session["UserIDClient"] = clientCRMAuth.CodeClient.ToString();
                    Session["UserLogin"] = clientCRMAuth.RaisonSociale.ToString();
                    return RedirectToAction("IndexClient", "Home");
                }
            }
            if (success == "errorauthConnection" || success == "errorauthConnectionClient")
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "error" });
                
            }

            //return "success authentificate";

            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Remove("UserID");
            Session.Remove("UserRole");
            Session.Remove("UserNom");
            Session.Remove("UserPrenom");
            Session.Remove("UserIDClient");
            Session.Remove("UserLogin");
            return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
        }
        

       /* public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }*/
    }
}
