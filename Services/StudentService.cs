using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories.Interfaces;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository repo, ILogger<StudentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // ✅ GET ALL
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _repo.GetAllAsync();
        }

        // ✅ GET BY ID
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var student = await _repo.GetByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student not found with Id: {Id}", id);
                throw new KeyNotFoundException("Student not found");
            }

            return student;
        }

        // ✅ ADD
        public async Task AddStudentAsync(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.Name))
                throw new ArgumentException("Student name is required");

            await _repo.AddAsync(student);

            _logger.LogInformation("Student added: {Name}", student.Name);
        }

        // ✅ UPDATE
        public async Task UpdateStudentAsync(Student student)
        {
            var existing = await _repo.GetByIdAsync(student.Id);

            if (existing == null)
            {
                _logger.LogWarning("Student not found for update: {Id}", student.Id);
                throw new KeyNotFoundException("Student not found");
            }

            await _repo.UpdateAsync(student);

            _logger.LogInformation("Student updated: {Id}", student.Id);
        }

        // ✅ DELETE
        public async Task DeleteStudentAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning("Student not found for delete: {Id}", id);
                throw new KeyNotFoundException("Student not found");
            }

            await _repo.DeleteAsync(id);

            _logger.LogInformation("Student deleted: {Id}", id);
        }
    }
}