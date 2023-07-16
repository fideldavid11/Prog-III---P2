using AUTOPROJECT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AUTOPROJECT.Controllers
{
    public class HomeController : Controller
    {
        DB_AUTOSEntities db = new DB_AUTOSEntities();
        public ActionResult Index()
        {
            var data  = db.AUTOS.ToList();
            return View(data);
        }

        
        [HttpPost]

        public ActionResult Index(string searchTxt, string searchTxt1)
        {
            var data = db.AUTOS.ToList();


            if (!string.IsNullOrEmpty(searchTxt))
            {
                data = data.Where(x => x.Marca.Equals(searchTxt)).ToList();
            }

            if (!string.IsNullOrEmpty(searchTxt1))
            {
                data = data.Where(x => x.Anio.Equals(searchTxt1)).ToList();
            }



            return View(data);
        }

       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AUTOS e)
        {
            if (ModelState.IsValid == true)
            {
                
                {
                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.ImgRuta = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            e.ImageFile.SaveAs(fileName);
                            db.AUTOS.Add(e);
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["CreateMessage"] = "<script>alert('Datos insertados correctamente.')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["CreateMessage"] = "<script>alert('Datos no pudieron ser insertados.')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('El tamaño de la imagen debe ser inferior a 1 MB')</script>";
                        }
                    }
                  
                }
            }

            return View();
        }

        public ActionResult Edit(int id )
        {
            var AUTOSRow = db.AUTOS.Where(model => model.IDauto == id).FirstOrDefault();
            Session["Image"] = AUTOSRow.ImgRuta;

            return View(AUTOSRow);
        }

        [HttpPost]

        public ActionResult Edit(AUTOS e)
        {
            if (ModelState.IsValid == true)
            {
                if (e.ImageFile != null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.ImgRuta = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            e.ImageFile.SaveAs(fileName);
                            db.Entry(e).State = EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                string ImagePath = Request.MapPath(Session["Image"].ToString());
                                if (System.IO.File.Exists(ImagePath))
                                {
                                    System.IO.File.Delete(ImagePath);
                                }
                                TempData["UpdateMessage"] = "<script>alert('Datos bien insertados.')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Datos no insertados.')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('El tamaño de la imagen debe ser inferior a 1 MB.')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Formato no soportado')</script>";
                    }


                }
                else
                {
                    e.ImgRuta = Session["Image"].ToString();
                    db.Entry(e).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Datos actualizados correctamente.')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Datos no actualizados.')</script>";
                    }

                }
            }
            return View();
        }


        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var EmployeeRow = db.AUTOS.Where(model => model.IDauto == id).FirstOrDefault();

                if (EmployeeRow != null)
                {
                    db.Entry(EmployeeRow).State = EntityState.Deleted;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMessage"] = "<script>alert('Datos borrado satisfactoriamente.')</script>";
                        string ImagePath = Request.MapPath(EmployeeRow.ImgRuta.ToString());
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                    }
                    else
                    {
                        TempData["DeleteMessage"] = "<script>alert('Datos no eliminados.')</script>";
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(int id)
        {
            var EmployeeRow = db.AUTOS.Where(model => model.IDauto == id).FirstOrDefault();
            Session["Image2"] = EmployeeRow.ImgRuta.ToString();
            return View(EmployeeRow);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
    }
}