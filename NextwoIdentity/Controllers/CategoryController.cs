using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NextwoIdentity.Data;
using NextwoIdentity.Models.ViewModels;
using System.Data;


namespace NextwoIdentity.Controllers
{
    public class CategoryController : Controller
    {

        private readonly NextwoDbContext db;

        public CategoryController( NextwoDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public ActionResult AllCategory()
        {
            var data = db.Categorys;

            return View(data);

        }
        [HttpGet]
        public ActionResult CreateCategory()
        {
            ViewBag.AllCat = new SelectList(db!.Categorys, "CategoreyId", "CategoreyName");

            return View();

        }
        [HttpPost]
        public ActionResult CreateCategory(Models.ViewModels.Category category)
        {

            var CategoryExist = db.Categorys.Any(x => x.CategoreyName == category.CategoreyName);

            if (ModelState.IsValid&& CategoryExist)
            {
                ModelState.AddModelError(nameof(category.CategoreyName), "Category is Already Exist");
                return View(category);
            }
     
            db.Categorys.Add(category);
            db.SaveChanges();
            return RedirectToAction("AllCategory");

        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                RedirectToAction("AllCategory");
            }
            var data = db.Categorys.Find(id);

            if (data == null) { RedirectToAction("AllCategory"); }
            return View(data);

        }

        [HttpPost]
        public ActionResult EditCategory( Models.ViewModels.Category category)
        {
            if (ModelState.IsValid)
            {
               db.Categorys.Update(category);
                db.SaveChanges();
                return RedirectToAction("AllCategory");
            }

            return View(category);

        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                RedirectToAction("AllCategory");
            }
            var data = db.Categorys.Find(id);

            if (data == null) { RedirectToAction("AllCategory"); }

            return View(data);

        }

        [HttpPost]
        public ActionResult Delete(Models.ViewModels.Category category)
        {
            var data = db.Categorys.Find(category.CategoreyId);

            if (data == null)

            { return RedirectToAction("AllCategory"); }

            db.Categorys.Remove(category);
            db.SaveChanges();

            return RedirectToAction("AllCategory");
        }


        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                RedirectToAction("AllCategory");
            }
            var data = db.Categorys.Find(id);

            if (data == null) { RedirectToAction("AllCategory"); }

            return View(data);

        }




    }
}
