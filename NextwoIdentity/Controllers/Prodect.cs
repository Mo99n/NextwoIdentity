using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity.Data;
using NextwoIdentity.Models.ViewModels;
using System.Data;

namespace NextwoIdentity.Controllers
{
    [Authorize]
    public class Prodect : Controller
    {
        private readonly NextwoDbContext db;
        private readonly ILogger<HomeController> _logger;

        public Prodect(ILogger<HomeController> logger, NextwoDbContext _db)
        {
            _logger = logger;
            db = _db;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CreateProdect()
        {
           ViewBag.AllPro = new SelectList(db!.Prodect, "ProdectId", "ProdectName",db.Categorys);

            return View();

        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateProdect(Models.ViewModels.Prodects prodect)
        {
        
            if (ModelState.IsValid)
            {
                db.Prodect.Add(prodect);
                db.SaveChanges();
                return RedirectToAction("AllProdect");
            }
            return View(prodect);

        }
        [HttpGet]
        public ActionResult CreateCategory()
        {
            ViewBag.AllCat = new SelectList(db!.Categorys, "CategoreyId", "CategoreyName");

            return View();

        }
        [HttpPost]
        public ActionResult CreateCategory(CategoryController category)
        {
            if (ModelState.IsValid)
            {
               /* db.Categorys.Add(category.);
                db.SaveChanges();*/
                return RedirectToAction("AllProdect");
            }
            return View(category);

        }
        [HttpGet]
        public ActionResult AllProdect()
        {
              var data =  db.Prodect.Include(x => x.Category);

            return  View(data);

        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditProdects(int? id)
        {
            if (id == null)
            {
                RedirectToAction("AllProdect");
            }
            var data = db.Prodect.Find(id);

            if (data == null) { RedirectToAction("AllProdect"); }

            return View(data);

        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditProdects(Models.ViewModels.Prodects prodects)
        {
            if (ModelState.IsValid)
            {
                db.Prodect.Update(prodects);
                db.SaveChanges();
                return RedirectToAction("AllProdect");
            }
            return View(prodects);

        }


        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                RedirectToAction("AllProdect");
            }
            var data = db.Prodect.Find(id);

            if (data == null) { RedirectToAction("AllProdect"); }

            return View(data);

        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                RedirectToAction("AllProdect");
            }
            var data = db.Prodect.Find(id);

            if (data == null) { RedirectToAction("AllProdect"); }

            return View(data);

        }

       [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Prodects prodect)
        {
            var data = db.Prodect.Find(prodect.ProdectId);

            if (data == null)

            { return RedirectToAction("AllProdect"); }

            db.Prodect.Remove(data);
            db.SaveChanges();

            return RedirectToAction("AllProdect");
        }
    }
}
