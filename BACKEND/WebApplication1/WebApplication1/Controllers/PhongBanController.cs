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

		//[Route("Insert")]
		//[HttpPost]
		//public JsonResult Insert(PhongBan phongBan)
		//{
		//	//string ssqlConnection = _configuration.GetConnectionString("QLCaAn");

		//	DataTable dataTable = new DataTable();
		//	string sqlDataSource = _configuration.GetConnectionString("QlCaAn");
		//	SqlDataReader sqlDataReader;
		//	string query = "Insert into PhongBan values" + "('" + phongBan.ID_Phong + "' , N'" + phongBan.TenPhong + "')";


		//	try
		//	{
		//		using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
		//		{
		//			sqlConnection.Open();
		//			using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
		//			{
		//				sqlDataReader = sqlCommand.ExecuteReader();
		//				dataTable.Load(sqlDataReader);
		//				sqlDataReader.Close();
		//				sqlConnection.Close();
		//			}
		//		}
		//		return new JsonResult("Success Insert");

		//	}
		//	catch (Exception ex)
		//	{
		//		return new JsonResult("Lỗi ko insert đc");
		//	}


		//}

		//[HttpDelete("Delete/{maPhong}")]
		//public IActionResult Delete(string maPhong)
		//{
		//	const string query = "DELETE FROM PhongBan WHERE ID_Phong = @ID_Phong";
		//	var connString = _configuration.GetConnectionString("QLCaAn");

		//	try
		//	{
		//		using var conn = new SqlConnection(connString);
		//		using var cmd = new SqlCommand(query, conn);
		//		cmd.Parameters.AddWithValue("@ID_Phong", maPhong);
		//		conn.Open();

		//		int rows = cmd.ExecuteNonQuery();
		//		if (rows > 0)
		//		{
		//			// Trả về JsonResult chứa object
		//			return new JsonResult(new
		//			{
		//				success = true,
		//				message = "Xóa phòng ban thành công"
		//			});
		//		}
		//		else
		//		{
		//			return new JsonResult(new
		//			{
		//				success = false,
		//				message = $"Không tìm thấy phòng ban {maPhong}"
		//			});
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, "Lỗi khi xóa phòng {MaPhong}", maPhong);
		//		return StatusCode(500, new
		//		{
		//			success = false,
		//			message = "Lỗi server: " + ex.Message
		//		});
		//	}
		//}

		//[HttpPut]

		//[Route("Update")]
		//public JsonResult Update(PhongBan phongBan)
		//{
		//	DataTable dataTable = new DataTable();
		//	string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
		//	string query = "UPDATE PhongBan SET TenPhong = @TenPhong WHERE ID_Phong = @ID_Phong";
		//	SqlDataReader sqlDataReader;


		//	try
		//	{
		//		using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
		//		{
		//			sqlConnection.Open();
		//			using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
		//			{
		//				sqlCommand.Parameters.AddWithValue("@TenPhong", phongBan.TenPhong);
		//				sqlCommand.Parameters.AddWithValue("@ID_Phong", phongBan.ID_Phong);
		//				sqlDataReader = sqlCommand.ExecuteReader();
		//				dataTable.Load(sqlDataReader);
		//				sqlDataReader.Close();
		//				sqlConnection.Close();
		//				return new JsonResult("Update Success");
		//			}
		//		}

		//	}
		//	catch (Exception ex)
		//	{
		//		return new JsonResult("Error");
		//	}
		//}
	}
}
