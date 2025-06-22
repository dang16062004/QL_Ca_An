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
				if (donDK.CaAn == 1)
					hanDangKy = DateTime.Today.AddHours(9);
				else if (donDK.CaAn == 2)
					hanDangKy = DateTime.Today.AddHours(15);
				else if (donDK.CaAn == 3)
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

		//[Route("Update")]
		//[HttpPut]
		//public JsonResult Update(DonDK donDK)
		//{
		//	try
		//	{
		//		using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
		//		conn.Open();

		//		// Lấy QDK từ bảng NhanVien
		//		string getQdkQuery = "SELECT QDK FROM NhanVien WHERE ID_NhanVien = @ID_NhanVien";
		//		using SqlCommand getQdkCmd = new SqlCommand(getQdkQuery, conn);
		//		getQdkCmd.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
		//		string qdk = getQdkCmd.ExecuteScalar()?.ToString();

		//		if (qdk == null)
		//			return new JsonResult("Không tìm thấy nhân viên");

		//		// Kiểm tra điều kiện LoaiDK theo QDK
		//		if (qdk == "CANHAN" && donDK.LoaiDK == "TAPTHE")

		//		{
		//			return new JsonResult("Nhân viên chỉ được đăng ký loại đăng ký phù hợp với QDK của họ");
		//		}

		//		DateTime hanDangKy;
		//		if (donDK.CaAn == "1")
		//			hanDangKy = DateTime.Today.AddHours(9);
		//		else if (donDK.CaAn == "2")
		//			hanDangKy = DateTime.Today.AddHours(15);
		//		else if (donDK.CaAn == "3")
		//			hanDangKy = DateTime.Today.AddHours(21);
		//		else
		//			return new JsonResult("Ca ăn không hợp lệ");

		//		if (DateTime.Now > hanDangKy)
		//			return new JsonResult("Đã quá thời gian sửa đơn đăng ký");

		//		string query = "UPDATE DonDK SET LoaiDK=@LoaiDK, ID_NhanVien=@ID_NhanVien, CaAn=@CaAn WHERE ID_DonDK=@ID_DonDK";

		//		using SqlCommand cmd = new SqlCommand(query, conn);
		//		cmd.Parameters.AddWithValue("@ID_DonDK", donDK.ID_DonDK);
		//		cmd.Parameters.AddWithValue("@LoaiDK", donDK.LoaiDK);
		//		cmd.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
		//		cmd.Parameters.AddWithValue("@CaAn", donDK.CaAn);
		//		cmd.ExecuteNonQuery();
		//		return new JsonResult("Update Success");
		//	}
		//	catch (Exception ex)
		//	{
		//		return new JsonResult(ex.Message);
		//	}
		//}
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



		[Route("InsertOnly")]
		[HttpPost]
		public JsonResult InsertOnly(DonCaNhanRequest request)
		{
			try
			{
				using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
				conn.Open();

				// 1. Lấy ID_NhanVien và HoVaTen từ TenDangNhap
				string getNVQuery = @"
					SELECT ID_NhanVien, HoVaTen 
					FROM NhanVien 
					WHERE TenDangNhap = @TenDangNhap";
				using SqlCommand cmdNV = new SqlCommand(getNVQuery, conn);
				cmdNV.Parameters.AddWithValue("@TenDangNhap", request.TenDangNhap);

				string idNhanVien = null;
				string hoVaTen = null;
				using (SqlDataReader reader = cmdNV.ExecuteReader())
				{
					if (!reader.Read())
						return new JsonResult("❌ Không tìm thấy nhân viên");

					idNhanVien = reader["ID_NhanVien"].ToString();
					hoVaTen = reader["HoVaTen"].ToString();
				}

				// 2. Kiểm tra loại đăng ký
				if (request.LoaiDK?.ToUpper() != "CANHAN")
					return new JsonResult("❌ Chỉ được phép đăng ký loại 'CANHAN'");
				// 2.1 Kiểm tra hạn đăng ký
				DateTime hanDangKy;
				if (request.CaAn == 1)
					hanDangKy = DateTime.Today.AddHours(9);
				else if (request.CaAn == 2)
					hanDangKy = DateTime.Today.AddHours(15);
				else if (request.CaAn == 3)
					hanDangKy = DateTime.Today.AddHours(21);
				else
					return new JsonResult("❌ Ca ăn không hợp lệ");

				if (DateTime.Now > hanDangKy)
					return new JsonResult("❌ Đã quá hạn đăng ký ca ăn");
				// 3. Insert vào bảng DonDK (NgayDK được SQL tự cập nhật)
				string insertDonQuery = @"
			INSERT INTO DonDK (LoaiDK, ID_NhanVien, CaAn)
			VALUES (@LoaiDK, @ID_NhanVien, @CaAn)";
				using (SqlCommand cmdInsert = new SqlCommand(insertDonQuery, conn))
				{
					cmdInsert.Parameters.AddWithValue("@LoaiDK", request.LoaiDK);
					cmdInsert.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
					cmdInsert.Parameters.AddWithValue("@CaAn", request.CaAn);
					cmdInsert.ExecuteNonQuery();
				}

				// 4. Truy vấn lại ID_DonDK vừa tạo gần nhất (dựa trên nhân viên và ca ăn)
				string getIDQuery = @"
			SELECT TOP 1 ID_DonDK 
			FROM DonDK 
			WHERE ID_NhanVien = @ID_NhanVien 
			  AND LoaiDK = @LoaiDK 
			  AND CaAn = @CaAn
			ORDER BY NgayDK DESC";
				string idDonDK = null;
				using (SqlCommand cmdGetID = new SqlCommand(getIDQuery, conn))
				{
					cmdGetID.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
					cmdGetID.Parameters.AddWithValue("@LoaiDK", request.LoaiDK);
					cmdGetID.Parameters.AddWithValue("@CaAn", request.CaAn);

					object result = cmdGetID.ExecuteScalar();
					if (result == null)
						return new JsonResult("❌ Không thể lấy ID_DonDK vừa tạo");
					idDonDK = result.ToString();
				}

				// 5. Insert vào bảng ChiTietDonDK
				string insertCTQuery = @"
			INSERT INTO ChiTietDonDK (SoLuong, ID_NhanVien, TrangThai, ID_DonDK)
			VALUES (@SoLuong, @ID_NhanVien, @TrangThai, @ID_DonDK)";
				using (SqlCommand cmdCT = new SqlCommand(insertCTQuery, conn))
				{
					cmdCT.Parameters.AddWithValue("@SoLuong", request.SoLuong);
					cmdCT.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
					cmdCT.Parameters.AddWithValue("@TrangThai", "WAIT");
					cmdCT.Parameters.AddWithValue("@ID_DonDK", idDonDK);
					cmdCT.ExecuteNonQuery();
				}

				return new JsonResult($"✅ Đăng ký thành công cho nhân viên: {hoVaTen}");
			}
			catch (Exception ex)
			{
				return new JsonResult("❌ Lỗi: " + ex.Message);
			}
		}

		[Route("InsertFull")]
		[HttpPost]
		public JsonResult InsertFull([FromBody] DonFullRequest request)
		{
			using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
			conn.Open();
			using SqlTransaction transaction = conn.BeginTransaction();

			try
			{
				// 0. Lấy ID_NhanVien và HoVaTen, QDK từ TenDangNhap
				string getNVQuery = @"
					SELECT ID_NhanVien, HoVaTen, QDK
					FROM NhanVien 
					WHERE TenDangNhap = @TenDangNhap";
				using SqlCommand cmdNV = new SqlCommand(getNVQuery, conn, transaction);
				cmdNV.Parameters.AddWithValue("@TenDangNhap", request.TenDangNhap);

				string idNhanVien = null;
				string hoVaTen = null;
				string qdk = null;

				using (SqlDataReader reader = cmdNV.ExecuteReader())
				{
					if (!reader.Read())
						return new JsonResult("❌ Không tìm thấy nhân viên");

					idNhanVien = reader["ID_NhanVien"].ToString();
					hoVaTen = reader["HoVaTen"].ToString();
					qdk = reader["QDK"].ToString();

					if (qdk == "CANHAN")
						return new JsonResult("❌ Nhân viên này chỉ có quyền đăng ký là CANHAN");
				}

				// 1. Kiểm tra loại đăng ký
				if (request.donDK.LoaiDK?.ToUpper() != "TAPTHE")
					return new JsonResult("❌ Chỉ được phép đăng ký loại 'TAPTHE'");

				// 2. Thêm đơn đăng ký
				string queryDon = @"
					INSERT INTO DonDK (LoaiDK, CaAn, ID_NhanVien)
					VALUES (@LoaiDK, @CaAn, @ID_NhanVien)";
				using SqlCommand cmdDon = new SqlCommand(queryDon, conn, transaction);
				cmdDon.Parameters.AddWithValue("@LoaiDK", "TAPTHE");
				cmdDon.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);
				cmdDon.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
				cmdDon.ExecuteNonQuery();


				// 3. Truy vấn lại ID_DonDK vừa tạo gần nhất
				string getIDQuery = @"
					SELECT TOP 1 ID_DonDK 
					FROM DonDK 
					WHERE ID_NhanVien = @ID_NhanVien 
					  AND LoaiDK = @LoaiDK 
					  AND CaAn = @CaAn
					ORDER BY NgayDK DESC";
				string idDonDK = null;
				using (SqlCommand cmdGetID = new SqlCommand(getIDQuery, conn, transaction))
				{
					cmdGetID.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
					cmdGetID.Parameters.AddWithValue("@LoaiDK", "TAPTHE");
					cmdGetID.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);

					object result = cmdGetID.ExecuteScalar();
					if (result == null)
						return new JsonResult("❌ Không thể lấy ID_DonDK vừa tạo");
					idDonDK = result.ToString();
				}
				DateTime hanDangKy;
				if (request.donDK.CaAn == 1)
					hanDangKy = DateTime.Today.AddHours(9);
				else if (request.donDK.CaAn == 2)
					hanDangKy = DateTime.Today.AddHours(15);
				else if (request.donDK.CaAn == 3)
					hanDangKy = DateTime.Today.AddHours(21);
				else
					return new JsonResult("❌ Ca ăn không hợp lệ");

				if (DateTime.Now > hanDangKy)
					return new JsonResult("❌ Đã quá hạn đăng ký ca ăn");
				// 4. Thêm các chi tiết đơn
				foreach (var ct in request.listChiTiet)
				{
					string queryCT = @"
				INSERT INTO ChiTietDonDK (ID_DonDK, ID_NhanVien, SoLuong, TrangThai)
				VALUES (@ID_DonDK, @ID_NhanVien, @SoLuong, @TrangThai)";
					using SqlCommand cmdCT = new SqlCommand(queryCT, conn, transaction);
					cmdCT.Parameters.AddWithValue("@ID_DonDK", idDonDK);
					cmdCT.Parameters.AddWithValue("@ID_NhanVien", ct.ID_NhanVien);
					cmdCT.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
					cmdCT.Parameters.AddWithValue("@TrangThai", "WAIT");
					cmdCT.ExecuteNonQuery();
				}
				// 0.1: Kiểm tra tất cả nhân viên có cùng phòng ban
				List<string> danhSachPhong = new List<string>();

				foreach (var ct in request.listChiTiet)
				{
					string getPhongQuery = "SELECT ID_Phong FROM NhanVien WHERE ID_NhanVien = @ID";
					using SqlCommand cmdPhong = new SqlCommand(getPhongQuery, conn, transaction);
					cmdPhong.Parameters.AddWithValue("@ID", ct.ID_NhanVien);

					object result = cmdPhong.ExecuteScalar();
					if (result == null)
						return new JsonResult($"❌ Không tìm thấy nhân viên có ID: {ct.ID_NhanVien}");

					danhSachPhong.Add(result.ToString());
				}

				if (danhSachPhong.Distinct().Count() > 1)
					return new JsonResult("❌ Tất cả nhân viên trong danh sách phải thuộc cùng một phòng ban.");





				transaction.Commit();
				return new JsonResult("✅ Đã thêm đơn và chi tiết thành công");
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return new JsonResult("❌ Lỗi: " + ex.Message);
			}
		}

		[Route("ChiTietDon")]
		[HttpGet]
		public JsonResult LayChiTietDonTheoID(string idDon)
		{
			try
			{
				using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
				conn.Open();

				// 1. Lấy thông tin loại đăng ký
				string loaiDK = "";
				string queryLoai = "SELECT LoaiDK FROM DonDK WHERE ID_DonDK = @ID_DonDK";
				using (SqlCommand cmd = new SqlCommand(queryLoai, conn))
				{
					cmd.Parameters.AddWithValue("@ID_DonDK", idDon);
					object result = cmd.ExecuteScalar();
					if (result == null)
						return new JsonResult("❌ Không tìm thấy đơn đăng ký");
					loaiDK = result.ToString();
				}

				// 2. Truy vấn chi tiết
				if (loaiDK.Trim().ToUpper() == "CANHAN")
				{
					string query = @"
				SELECT d.ID_DonDK, d.NgayDK, d.CaAn, nv.HoVaTen, ct.SoLuong, ct.TrangThai
				FROM DonDK d
				JOIN ChiTietDonDK ct ON d.ID_DonDK = ct.ID_DonDK
				JOIN NhanVien nv ON nv.ID_NhanVien = d.ID_NhanVien
				WHERE d.ID_DonDK = @ID_DonDK";

					using SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@ID_DonDK", idDon);

					using SqlDataReader reader = cmd.ExecuteReader();
					if (!reader.Read())
						return new JsonResult("❌ Không có chi tiết đơn");

					return new JsonResult(new
					{
						ID_DonDK = reader["ID_DonDK"],
						NgayDK = Convert.ToDateTime(reader["NgayDK"]).ToString("dd/MM/yyyy"),
						CaAn = reader["CaAn"],
						HoVaTen = reader["HoVaTen"],
						SoLuong = reader["SoLuong"],
						TrangThai = reader["TrangThai"],
						LoaiDK = "CANHAN"
					});
				}
				else if (loaiDK.Trim().ToUpper() == "TAPTHE")
				{
					string query = @"
				SELECT d.ID_DonDK, d.NgayDK, d.CaAn, nv.HoVaTen, ct.SoLuong, ct.TrangThai
				FROM DonDK d
				JOIN ChiTietDonDK ct ON d.ID_DonDK = ct.ID_DonDK
				JOIN NhanVien nv ON nv.ID_NhanVien = ct.ID_NhanVien
				WHERE d.ID_DonDK = @ID_DonDK";

					using SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@ID_DonDK", idDon);

					DataTable table = new DataTable();
					using SqlDataReader reader = cmd.ExecuteReader();
					table.Load(reader);

					return new JsonResult(new
					{
						LoaiDK = "TAPTHE",
						DanhSachChiTiet = table
					});
				}
				else
				{
					return new JsonResult("❌ Loại đăng ký không hợp lệ");
				}
			}
			catch (Exception ex)
			{
				return new JsonResult("❌ Lỗi: " + ex.Message);
			}
		}
		[Route("UpdateDonOnly")]
		[HttpPut]
		public JsonResult UpdateDonOnly(DonCaNhanRequest request, string iD)
		{

			DataTable dataTable = new DataTable();
			SqlDataReader dataReader;
			try
			{
				using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					conn.Open();
					// 1. Lấy ID_NhanVien và HoVaTen từ TenDangNhap
					string getNVQuery = @"
					SELECT ID_NhanVien, HoVaTen 
					FROM NhanVien 
					WHERE TenDangNhap = @TenDangNhap";
					using SqlCommand cmdNV = new SqlCommand(getNVQuery, conn);
					cmdNV.Parameters.AddWithValue("@TenDangNhap", request.TenDangNhap);

					string idNhanVien = null;
					string hoVaTen = null;
					using (SqlDataReader reader = cmdNV.ExecuteReader())
					{
						if (!reader.Read())
							return new JsonResult("❌ Không tìm thấy nhân viên");

						idNhanVien = reader["ID_NhanVien"].ToString();
						hoVaTen = reader["HoVaTen"].ToString();
					}

					// 2. Kiểm tra loại đăng ký
					if (request.LoaiDK?.ToUpper() != "CANHAN")
						return new JsonResult("❌ Chỉ được phép sửa loại 'CANHAN'");
					// 2.1 Kiểm tra hạn đăng ký
					DateTime hanDangKy;
					if (request.CaAn == 1)
						hanDangKy = DateTime.Today.AddHours(9);
					else if (request.CaAn == 2)
						hanDangKy = DateTime.Today.AddHours(15);
					else if (request.CaAn == 3)
						hanDangKy = DateTime.Today.AddHours(21);
					else
						return new JsonResult("❌ Ca ăn không hợp lệ");

					if (DateTime.Now > hanDangKy)
						return new JsonResult("❌ Đã quá hạn đăng ký ca ăn");
					string queryUp = @"BEGIN TRANSACTION;

					-- Cập nhật bảng 1 dựa trên bảng 2
					UPDATE dk 
					SET dk.CaAn = @CaAn
					FROM DonDK dk
					JOIN ChiTietDonDK ct  ON ct.ID_DonDK = dk.ID_DonDK
					JOIN NhanVien nv  ON nv.ID_NhanVien = dk.ID_NhanVien
					WHERE dk.ID_DonDK=@ID_DonDK and nv.ID_NhanVien =@ID_NhanVien;

					-- Cập nhật bảng 2 dựa trên bảng 1   ::::EROR: CHƯA LOAD VÀO BẢNG CHITIETDONDK
					UPDATE ct 
					SET ct.SoLuong = @Soluong
					FROM ChiTietDonDK ct
					JOIN DonDK dk ON ct.ID_DonDK = dk.ID_DonDK
					JOIN NhanVien nv  ON nv.ID_NhanVien = ct.ID_NhanVien
					WHERE dk.ID_DonDK=@ID_DonDK and nv.ID_NhanVien =@ID_NhanVien;

					COMMIT;

						";
					using (SqlCommand sqlCommand = new SqlCommand(queryUp, conn))
					{
						sqlCommand.Parameters.AddWithValue("@CaAn", request.CaAn);
						sqlCommand.Parameters.AddWithValue("@Soluong", request.SoLuong);
						sqlCommand.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
						sqlCommand.Parameters.AddWithValue("@ID_DonDK", iD);
						dataReader = sqlCommand.ExecuteReader();
						dataTable.Load(dataReader);

					}

					dataReader.Close();
					conn.Close();
				}
				return new JsonResult("Update success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.ToString());
			}
		}
		[Route("UpdateFull")]
		[HttpPut]
		public JsonResult UpdateFull([FromBody] DonFullRequest request, string Id_DonDK)
		{
			using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));
			conn.Open();
			using SqlTransaction transaction = conn.BeginTransaction();

			try
			{
				// 0. Lấy ID_NhanVien và HoVaTen, QDK từ TenDangNhap
				string getNVQuery = @"
					SELECT ID_NhanVien, HoVaTen, QDK
					FROM NhanVien 
					WHERE TenDangNhap = @TenDangNhap";
				using SqlCommand cmdNV = new SqlCommand(getNVQuery, conn, transaction);
				cmdNV.Parameters.AddWithValue("@TenDangNhap", request.TenDangNhap);

				string idNhanVien = null;
				string hoVaTen = null;
				string qdk = null;

				using (SqlDataReader reader = cmdNV.ExecuteReader())
				{
					if (!reader.Read())
						return new JsonResult("❌ Không tìm thấy nhân viên");

					idNhanVien = reader["ID_NhanVien"].ToString();
					hoVaTen = reader["HoVaTen"].ToString();
					qdk = reader["QDK"].ToString();

					if (qdk == "CANHAN")
						return new JsonResult("❌ Nhân viên này chỉ có quyền đăng ký là CANHAN");
				}

				// 1. Kiểm tra loại đăng ký
				if (request.donDK.LoaiDK?.ToUpper() != "TAPTHE")
					return new JsonResult("❌ Chỉ được phép đăng ký loại 'TAPTHE'");
				// 0.1: Kiểm tra tất cả nhân viên có cùng phòng ban
				List<string> danhSachPhong = new List<string>();

				foreach (var ct in request.listChiTiet)
				{
					string getPhongQuery = "SELECT ID_Phong FROM NhanVien WHERE ID_NhanVien = @ID";
					using SqlCommand cmdPhong = new SqlCommand(getPhongQuery, conn, transaction	);
					cmdPhong.Parameters.AddWithValue("@ID", ct.ID_NhanVien);

					object result = cmdPhong.ExecuteScalar();
					if (result == null)
						return new JsonResult($"❌ Không tìm thấy nhân viên có ID: {ct.ID_NhanVien}");

					danhSachPhong.Add(result.ToString());
				}

				if (danhSachPhong.Distinct().Count() > 1)
					return new JsonResult("❌ Tất cả nhân viên trong danh sách phải thuộc cùng một phòng ban.");
				// 2. Sửa đơn đăng ký
				string queryDon = @"
					Update  dk set dk.CaAn=@CaAn 
					FROM DonDK dk
					JOIN NhanVien nv on nv.ID_NhanVien = dk.ID_NhanVien
					where dk.ID_DonDK = @ID_DonDK and dk.ID_NhanVien = @ID_NhanVien";
				using SqlCommand cmdDon = new SqlCommand(queryDon, conn, transaction);
				//cmdDon.Parameters.AddWithValue("@LoaiDK", "TAPTHE");
				cmdDon.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);
				cmdDon.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
				cmdDon.Parameters.AddWithValue("@ID_DonDK", Id_DonDK);
				cmdDon.ExecuteNonQuery();



				DateTime hanDangKy;
				if (request.donDK.CaAn == 1)
					hanDangKy = DateTime.Today.AddHours(9);
				else if (request.donDK.CaAn == 2)
					hanDangKy = DateTime.Today.AddHours(15);
				else if (request.donDK.CaAn == 3)
					hanDangKy = DateTime.Today.AddHours(21);
				else
					return new JsonResult("❌ Ca ăn không hợp lệ");

				if (DateTime.Now > hanDangKy)
					return new JsonResult("❌ Đã quá hạn sửa ca ăn");
				// 4. Sửa các chi tiết đơn
				foreach (var ct in request.listChiTiet)
				{
					string queryCT = @"
					Update  ct set ct.SoLuong=@SoLuong 
					FROM ChiTietDonDK ct
					JOIN DonDK dk on dk.ID_DonDK = ct.ID_DonDK
					JOIN NhanVien nv on nv.ID_NhanVien = dk.ID_NhanVien
					where ct.ID_DonDK = @ID_DonDK and ct.ID_NhanVien = @ID_NhanVien
				";
					using SqlCommand cmdCT = new SqlCommand(queryCT, conn, transaction);
					cmdCT.Parameters.AddWithValue("@ID_DonDK", Id_DonDK);
					cmdCT.Parameters.AddWithValue("@ID_NhanVien", ct.ID_NhanVien);
					cmdCT.Parameters.AddWithValue("@SoLuong", ct.SoLuong);

					cmdCT.ExecuteNonQuery();
				}

				transaction.Commit();

				return new JsonResult("✅ Đã sửa đơn và chi tiết thành công");
			}
			catch (Exception ex)
			{
				transaction.Rollback();

				return new JsonResult("❌ Lỗi: " + ex.Message);
			}
		}






	}
}
