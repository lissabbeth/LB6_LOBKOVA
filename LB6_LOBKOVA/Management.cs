using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace LB6_LOBKOVA
{
    public class Management
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public double? Budget { get; set; }
        public int? Employees { get; set; }
        public string Manager { get; set; } = string.Empty;

        public static IEnumerable<Management> GetAll()
        {
            var records = new List<Management>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MunicipalConnection"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Department, Budget, Employees, Manager FROM Management", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(new Management
                        {
                            Id = reader.GetInt32(0),
                            Department = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Budget = reader.IsDBNull(2) ? (double?)null : reader.GetDouble(2),
                            Employees = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            Manager = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        });
                    }
                }
            }
            return records;
        }

        public static IEnumerable<Management> Search(string searchCriteria)
        {
            var records = new List<Management>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MunicipalConnection"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT Id, Department, Budget, Employees, Manager " +
                    "FROM Management " +
                    "WHERE Department LIKE @Search OR Manager LIKE @Search", connection);
                command.Parameters.AddWithValue("@Search", $"%{searchCriteria}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(new Management
                        {
                            Id = reader.GetInt32(0),
                            Department = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Budget = reader.IsDBNull(2) ? (double?)null : reader.GetDouble(2),
                            Employees = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            Manager = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        });
                    }
                }
            }
            return records;
        }

        public void Insert()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MunicipalConnection"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Management (Department, Budget, Employees, Manager) VALUES (@Department, @Budget, @Employees, @Manager)",
                    connection);
                command.Parameters.AddWithValue("@Department", string.IsNullOrWhiteSpace(Department) ? (object)DBNull.Value : Department);
                command.Parameters.AddWithValue("@Budget", Budget.HasValue ? (object)Budget.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Employees", Employees.HasValue ? (object)Employees.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Manager", string.IsNullOrWhiteSpace(Manager) ? (object)DBNull.Value : Manager);
                command.ExecuteNonQuery();
            }
        }

        public void Update()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MunicipalConnection"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Management SET Department = @Department, Budget = @Budget, Employees = @Employees, Manager = @Manager WHERE Id = @Id",
                    connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Department", string.IsNullOrWhiteSpace(Department) ? (object)DBNull.Value : Department);
                command.Parameters.AddWithValue("@Budget", Budget.HasValue ? (object)Budget.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Employees", Employees.HasValue ? (object)Employees.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Manager", string.IsNullOrWhiteSpace(Manager) ? (object)DBNull.Value : Manager);
                command.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MunicipalConnection"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Management WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
