﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repos;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var employees = await _employeeRepo.GetAllAsync();
            return Newtonsoft.Json.JsonConvert.SerializeObject(employees);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeRepo.GetAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

             return Ok(employee);
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            _employeeRepo.InsertAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
            // return CreatedAtRoute("Employee/Create", true);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Employee employee)
        {
            _employeeRepo.UpdateAsync(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _employeeRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}