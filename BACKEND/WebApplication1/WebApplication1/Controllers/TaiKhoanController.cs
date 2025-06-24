using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaiKhoanController : ControllerBase
	{
		private IConfiguration _configuration;
		private readonly ILogger<PhongBanController> _logger;
		public TaiKhoanController(IConfiguration config, ILogger<PhongBanController> logger)
		{
			_configuration = config;
			_logger = logger;
		}

		[Route("GetAll")]
		[HttpGet]
		public JsonResult Get()
		{
			DataTable dataTable = new DataTable();
			string query = "Select * from TaiKhoan";
			string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
			SqlDataReader sqlDataReader;

			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();
						return new JsonResult(dataTable);
					}
				}

			}
			catch
			{
				return new JsonResult("NotFound");
			}
		}
	}
}
