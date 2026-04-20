using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService service, ILogger<StudentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all students");

            var students = await _service.GetAllStudentsAsync();

            return Ok(new
            {
                Success = true,
                Data = students
            });
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetStudentByIdAsync(id);

            return Ok(new
            {
                Success = true,
                Data = student
            });
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid student data for creation");

                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            await _service.AddStudentAsync(student);

            _logger.LogInformation("Student created: {Name}", student.Name);

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, new
            {
                Success = true,
                Message = "Student added successfully"
            });
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "ID mismatch"
                });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid student data for update");

                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data"
                });
            }

            await _service.UpdateStudentAsync(student);

            _logger.LogInformation("Student updated: {Id}", student.Id);

            return Ok(new
            {
                Success = true,
                Message = "Student updated successfully"
            });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteStudentAsync(id);

            _logger.LogInformation("Student deleted: {Id}", id);

            return Ok(new
            {
                Success = true,
                Message = "Student deleted successfully"
            });
        }
    }
}