using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;

namespace HotelWebsite.Controllers
{
    [Authorize]
    public class VoucherController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: Voucher
        public ActionResult Index()
        {
            var vouchers = db.Vouchers.Include(v => v.RoomCategory).ToList();
            return View(vouchers);
        }

        // GET: Voucher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Include(v => v.RoomCategory).FirstOrDefault(v => v.VoucherId == id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // GET: Voucher/Create
        public string GenerateVoucherCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: Voucher/Create
        public ActionResult Create()
        {
            ViewBag.VoucherCode = GenerateVoucherCode(6); // Generate and set the voucher code

            // Fetch only active categories
            var activeCategories = db.RoomCategories.Where(c => c.Status == true).ToList();
            ViewBag.Categories = activeCategories; // Set active categories in ViewBag

            return View();
        }


        // POST: Voucher/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Code,DiscountAmount,ExpiryDate,IsActive,CategoryId,Remarks")] Voucher voucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    voucher.IsActive = true; // Set IsActive to true by default

                    db.Vouchers.Add(voucher);
                    db.SaveChanges();

                    return Json(new { success = true, message = "Voucher added successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "ModelState is not valid" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Voucher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }

            // Fetch only active categories for the dropdown
            var activeCategories = db.RoomCategories.Where(c => c.Status == true).ToList();
            ViewBag.Categories = new SelectList(activeCategories, "Id", "Category", voucher.CategoryId);

            return View(voucher);
        }


        // POST: Voucher/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "VoucherId,Code,DiscountAmount,ExpiryDate,IsActive,CategoryId,Remarks")] Voucher voucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(voucher).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Voucher updated successfully" });
                }
                else
                {
                    // Log model state errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "ModelState is not valid", errors });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Voucher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Voucher voucher = db.Vouchers.Find(id);
                if (voucher == null)
                {
                    return HttpNotFound(); // Handle case where voucher with given id is not found
                }

                db.Vouchers.Remove(voucher);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

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
