using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7.Models;

namespace Nordic_Align_V7.Controllers;

public class OrderController : Controller
{
    private readonly NordicAlignDBContext _db;
    private readonly UserManager<UserModel> _userManager;


    public OrderController(NordicAlignDBContext db, UserManager<UserModel> userManager)
    {
        _db = db;
        _userManager = userManager;

    }
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();
        var role = roles.FirstOrDefault();



        var model = _db.Orders.Include(o => o.Recepient)
                              .Include(o => o.Courier)
                              .Include(o => o.Transport)
                              .Include(o => o.Warehouses)
                              .AsQueryable();

        if (role != "Admin" && user != null)
        {
            var userFullName = user.FirstName + " " + user.LastName; // Пълното име на потребителя
            model = model.Where(o => o.Sender == userFullName);  // Филтрирайте по пълното име на потребителя
        }

        return View("OrdersList", await model.ToListAsync());
    }

    //CREATE
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var model = new OrderModel
        {
            Sender = user.FirstName + " " + user.LastName // Попълване на полето "Sender" с името на потребителя
        };

        ViewBag.Recepients = _db.Recepients.ToList();
        ViewBag.Couriers = _db.Couriers.ToList();
        ViewBag.Transports = _db.Transports.ToList();
        ViewBag.Warehouses = _db.Warehouses.ToList();
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public async Task<IActionResult> Create(OrderModel order, List<int> warehouses)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        order.Sender = user.FirstName + " " + user.LastName;


        if (warehouses != null && warehouses.Any())
        {
            order.Warehouses = _db.Warehouses.Where(x => warehouses.Contains(x.Id)).ToList();
        }
        else
        {
            order.Warehouses = new List<WarehouseModel>();
        }
        order.OrderDate = DateTime.Now;
        _db.Orders.Add(order);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }


    //EDIT
    public IActionResult Edit(int id)
    {
        var model = _db.Orders.FirstOrDefault(x => x.Id == id);
        ViewBag.Recepients = _db.Recepients.ToList();
        ViewBag.Couriers = _db.Couriers.ToList();
        ViewBag.Transports = _db.Transports.ToList();
        ViewBag.Warehouses = _db.Warehouses.ToList();
        return View("CreateOrUpdate", model);
    }
    [HttpPost]
    public IActionResult Edit(OrderModel order, List<int> warehouses)
    {
        var existingOrder = _db.Orders.Include(o => o.Warehouses).FirstOrDefault(o => o.Id == order.Id);

        if (existingOrder == null)
        {
            return NotFound();
        }

        existingOrder.Warehouses.Clear();

        if (warehouses != null && warehouses.Any())
        {
            var selectedWarehouses = _db.Warehouses.Where(w => warehouses.Contains(w.Id)).ToList();
            foreach (var warehouse in selectedWarehouses)
            {
                existingOrder.Warehouses.Add(warehouse);
            }
        }
        if (string.IsNullOrEmpty(order.Sender))
        {
            var user = _userManager.GetUserAsync(User).Result;
            order.Sender = user!.FirstName + " " + user.LastName;
        }
        else
        {
            existingOrder.Sender = order.Sender;

        }

        existingOrder.RecepientId = order.RecepientId;
        existingOrder.CourierId = order.CourierId;
        existingOrder.DeliveryDate = order.DeliveryDate;
        existingOrder.TransportId = order.TransportId;
        existingOrder.Price = order.Price;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    //DELETE
    public IActionResult Delete(int id)
    {
        var order = _db.Orders.FirstOrDefault(x => x.Id == id);
        _db.Orders.Remove(order!);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }


    //SEARCH 
    public IActionResult OrdersByCourierId(int courierId)
    {
        var model = _db.Orders.Where(x => x.CourierId == courierId);
        return View("OrdersList", model);
    }
    public IActionResult OrdersByTransportId(int transportId)
    {
        var model = _db.Orders.Where(x => x.TransportId == transportId);
        return View("OrdersList", model);
    }
    public IActionResult OrdersByRecipientId(int recipientId)
    {
        var model = _db.Orders.Where(x => x.RecepientId == recipientId);
        return View("OrdersList", model);
    }


    [Route("Order/OrdersByDate")]
    public async Task<IActionResult> OrdersByDate(DateTime? startDate, DateTime? endDate)
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();
        var role = roles.FirstOrDefault();

        // Add FulName in ViewBag
        ViewBag.UserFullName = user!.FirstName + " " + user.LastName;

        var model = _db.Orders.Include(o => o.Recepient)
                              .Include(o => o.Courier)
                              .Include(o => o.Transport)
                              .Include(o => o.Warehouses)
                              .AsQueryable();

        if (startDate.HasValue)
        {
            model = model.Where(o => o.OrderDate >= startDate.Value);
        }
        if (endDate.HasValue)
        {
            model = model.Where(o => o.OrderDate <= endDate.Value);
        }

        if (role != "Admin" && user != null)
        {
            model = model.Where(o => o.Sender == user.UserName);
        }

        var orderList = model.ToList();

        if (orderList.Count == 0)
        {
            TempData["Message"] = "Det finns inga beställningar för de valda datumen.";
        }

        return View("OrdersList", orderList);
    }
}
