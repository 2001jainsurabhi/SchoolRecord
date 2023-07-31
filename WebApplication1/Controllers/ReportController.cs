using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // ReportController.cs
    public class ReportController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Index(string gradeFilter)
        {
            List<ReportData> reportData = new List<ReportData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT s.StudentName, st.StandardName AS GradeLevel, sub.SubjectName AS Subject " +
                               "FROM Student s " +
                               "JOIN Standard st ON s.StandardId = st.StandardId " +
                               "LEFT JOIN Subject sub ON s.StandardId = sub.StandardId";

                if (!string.IsNullOrEmpty(gradeFilter))
                {
                    query += " WHERE st.StandardName = @gradeFilter";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(gradeFilter))
                    {
                        command.Parameters.AddWithValue("@gradeFilter", gradeFilter);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReportData data = new ReportData
                            {
                                StudentName = reader["StudentName"].ToString(),
                                GradeLevel = reader["GradeLevel"].ToString(),
                                Subject = reader["Subject"].ToString()
                            };

                            reportData.Add(data);
                        }
                    }
                }
            }

            return View(reportData);
        }
    }

}