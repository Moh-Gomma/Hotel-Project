using Hotel.Domain.Entities;
using Hotel.Infrastructue.Data;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hotel.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(x => x.villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM VillaList = new()
            {
                VillaList = _db.Villas.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(VillaList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VillaNumberVM obj)
        {
            var ExistedVillaId = _db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !ExistedVillaId)
            {

                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["Success"] = "Villa Number Has Been Added Successfully";

                return RedirectToAction(nameof(Index));
            }
            if (ExistedVillaId)
            {
                TempData["Error"] = "Number is Already Exist";
            }
            obj.VillaList = _db.Villas.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return View(obj);
        }
        public IActionResult Update(int id)
        {
            var selectedVilla = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == id);
            if (selectedVilla == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var villNumberVm = new VillaNumberVM
            {
                VillaNumber = selectedVilla,
                VillaList = _db.Villas.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            return View(villNumberVm);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villanumberVm)
        {

            if (ModelState.IsValid)
            {

                _db.VillaNumbers.Update(villanumberVm.VillaNumber);
                _db.SaveChanges();
                TempData["Success"] = "Villa Number Has Been Updated Successfully";

                return RedirectToAction(nameof(Index));
            }

            villanumberVm.VillaList = _db.Villas.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            return View(villanumberVm);
        }



        public IActionResult Delete(int id)
        {
            var selectedVilla = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == id);
            if (selectedVilla == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var villNumberVm = new VillaNumberVM
            {
                VillaNumber = selectedVilla,
                VillaList = _db.Villas.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            return View(villNumberVm);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            var selectedVilla = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (selectedVilla is not null)
            {
                _db.VillaNumbers.Remove(selectedVilla);
                _db.SaveChanges();
                TempData["Success"] = "Villa Number Has Benn Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
