using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Models;

namespace Nordic_Align_V7.Controllers;

public class RecepientController : Controller
{
    private readonly NordicAlignDBContext _db;

    public RecepientController(NordicAlignDBContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        var model = _db.Recepients.AsEnumerable();
        return View("RecepientsList", model);
    }


    //CREATE
    public IActionResult Create()
    {
        var model = new RecepientModel();
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Create(RecepientModel recepient)
    {
        _db.Recepients.Add(recepient);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //EDIT
    public IActionResult Edit(int id)
    {
        var model = _db.Recepients.FirstOrDefault(x => x.Id == id);
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Edit(RecepientModel recepient)
    {
        _db.Recepients.Update(recepient);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //DELETE
    public IActionResult Delete(int id)
    {
        var recepient = _db.Recepients.FirstOrDefault(x => x.Id == id);

        if (recepient == null)
        {
            return NotFound(); // 404 fel
        }

        _db.Recepients.Remove(recepient);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }
}
