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
		[Route("Login")]
		[HttpPost]
		public JsonResult DangNhap(TaiKhoan taiKhoan)
		{
			DataTable dataTable = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
			string query = "select count(*) from TaiKhoan where TenDangNhap=@TenDangNhap and MatKhau=@MatKhau   ";

			SqlDataReader sqlDataReader;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						
						sqlCommand.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
						sqlCommand.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
						//sqlCommand.Parameters.AddWithValue("@NgayTao", taiKhoan.NgayTao);
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
				return new JsonResult("Lỗi đăng nhập");
			}
		}
		[Route("Insert")]
		[HttpPost]
		public JsonResult Insert(TaiKhoan taiKhoan)
		{
			DataTable dataTable = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
			string query = "Insert into TaiKhoan (ID_TaiKhoan, TenDangNhap, MatKhau) VALUES (@ID_TaiKhoan, @TenDangNhap, @MatKhau)";

			SqlDataReader sqlDataReader;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@ID_TaiKhoan", taiKhoan.ID_TaiKhoan);
						sqlCommand.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
						sqlCommand.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
						//sqlCommand.Parameters.AddWithValue("@NgayTao", taiKhoan.NgayTao);
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();

					}
				}

				return new JsonResult("Success Insert");
			}
			catch (Exception ex)
			{
				return new JsonResult("Lỗi thêm mới");
			}
		}

		[HttpDelete("Delete/{maTaiKhoan}")]
		public IActionResult Delete(string maTaiKhoan)
		{
			const string query = "DELETE FROM TaiKhoan WHERE ID_TaiKhoan = @ID_TaiKhoan";
			var connString = _configuration.GetConnectionString("QLCaAn");

			try
			{
				using var conn = new SqlConnection(connString);
				using var cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@ID_TaiKhoan", maTaiKhoan);
				conn.Open();

				int rows = cmd.ExecuteNonQuery();
				if (rows > 0)
				{
					// Trả về JsonResult chứa object
					return new JsonResult(new
					{
						success = true,
						message = "Xóa  tài khoản thành công"
					});
				}
				else
				{
					return new JsonResult(new
					{
						success = false,
						message = $"Không tìm thấy  tài khoản {maTaiKhoan}"
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Lỗi khi xóa tài khoản {maTaiKhoan}", maTaiKhoan);
				return StatusCode(500, new
				{
					success = false,
					message = "Lỗi server: " + ex.Message
				});
			}
		}

		[Route("Update")]
		[HttpPut]
		public JsonResult Update(TaiKhoan taiKhoan)
		{
			string SqlDataSource = _configuration.GetConnectionString("QLCaAn");
			SqlDataReader sqlDataReader;
			string query = "Update TaiKhoan set TenDangNhap=@TenDangNhap,NgayTao=@NgayTao,MatKhau=@MatKhau where ID_TaiKhoan=@ID_TaiKhoan ";
			DataTable dataTable = new DataTable();
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(SqlDataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@ID_TaiKhoan", taiKhoan.ID_TaiKhoan);
						sqlCommand.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
						sqlCommand.Parameters.AddWithValue("@NgayTao", taiKhoan.NgayTao);
						sqlCommand.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();

					}
				}
				return new JsonResult(" Update Success");
			}
			catch (Exception ex)
			{
				return new JsonResult("Error");
			}
		}

	}
}
