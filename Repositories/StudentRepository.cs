using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories.Interfaces;

namespace StudentManagementSystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(IConfiguration config, ILogger<StudentRepository> logger)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        // GET ALL (ASYNC)
        public async Task<List<Student>> GetAllAsync()
        {
            var list = new List<Student>();

            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("SELECT * FROM Students", con);

                await con.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(MapStudent(reader));
                }

                _logger.LogInformation("Fetched {Count} students", list.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching students");
                throw;
            }

            return list;
        }

        // GET BY ID
        public async Task<Student?> GetByIdAsync(int id)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Students WHERE Id = @Id", con);

                cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                await con.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapStudent(reader);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student with Id {Id}", id);
                throw;
            }
        }

        // ADD
        public async Task AddAsync(Student student)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Students (Name, Email, Age, Course) 
                      VALUES (@Name, @Email, @Age, @Course)", con);

                cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value = student.Name;
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 100).Value = student.Email;
                cmd.Parameters.Add("@Age", System.Data.SqlDbType.Int).Value = student.Age;
                cmd.Parameters.Add("@Course", System.Data.SqlDbType.NVarChar, 100).Value = student.Course;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                _logger.LogInformation("Student added: {Name}", student.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding student");
                throw;
            }
        }

        // UPDATE
        public async Task UpdateAsync(Student student)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(
                    @"UPDATE Students 
                      SET Name=@Name, Email=@Email, Age=@Age, Course=@Course 
                      WHERE Id=@Id", con);

                cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = student.Id;
                cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value = student.Name;
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 100).Value = student.Email;
                cmd.Parameters.Add("@Age", System.Data.SqlDbType.Int).Value = student.Age;
                cmd.Parameters.Add("@Course", System.Data.SqlDbType.NVarChar, 100).Value = student.Course;

                await con.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();

                if (rows == 0)
                    throw new KeyNotFoundException("Student not found");

                _logger.LogInformation("Student updated: {Id}", student.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student");
                throw;
            }
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Students WHERE Id=@Id", con);

                cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                await con.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();

                if (rows == 0)
                    throw new KeyNotFoundException("Student not found");

                _logger.LogInformation("Student deleted: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student");
                throw;
            }
        }

        // Common Mapper
        private Student MapStudent(SqlDataReader reader)
        {
            return new Student
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Email = reader["Email"].ToString(),
                Age = (int)reader["Age"],
                Course = reader["Course"].ToString(),
                CreatedDate = (DateTime)reader["CreatedDate"]
            };
        }
    }
}