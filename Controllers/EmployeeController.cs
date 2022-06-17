using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using ProjektSwagger.Models;
using System.Data.Entity.Hierarchy;
using System.Web;

namespace ProjektSwagger.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context) {
            _context = context;
        }

        /////////////////////////////////////////////////////////////////////////////

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<string>> GetEmployee() {
            IEnumerable<Employee> employees = from x in _context.Employees select x;
            return employees.ToList().Select(x => x.ToString());
        }

        // GET: api/Employees/getByID/5
        [HttpGet("getByID/{id}")]
        public async Task<ActionResult<string>> GetEmployee(int id) {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null) {
                return NotFound();
            }

            return employee.ToString();
        }

        // GET: api/Employees/getByHierarchy/foo
        [HttpGet("getByHierarchy/{hierarchy}")]
        public async Task<IEnumerable<string>> GetEmployee(string hierarchy) {
            var parsedHierarchy = HttpUtility.UrlDecode(hierarchy);
            try {
                var hierarchyTyped = new HierarchyId(parsedHierarchy);
                IEnumerable<Employee> employees = from x in _context.Employees
                                                  where x.Hierarchy == hierarchyTyped
                                                  select x;
                return employees.ToList().Select(x => x.ToString());
            } catch (Exception) {
                return new List<string> { "Incorrect Hierarchy path provided." };
            }
        }

        // GET: api/Employees/getByHierarchy/foo
        [HttpGet("getSubordinatesByHierarchy/{hierarchy}")]
        public async Task<IEnumerable<string>> GetEmployeeSubordinates(string hierarchy) {
            var parsedHierarchy = HttpUtility.UrlDecode(hierarchy);
            Employee employee = (from x in _context.Employees
                                 where x.Hierarchy == new HierarchyId(parsedHierarchy)
                                 select x).FirstOrDefault();
            IEnumerable<Employee> subordinates = employee.GetSubordinates(_context);
            return subordinates.ToList().Select(x => x.ToString());
        }

        // GET: api/Employees/getAverageWage
        [HttpGet("getAverageWage")]
        public async Task<int> GetAvgWage() {
            var avgWage = _context.Database.SqlQuery<int>("EXEC GetAvgWage").FirstOrDefault();
            return avgWage;
        }

        // GET: api/Employees/getMaxWage
        [HttpGet("getMaxWage")]
        public async Task<int> getMaxWage() {
            var avgWage = _context.Database.SqlQuery<int>("EXEC getMaxWage").FirstOrDefault();
            return avgWage;
        }

        // GET: api/Employees/getMinWage
        [HttpGet("getMinWage")]
        public async Task<int> getMinWage() {
            var avgWage = _context.Database.SqlQuery<int>("EXEC getMinWage").FirstOrDefault();
            return avgWage;
        }

        /////////////////////////////////////////////////////////////////////////////

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromForm] Employee employee) {
            if (id != employee.Id) {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (Exception) {
                if (!EmployeeExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromForm] Employee employee) {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id) {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        // DELETE: api/Employees/5
        [HttpDelete("restructuring/{hierarchy}")]
        public async Task<IEnumerable<string>> Restructuring(string hierarchy) {
            var parsedHierarchy = HttpUtility.UrlDecode(hierarchy);
            try {
                var hierarchyTyped = new HierarchyId(parsedHierarchy);
                Employee employee = (from x in _context.Employees
                                     where x.Hierarchy == hierarchyTyped
                                     select x).FirstOrDefault();
                employee.Restructuring(_context);
                return new List<string> { "Restructured division {" + parsedHierarchy + "}." };
            } catch (Exception) {
                return new List<string> { "Incorrect Hierarchy path provided." };
            }
        }

        private bool EmployeeExists(int id) {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
