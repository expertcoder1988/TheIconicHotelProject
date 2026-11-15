using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MountFeastMVC.Models;

namespace HotelWebsite.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            ViewBag.Title = "0";
            return View();
        }

        public JsonResult PostList()
        {
            try
            {
                var res = (from obj in HDE.Posts
                           where obj.IsPublished == true
                           select new
                           {
                               Title = obj.Title,
                               pDate = obj.PublishedDate,
                               Id = obj.PostId
                           }).ToList();

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            var res = (from obj in HDE.Posts
                       where obj.IsPublished == true && obj.PostId == id 
                       select new Article
                       {
                           Title = obj.Title,
                           PublishedDate = obj.PublishedDate,
                           PostId = obj.PostId,
                           Slug = obj.Slug ,
                           Description = obj.Description,
                           ShortDescriptions = obj.ShortDescriptions,
                           MetaDescription = obj.MetaDescription,
                           Keywords = obj.Keywords,
                           Author = obj.Author,
                           Tags = obj.Tags 
                       }).ToList();

            TempData["Post"] = res;
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public string Manage(Post model)
        {
            try
            {
                model.IsPublished = true;
                model.PublishedDate = DateTime.Now;
                model.ModifyDate = DateTime.Now;

                HDE.Entry(model).State = System.Data.EntityState.Added;
                HDE.SaveChanges();

                return "Post saved succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public string EditPost(Post model)
        {
            try
            {
                model.IsPublished = true;
                model.PublishedDate = DateTime.Now;
                model.ModifyDate = DateTime.Now;

                HDE.Entry(model).State = System.Data.EntityState.Modified;
                HDE.SaveChanges();

                return "Post saved succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string DeletePost(int postId)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Posts.FirstOrDefault(item => item.PostId == postId);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.IsPublished = false;

                        // Save changes in database
                        context.SaveChanges();
                    }
                }

                return "Property updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/blog"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }

    public class Article
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string DefaultImage { get; set; }
        public bool IsPublished { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsDeleted { get; set; }
        public string VideoUrl { get; set; }
        public string ShortDescriptions { get; set; }
        public string Tags { get; set; }
        public string MetaDescription { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }
    }

}