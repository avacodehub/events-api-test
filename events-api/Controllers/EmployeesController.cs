#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using events_api.Data;

namespace events_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly Context _context;

        public EmployeesController(Context context)
        {
            _context = context;
        }

        // POST: api/Employee/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> SearchEmployees(EmployeeFilterDTO employeeFilterDTO)
        { 
            
            IQueryable<Employee> q = _context.Employees.Include(x => x.Position);

            if (employeeFilterDTO.PositionIds !=null && employeeFilterDTO.PositionIds.Any())
            {
                var _guids = employeeFilterDTO.PositionIds.Select(z => Guid.ParseExact(z, "D")).ToList();
                q = q.Where(x => _guids.Any(z => z == x.PositionId));
            }

            if (!String.IsNullOrEmpty(employeeFilterDTO.SearchTerm))
            {
                q = q.Where(x => x.LastName.Contains(employeeFilterDTO.SearchTerm));
            }
            return await q.Select(employee => new EmployeeDTO(employee)).ToListAsync();
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, EmployeePostDTO employee)
        {
            Employee _employee = _context.Employees.Find(Guid.ParseExact(id, "D"));

            if (_employee == null)
            {
                return NotFound();
            }

            _employee.LastName = employee.LastName;
            _employee.FirstName = employee.FirstName;
            _employee.MiddleName = employee.MiddleName;
            _employee.Phone = employee.Phone;
            _employee.PositionId = employee.PositionId;
            _employee.Position = _context.Positions.FirstOrDefault(x => x.Id == employee.PositionId);

         

            _context.Entry(_employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostEmployee(EmployeePostDTO employeeDTO)
        {
           
            var employee = new Employee
            {
                LastName = employeeDTO.LastName,
                FirstName = employeeDTO.FirstName,
                MiddleName = employeeDTO.MiddleName,
                Phone = employeeDTO.Phone,
                PositionId = employeeDTO.PositionId ,
               Position = _context.Positions.FirstOrDefault(x => x.Id == employeeDTO.PositionId)
               

            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee.Id.ToString();
           
        }

        // DELETE: api/Employees/5
        [HttpPost("delete")]
        public async Task<IActionResult> PostEmployeeDelete(List<string> ids)
        {
            var _guids = ids.Select(z => Guid.ParseExact(z, "D"));
            var _employees = _context.Employees.Where(x => _guids.Any(z => z == x.Id)).ToList();

            _context.Employees.RemoveRange(_employees);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Employees/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEmployee(Guid id)
        //{
        //    var employee = await _context.Employees.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Employees.Remove(employee);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
