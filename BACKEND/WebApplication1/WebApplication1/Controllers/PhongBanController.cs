using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class PhongBanController : ControllerBase
	{
		private IConfiguration _configuration;
		private readonly ILogger<PhongBanController> _logger;
		public PhongBanController(IConfiguration config, ILogger<PhongBanController> logger)
		{
			_configuration = config;
			_logger = logger;
		}


		[Route("GetAll")]
		[HttpGet]
		public JsonResult Get()
		{
			DataTable dataTable = new DataTable();
			string query = "select * from PhongBan";
			string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
			SqlDataReader sqlDataReader;

			//SqlConnection->mở kết nối tới CSDL
			//    ↓
			//SqlCommand->tạo câu lệnh SQL chạy trên kết nối
			//    ↓
			//SqlDataReader->đọc kết quả trả về từ lệnh SELECT



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
					}
				}
				return new JsonResult(dataTable);
			}
			catch (Exception ex)
			{
				return new JsonResult("Error");
			}


		}

		
	}
}
