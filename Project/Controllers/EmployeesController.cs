using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeesModel>>> GetEmployees()
        {
            return await (from s in _context.Employees
                          select new EmployeesModel
                          {

                              EmployeeId = s.EmployeeId,
                              LastName = s.LastName,
                              FirstName = s.FirstName

                          }).ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<EmployeesModel>>> GetEmployees(int id)
        {
            var employees = await (from s in _context.Employees
                                   where id == s.EmployeeId
                                   select new EmployeesModel
                                   {

                                       EmployeeId = s.EmployeeId,
                                       LastName = s.LastName,
                                       FirstName = s.FirstName

                                   }).ToListAsync();

            if (employees == null)
            {
                return NotFound();
            }

            return employees;
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, Employees employees)
        {
            if (id != employees.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployees(Employees employees)
        {
            _context.Employees.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployees", new { id = employees.EmployeeId }, employees);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employees>> DeleteEmployees(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();

            return employees;
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
