using Microsoft.AspNetCore.Mvc;
using Nordic_Align_V7.Models;
using Nordic_Align_V7;
using Microsoft.EntityFrameworkCore;

public class TransportController : Controller
{
    private readonly NordicAlignDBContext _db;

    public TransportController(NordicAlignDBContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        var model = _db.Transports.AsEnumerable();
        return View("TransportsList", model);
    }

    //CREATE
    public IActionResult Create()
    {
        var model = new TranportModel();
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Create(TranportModel tranport, string registrationDate)
    {
        try
        {
            
        tranport.RegistrationDate = DateTime.Parse(registrationDate);
        _db.Transports.Add(tranport);
        _db.SaveChanges();
        return RedirectToAction("Index");
           

        }
        catch (Exception ex)
        {
            ViewBag.RegisterFail = ex.InnerException?.Message ?? ex.Message;
            return View("CreateOrUpdate", tranport);
        }
    }

    //EDIT
    public IActionResult Edit(int id)
    {
        var model = _db.Transports.FirstOrDefault(x => x.Id == id);
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Edit(TranportModel tranport, string registrationDate)
    {
        tranport.RegistrationDate = DateTime.Parse(registrationDate);
        _db.Transports.Update(tranport);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //DELETE
    public IActionResult Delete(int id)
    {
        var transport = _db.Transports.Include(t => t.Orders).FirstOrDefault(x => x.Id == id);

        if (transport == null)
        {
            return NotFound(); 
        }

        if (transport.Orders != null && transport.Orders.Any())
        {
            TempData["ErrorMessage"] = "Operation är omöjlig. Fordonen är kopplad till aktiva beställningar.";
            return RedirectToAction("Index");
        }

        _db.Transports.Remove(transport);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

}
