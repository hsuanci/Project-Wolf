using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public OrdersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersModel>>> GetOrders()
        {
            return await (from s in _context.Orders
                          join c in _context.Employees on s.EmployeeId equals c.EmployeeId
                          select new OrdersModel
                          {
                              OrderId = s.OrderId,
                              CustomerId = s.CustomerId,
                              EmployeeId = s.EmployeeId,
                              EmployeeName = c.LastName + ',' + c.FirstName,
                              OrderDate = s.OrderDate,
                              RequiredDate = s.RequiredDate,
                              ShipVia = s.ShipVia,
                              Freight = s.Freight,
                              ShipName = s.ShipName,
                              ShipAddress = s.ShipAddress,
                              ShipCity = s.ShipCity,
                              ShipRegion = s.ShipRegion,
                              ShipPostalCode = s.ShipPostalCode,
                              ShipCountry = s.ShipCountry
                          }).ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrdersModel>>> GetOrders(int id)
        {
            var orders = await (from s in _context.Orders
                                join c in _context.Employees on s.EmployeeId equals c.EmployeeId
                                where s.OrderId == id
                                select new OrdersModel
                                {
                                    OrderId = s.OrderId,
                                    CustomerId = s.CustomerId,
                                    EmployeeId = s.EmployeeId,
                                    EmployeeName = c.LastName + ',' + c.FirstName,
                                    OrderDate = s.OrderDate,
                                    RequiredDate = s.RequiredDate,
                                    ShipVia = s.ShipVia,
                                    Freight = s.Freight,
                                    ShipName = s.ShipName,
                                    ShipAddress = s.ShipAddress,
                                    ShipCity = s.ShipCity,
                                    ShipRegion = s.ShipRegion,
                                    ShipPostalCode = s.ShipPostalCode,
                                    ShipCountry = s.ShipCountry
                                }).ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(int id, Orders orders)
        {
            if (id != orders.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(orders).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Successful Update OrderID:" + id + "Detail String:" + JsonConvert.SerializeObject(orders));
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Orders>> PostOrders(Orders orders)
        {
            try
            {
                _context.Orders.Add(orders);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetOrders", new { id = orders.OrderId }, orders);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Orders>> DeleteOrders(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return orders;
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
