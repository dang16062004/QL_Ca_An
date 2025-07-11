using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChiTietDonDKController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public ChiTietDonDKController(IConfiguration configuration)
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
				string query = "select * from ChiTietDonDK join NhanVien on NhanVien.ID_NhanVien=ChiTietDonDK.ID_NhanVien join DonDK on DonDK.ID_DonDK =ChiTietDonDK.ID_DonDK ";
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
		[HttpPost]
		public JsonResult Insert(ChiTietDonDK chiTietDonDK)
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");

				string query = @"
			DECLARE @NgayDK DATE, @CaAn NVARCHAR(10), @GioKetThuc TIME;
			SELECT @NgayDK = NgayDK, @CaAn = CaAn FROM DonDK WHERE ID_DonDK = @ID_DonDK;

			-- Giờ kết thúc tùy vào ca
			IF @CaAn = '1' SET @GioKetThuc = '09:00:00';
			ELSE IF @CaAn = '2' SET @GioKetThuc = '15:00:00';

			-- Nếu quá giờ thì không cho đăng ký
			IF GETDATE() > CAST(@NgayDK AS DATETIME) + CAST(@GioKetThuc AS DATETIME)
			BEGIN
				THROW 50000, 'Đã quá thời gian đăng ký.', 1;
			END

			INSERT INTO ChiTietDonDK(ID_ChiTietDonDK, SoLuong, ID_NhanVien, TrangThai, ID_DonDK)
			VALUES (@ID_ChiTietDonDK, @SoLuong, @ID_NhanVien, @TrangThai, @ID_DonDK);
		";

				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@ID_DonDK", chiTietDonDK.ID_DonDK);
						//sqlCommand.Parameters.AddWithValue("@TrangThai", chiTietDonDK.TrangThai);
						sqlCommand.Parameters.AddWithValue("@ID_NhanVien", chiTietDonDK.ID_NhanVien);
						sqlCommand.Parameters.AddWithValue("@SoLuong", chiTietDonDK.SoLuong);
						sqlCommand.Parameters.AddWithValue("@ID_ChiTietDonDK", chiTietDonDK.ID_ChiTietDonDK);

						sqlCommand.ExecuteNonQuery();
					}
				}
				return new JsonResult("Insert Success");
			}
			catch (Exception ex)
			{
				return new JsonResult("Error: " + ex.Message);
			}
		}
		[Route("Update")]
		[HttpPut]
		public JsonResult Update(ChiTietDonDK chiTietDonDK)
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");

				string query = @"
			DECLARE @NgayDK DATE, @CaAn NVARCHAR(10), @GioKetThuc TIME;
			SELECT @NgayDK = NgayDK, @CaAn = CaAn FROM DonDK WHERE ID_DonDK = (
				SELECT ID_DonDK FROM ChiTietDonDK WHERE ID_ChiTietDonDK = @ID_ChiTietDonDK
			);

			IF @CaAn = '1' SET @GioKetThuc = '09:00:00';
			ELSE IF @CaAn = '2' SET @GioKetThuc = '15:00:00';

			IF GETDATE() > CAST(@NgayDK AS DATETIME) + CAST(@GioKetThuc AS DATETIME)
			BEGIN
				THROW 50001, 'Đã quá thời gian cập nhật hoặc hủy.', 1;
			END

			UPDATE ChiTietDonDK 
			SET SoLuong = @SoLuong,
				ID_NhanVien = @ID_NhanVien,
				TrangThai = @TrangThai
			WHERE ID_ChiTietDonDK = @ID_ChiTietDonDK;
		";

				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						//sqlCommand.Parameters.AddWithValue("@TrangThai", chiTietDonDK.TrangThai);
						sqlCommand.Parameters.AddWithValue("@ID_NhanVien", chiTietDonDK.ID_NhanVien);
						sqlCommand.Parameters.AddWithValue("@SoLuong", chiTietDonDK.SoLuong);
						sqlCommand.Parameters.AddWithValue("@ID_ChiTietDonDK", chiTietDonDK.ID_ChiTietDonDK);

						sqlCommand.ExecuteNonQuery();
					}
				}
				return new JsonResult("Update Success");
			}
			catch (Exception ex)
			{
				return new JsonResult("Error: " + ex.Message);
			}
		}

		[Route("Huy{id}")]
		[HttpPut]
		public JsonResult HuyDon(string id)
		{
			string cancleOrder = "hủy";
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");
				string query = @"
					UPDATE ChiTietDonDK 
					SET 
						TrangThai = @TrangThai
					WHERE ID_ChiTietDonDK = @ID_ChiTietDonDK";


				SqlDataReader sqlDataReader;
				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{

						sqlCommand.Parameters.AddWithValue("@ID_ChiTietDonDK", id);
						sqlCommand.Parameters.AddWithValue("@TrangThai", cancleOrder);

						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();
					}
				}
				return new JsonResult(" Cancle order Success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.ToString());
			}
		}


		[Route("GetbyName")]
		[HttpGet]
		public JsonResult GetbyName(string name, string maphongban)
		{
			try
			{
				// Kiểm tra nếu tên không hợp lệ (rỗng, null hoặc chỉ chứa khoảng trắng)
				if (string.IsNullOrWhiteSpace(name) || name.Trim().Split(' ').Length < 2)
				{
					return new JsonResult("Vui lòng nhập đầy đủ họ và tên!");
				}
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");

				string query = @"
			SELECT 
				CONVERT(DATE, dk.NgayDK) AS Ngay,
				SUM(CASE WHEN dk.CaAn = '1' THEN ct.SoLuong ELSE 0 END) AS Ca1,
				SUM(CASE WHEN dk.CaAn = '2' THEN ct.SoLuong ELSE 0 END) AS Ca2,
				SUM(CASE WHEN dk.CaAn = '3' THEN ct.SoLuong ELSE 0 END) AS Ca3
			FROM ChiTietDonDK ct
			JOIN NhanVien nv ON nv.ID_NhanVien = ct.ID_NhanVien
			JOIN DonDK dk ON dk.ID_DonDK = ct.ID_DonDK
			JOIN PhongBan pb ON pb.ID_Phong = nv.ID_Phong
			WHERE nv.HoVaTen = @name
			  and pb.ID_Phong = @maphongban
			GROUP BY CONVERT(DATE, dk.NgayDK)
			ORDER BY Ngay";

				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@name", name);
						sqlCommand.Parameters.AddWithValue("@maphongban", maphongban);

						using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
						{
							dataTable.Load(sqlDataReader);
						}
					}
					sqlConnection.Close();
				}

				return new JsonResult(dataTable);
			}
			catch (Exception ex)
			{
				return new JsonResult("Lỗi: " + ex.Message);
			}
		}

		[HttpGet("GetbyCaAn")]
		public JsonResult GetbyCaAn(DateOnly date, int caAn)
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");

				string query = @"
			SELECT 
				nv.HoVaTen,
				ct.ID_NhanVien,
				pb.TenPhong,
				SUM(ct.SoLuong) AS SoLuong,
				SUM(ct.SoLuong) * 15000 AS ThanhTien
				FROM ChiTietDonDK ct
				JOIN DonDK dk ON dk.ID_DonDK = ct.ID_DonDK
				JOIN NhanVien nv ON nv.ID_NhanVien = ct.ID_NhanVien
				join PhongBan pb on pb.ID_Phong=nv.ID_Phong
				WHERE CONVERT(DATE, dk.NgayDK) = @NgayDK AND dk.CaAn = @CaAn
				GROUP BY nv.HoVaTen, ct.ID_NhanVien,pb.TenPhong;";

				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@NgayDK", date);
						sqlCommand.Parameters.AddWithValue("@CaAn", caAn);

						using (SqlDataReader reader = sqlCommand.ExecuteReader())
						{
							dataTable.Load(reader);
						}
					}
				}

				return new JsonResult(dataTable);
			}
			catch (Exception ex)
			{
				return new JsonResult("Lỗi: " + ex.Message);
			}
		}
		[Route("GetbyMonth")]
		[HttpGet]
		public JsonResult GetbyMonth(DateTime date)
		{
			if (date == default)
				return new JsonResult("Ngày không hợp lệ!");

			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");

				string query = @"
			
				SELECT 
				nv.HoVaTen,
				ct.ID_NhanVien,
				pb.TenPhong,
				SUM(CASE WHEN dk.CaAn = '1' THEN ct.SoLuong ELSE 0 END) AS Ca1,
				SUM(CASE WHEN dk.CaAn = '2' THEN ct.SoLuong ELSE 0 END) AS Ca2,
				SUM(CASE WHEN dk.CaAn = '3' THEN ct.SoLuong ELSE 0 END) AS Ca3,
				SUM(ct.SoLuong) AS TongSoLuong,
				SUM(ct.SoLuong) * 15000 AS ThanhTien
				FROM ChiTietDonDK ct
				JOIN DonDK dk ON dk.ID_DonDK = ct.ID_DonDK
				JOIN NhanVien nv ON nv.ID_NhanVien = ct.ID_NhanVien
				join PhongBan pb on pb.ID_Phong=nv.ID_Phong
				WHERE MONTH(dk.NgayDK) = MONTH(@NgayDK)
				 AND YEAR(dk.NgayDK) = YEAR(@NgayDK)

				GROUP BY nv.HoVaTen, ct.ID_NhanVien,pb.TenPhong;";

				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@NgayDK", date);


						using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
						{
							dataTable.Load(sqlDataReader);
						}
					}
					sqlConnection.Close();
				}

				return new JsonResult(dataTable);
			}
			catch (Exception ex)
			{
				return new JsonResult("Lỗi: " + ex.Message);
			}
		}

	}
}
