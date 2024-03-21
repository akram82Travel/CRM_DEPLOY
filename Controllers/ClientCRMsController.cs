using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRMSTUBSOFT;
using CRMSTUBSOFT.Services.Business;

namespace CRMSTUBSOFT.Controllers
{
    public class ClientCRMsController : Controller
    {
        private CrmModelEntities db = new CrmModelEntities();
 SecurityServices securityService = new SecurityServices();
        // GET: ClientCRMs
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            return View(db.ClientCRM.ToList());
        }

        // GET: ClientCRMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientCRM clientCRM = db.ClientCRM.Find(id);
            if (clientCRM == null)
            {
                return HttpNotFound();
            }
            return View(clientCRM);
        }

        // GET: ClientCRMs/Create
        public ActionResult Create()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }
            return View();
        }

        // POST: ClientCRMs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeClient,RaisonSociale,Adresse1,MartriculeFiscale,Tel1,Email,CodeClientCommercial, MotPasse")] ClientCRM clientCRM)
        {
            if (ModelState.IsValid)
            {
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["imageElement"];
               
              // clientCRM.imageElement = securityService.ConvertToBytes(file);

                /*EndInsert image*/


                /*Image/root/rename*/

                Guid guid = Guid.NewGuid();

                string newfileName = guid.ToString();

                string fileextention = Path.GetExtension(file.FileName);

                string fileName = newfileName + fileextention;

                string uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));

                file.SaveAs(uploadpath);

                /*End/Image/root/rename*/

              //  clientCRM.FileName = fileName;
              // clientCRM.imagephysique = file.FileName;
                
                db.ClientCRM.Add(clientCRM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clientCRM);
        }

        // GET: ClientCRMs/Edit/5
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
            ClientCRM clientCRM = db.ClientCRM.Find(id);
            if (clientCRM == null)
            {
                return HttpNotFound();
            }
            return View(clientCRM);
        }

        // POST: ClientCRMs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeClient,RaisonSociale,Adresse1,MartriculeFiscale,Tel1,Email,CodeClientCommercial,MotPasse,imageElement,FileName,imagephysique")] ClientCRM clientCRM)
        {
            if (ModelState.IsValid)
            {
                SecurityServices securityService = new SecurityServices();
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["EditimageElement"];
                if (file.FileName != "")
                {
                    //clientCRM.imageElement = securityService.ConvertToBytes(file);
                }
                //*EndInsert image*/

                /*Update file*/

                if (file.FileName != "")
                {
                    //Delete Old file from the file system
                   /* var path = Path.Combine(Server.MapPath("~/UploadedFiles"), clientCRM.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }*/

                    //ADD NEW file from the file system

                    string uploadpath = "";

                    Guid guid = Guid.NewGuid();

                    string newfileName = guid.ToString();

                    string fileextention = Path.GetExtension(file.FileName);

                    string fileName = newfileName + fileextention;


                    uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));
                   // clientCRM.FileName = fileName;
                   // clientCRM.imagephysique = file.FileName;

                    file.SaveAs(uploadpath);
                }
                /*Insert Update file*/

                db.Entry(clientCRM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientCRM);
        }

        // GET: ClientCRMs/Delete/5
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
            ClientCRM clientCRM = db.ClientCRM.Find(id);
            if (clientCRM == null)
            {
                return HttpNotFound();
            }
            return View(clientCRM);
        }

        // POST: ClientCRMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ClientCRM clientCRM = db.ClientCRM.Find(id);
            db.ClientCRM.Remove(clientCRM);
            db.SaveChanges();

           /* if (clientCRM.FileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/UploadedFiles"), clientCRM.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }*/
            return RedirectToAction("Index");
        }


        /*public ActionResult RetrieveImage(string id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }*/
        /*public byte[] GetImageFromDataBase(string Id)
        {
            /*var q = from temp in db.ClientCRM where temp.CodeClient == Id select temp.imageElement;
            byte[] cover = q.First();
            //return cover;
        }*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
