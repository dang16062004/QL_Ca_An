using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DonDKController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public DonDKController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[Route("GetAll")]
		[HttpGet]
		public JsonResult Get()
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");
				string query = "select  dk.ID_DonDK, dk.NgayDK ,dk.LoaiDK,nv.HoVaTen,dk.CaAn from DonDK dk " +
					" join NhanVien nv on nv.ID_NhanVien = dk.ID_NhanVien";
				SqlDataReader sqlDataReader;

				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
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
				return new JsonResult(ex.ToString());
			}
		}
		[Route("Insert")]
		[HttpPost]
		public JsonResult Insert(DonDK donDK)
		{
			try
			{
				using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
				conn.Open();

				string insertQuery = "INSERT INTO DonDK(ID_DonDK, NgayDK, LoaiDK, ID_NhanVien, CaAn) VALUES (@ID_DonDK, GETDATE(), @LoaiDK, @ID_NhanVien, @CaAn)";


				using SqlCommand cmd = new SqlCommand(insertQuery, conn);
				cmd.Parameters.AddWithValue("@ID_DonDK", donDK.ID_DonDK);
				cmd.Parameters.AddWithValue("@LoaiDK", donDK.LoaiDK);
				cmd.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
				cmd.Parameters.AddWithValue("@CaAn", donDK.CaAn);
			

				// Lấy QDK từ bảng NhanVien
				string getQdkQuery = "SELECT QDK FROM NhanVien WHERE ID_NhanVien = @ID_NhanVien";
				using SqlCommand getQdkCmd = new SqlCommand(getQdkQuery, conn);
				getQdkCmd.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
				string qdk = getQdkCmd.ExecuteScalar()?.ToString();

				if (qdk == null)
					return new JsonResult("Không tìm thấy nhân viên");

				// Kiểm tra điều kiện LoaiDK theo QDK
				if (qdk == "CANHAN" && donDK.LoaiDK == "TAPTHE")

				{
					return new JsonResult("Nhân viên chỉ được đăng ký loại đăng ký phù hợp với QDK của họ");
				}

				DateTime hanDangKy;
				if (donDK.CaAn == "1")
					hanDangKy = DateTime.Today.AddHours(9);
				else if (donDK.CaAn == "2")
					hanDangKy = DateTime.Today.AddHours(15);
				else if (donDK.CaAn == "3")
					hanDangKy = DateTime.Today.AddHours(21);
				else
					return new JsonResult("Ca ăn không hợp lệ");

				if (DateTime.Now > hanDangKy)
					return new JsonResult("Đã quá hạn đăng ký ca ăn");

				cmd.ExecuteNonQuery();
				return new JsonResult("Insert Success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.Message);
			}
		}

		[Route("Update")]
		[HttpPut]
		public JsonResult Update(DonDK donDK)
		{
			try
			{
				using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
				conn.Open();

				// Lấy QDK từ bảng NhanVien
				string getQdkQuery = "SELECT QDK FROM NhanVien WHERE ID_NhanVien = @ID_NhanVien";
				using SqlCommand getQdkCmd = new SqlCommand(getQdkQuery, conn);
				getQdkCmd.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
				string qdk = getQdkCmd.ExecuteScalar()?.ToString();

				if (qdk == null)
					return new JsonResult("Không tìm thấy nhân viên");

				// Kiểm tra điều kiện LoaiDK theo QDK
				if (qdk == "CANHAN" && donDK.LoaiDK == "TAPTHE")

				{
					return new JsonResult("Nhân viên chỉ được đăng ký loại đăng ký phù hợp với QDK của họ");
				}

				DateTime hanDangKy;
				if (donDK.CaAn == "1")
					hanDangKy = DateTime.Today.AddHours(9);
				else if (donDK.CaAn == "2")
					hanDangKy = DateTime.Today.AddHours(15);
				else if (donDK.CaAn == "3")
					hanDangKy = DateTime.Today.AddHours(21);
				else
					return new JsonResult("Ca ăn không hợp lệ");

				if (DateTime.Now > hanDangKy)
					return new JsonResult("Đã quá thời gian sửa đơn đăng ký");

				string query = "UPDATE DonDK SET LoaiDK=@LoaiDK, ID_NhanVien=@ID_NhanVien, CaAn=@CaAn WHERE ID_DonDK=@ID_DonDK";

				using SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@ID_DonDK", donDK.ID_DonDK);
				cmd.Parameters.AddWithValue("@LoaiDK", donDK.LoaiDK);
				cmd.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
				cmd.Parameters.AddWithValue("@CaAn", donDK.CaAn);
				cmd.ExecuteNonQuery();
				return new JsonResult("Update Success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.Message);
			}
		}
		[Route("Delete/{id}")]
		[HttpDelete]
		public JsonResult Delete(string id)
		{
			try
			{
				string dataSource = _configuration.GetConnectionString("QLCaAn");
				string caAn = "", quyen = "";

				using SqlConnection conn = new SqlConnection(dataSource);
				conn.Open();

				// Lấy CaAn và PhanQuyen
				string checkQuery = @"
			SELECT CaAn, nv.PhanQuyen
			FROM DonDK d
			JOIN NhanVien nv ON d.ID_NhanVien = nv.ID_NhanVien
			WHERE d.ID_DonDK = @ID_DonDK";
				using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
				{
					checkCmd.Parameters.AddWithValue("@ID_DonDK", id);
					using (SqlDataReader reader = checkCmd.ExecuteReader())
					{
						if (!reader.Read())
							return new JsonResult("Không tìm thấy đơn đăng ký");

						caAn = reader["CaAn"].ToString();
						quyen = reader["PhanQuyen"].ToString();
					}
				}

				// Kiểm tra quyền
				if (caAn == "3" && quyen != "ADMIN")
					return new JsonResult("Chỉ ADMIN mới có quyền xóa ca ăn 3");

				// Thực hiện xóa
				string deleteQuery = "DELETE FROM DonDK WHERE ID_DonDK = @ID_DonDK";
				using SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
				deleteCmd.Parameters.AddWithValue("@ID_DonDK", id);
				deleteCmd.ExecuteNonQuery();

				return new JsonResult("Delete Success");
			}
			catch (Exception ex)
			{
				return new JsonResult("Lỗi: " + ex.Message);
			}
		}


	}
}
