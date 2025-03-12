using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nordic_Align_V7.Models;

namespace Nordic_Align_V7.Controllers;

public class CourierController : Controller
{
    private readonly NordicAlignDBContext _db;

    public CourierController(NordicAlignDBContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var model = _db.Couriers.AsEnumerable();
        return View("CouriersList", model);
    }
    public IActionResult Create()
    {
        var model = new CourierModel();
        return View("CreateOrUpdate", model);
    }



    //CREATE
    [HttpPost]
    public IActionResult Create(CourierModel courier, string endWorkTime, string startWorkTime, string employmentDate)
    {

        courier.EndWorkTime = TimeSpan.Parse(endWorkTime);
        courier.StartWorkTime = TimeSpan.Parse(startWorkTime);
        courier.EmploymentDate = DateTime.Parse(employmentDate);
        _db.Couriers.Add(courier);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //EDIT
    public IActionResult Edit(int id)
    {
        var model = _db.Couriers.FirstOrDefault(x => x.Id == id);
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Edit(CourierModel courier, string endWorkTime, string startWorkTime, string employmentDate)
    {
        courier.EndWorkTime = TimeSpan.Parse(endWorkTime);
        courier.StartWorkTime = TimeSpan.Parse(startWorkTime);
        courier.EmploymentDate = DateTime.Parse(employmentDate);
        _db.Couriers.Update(courier);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //DELETE
    public IActionResult Delete(int id)
    {
        var courier = _db.Couriers.FirstOrDefault(x => x.Id == id);
        _db.Couriers.Remove(courier!);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //SEARCH
    public IActionResult Search(string searchString)
    {
        if (searchString == null || searchString.IsNullOrEmpty())
            return RedirectToAction("Index");
        var model = _db.Couriers.Where(x => x.FullName.Contains(searchString));
        return View("CouriersList", model);
    }
}
