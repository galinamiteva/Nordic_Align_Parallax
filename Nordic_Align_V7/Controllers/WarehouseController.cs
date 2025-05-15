using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7.Models;

namespace Nordic_Align_V7.Controllers;

public class WarehouseController : Controller
{
    private readonly NordicAlignDBContext _db;

    public WarehouseController(NordicAlignDBContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        var model = _db.Warehouses.AsEnumerable();
        return View("WarehousesList", model);
    }
    //CREATE
    public IActionResult Create()
    {
        var model = new WarehouseModel();
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Create(WarehouseModel warehouse)
    {
        _db.Warehouses.Add(warehouse);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //EDIT
    public IActionResult Edit(int id)
    {
        var model = _db.Warehouses.FirstOrDefault(x => x.Id == id);
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Edit(WarehouseModel warehouse)
    {
        _db.Warehouses.Update(warehouse);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //DELETE
    public IActionResult Delete(int id)
    {
        var warehouse = _db.Warehouses.Include(w => w.Orders).FirstOrDefault(x => x.Id == id);

        if (warehouse == null)
        {
            return NotFound(); 
        }

        if (warehouse.Orders != null && warehouse.Orders.Any())
        {
            TempData["ErrorMessage"] = @Nordic_Align_V7.Resources.Resource.Operationäromöjlig_Lagretharaktivabeställningar;
            return RedirectToAction("Index");
        }

        _db.Warehouses.Remove(warehouse);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

}
