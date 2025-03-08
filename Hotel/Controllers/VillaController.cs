using Hotel.Domain.Entities;
using Hotel.Infrastructue.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hotel.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name == obj.Description)
            {
                ModelState.AddModelError("Description", "The  Description Can't be the same of Name");
            }
            if (ModelState.IsValid)
            {

                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Villa Has Been Added Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Update(int id)
        {
            var obj = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {

            if (ModelState.IsValid)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Villa Has Been Updated Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }



        public IActionResult Delete(int id)
        {
            var obj = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {

            if (obj is not null)
            {
                _db.Villas.Remove(obj);
                _db.SaveChanges();
                TempData["Success"] = "Villa Has Benn Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
