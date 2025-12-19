using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Oritso_Assignment_Abhishek_.Models;
namespace Oritso_Assignment_Abhishek_.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpGet]
        public IActionResult Index(string? title, string? status, string? createdByName, string? remarks, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Title", title);
                    param.Add("@Status", status);
                    param.Add("@CreatedByName", createdByName);
                    param.Add("@Remarks", remarks);
                    param.Add("@FromDate", fromDate);
                    param.Add("@ToDate", toDate);

                    var res = connection.Query<Tasks>(
                        "sp_getTasks",
                        param,
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    return View(res);
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong while fetching Tasks.";
                return View(new List<Tasks>());
            }
        }

        [HttpPost]
        public IActionResult CreateTask(Tasks data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid data. Please check inputs.";
                    return View();
                }
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Title", data.Title);
                    parameters.Add("Status", data.Status);
                    parameters.Add("Description", data.Description);
                    parameters.Add("Remarks", data.Remarks);
                    parameters.Add("DueDate", data.DueDate);
                    parameters.Add("CreatedById", "Admin-1");
                    parameters.Add("CreatedByName", "Admin");

                    parameters.Add("@TaskId", dbType: DbType.String, size: 40, direction: ParameterDirection.Output);


                    con.Execute(
                        "sp_createTask",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    //OutPut
                    string newTaskId = parameters.Get<string>("@TaskId");

                    TempData["SuccessMessage"] = $"Task created successfully.";
                    TempData["ClearForm"] = true;
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while creating the task.";
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult GetTaskById(string id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("id", id);
                    var res = con.QueryFirstOrDefault<Tasks>("sp_getTaskById", parameter, commandType: CommandType.StoredProcedure);
                    if (res == null)
                    {
                        NotFound();
                    }
                    return View("CreateTask", res);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while fetching the task.";
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult UpdateTask(Tasks data)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(data);
                }
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Id", data.Id);
                    parameters.Add("Title", data.Title);
                    parameters.Add("Status", data.Status);
                    parameters.Add("Description", data.Description);
                    parameters.Add("Remarks", data.Remarks);
                    parameters.Add("DueDate", data.DueDate);
                    parameters.Add("CreatedById", "Admin-1");
                    parameters.Add("CreatedByName", "Admin");
                    parameters.Add("CreatedOn", data.CreatedOn);

                    parameters.Add("@TaskId", dbType: DbType.String, size: 40, direction: ParameterDirection.Output);


                    con.Execute(
                        "sp_updateTask",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    //OutPut
                    string updatedTaskId = parameters.Get<string>("@TaskId");

                    TempData["SuccessMessage"] =
                  $"Task updated successfully.";

                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while updating the task.";
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult DeleteTask(string id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Id", id);
                    parameters.Add("IsActive", false);  
                    parameters.Add("@TaskId", dbType: DbType.String, size: 40, direction: ParameterDirection.Output);

                    con.Execute("sp_deleteTask", parameters, commandType: CommandType.StoredProcedure);

                    string deletedTaskId = parameters.Get<string>("@TaskId");
                    TempData["SuccessMessage"] = $"Task deleted successfully.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while deleting the task.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult CreateTask()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
