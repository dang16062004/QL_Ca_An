using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Security.Claims;
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


		[Route("DeletebyUser")]
		[Authorize(Roles ="User,Admin")]
		[HttpDelete]
		public IActionResult DeletebyUser(int iD_Don)
		{
			try
			{

				//Check trùng ID_NhanVien
				//Lấy ID_NhanVien từ JWT
				int? ID_NhanVien = (int)GetIDFromJWT();
				using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					connection.Open();
					using (SqlTransaction transaction = connection.BeginTransaction())
					{
						//lấy ID_NhanVien từ database 
						int ID_NhanVienCheck;
						string queryNhanVien = @"select ID_NhanVien from DonDK where ID_DonDK = @ID_DonDK";
						using (SqlCommand commandNhanVien = new SqlCommand(queryNhanVien, connection, transaction))
						{
							commandNhanVien.Parameters.AddWithValue("@ID_DonDK",iD_Don);
							object result = commandNhanVien.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy ID nhân viên");
							}
							ID_NhanVienCheck = (int)result;
						}
						if (ID_NhanVien != ID_NhanVienCheck)
						{
							return BadRequest("Không trùng ID_Nhan vien");
						}

						//xóa DonDK thì phải xóa ID nhân viên cũng như ID_Chi tiết nhân viên

						//Lấy ID_ChiTiet 
						int ID_ChiTiet;
						string queryChiTiet = @"select ID_ChiTietDonDK from ChiTietDonDK where ID_DonDK = @ID_DonDK";
						using (SqlCommand commandChiTiet = new SqlCommand(queryChiTiet, connection, transaction))
						{
							commandChiTiet.Parameters.AddWithValue("@ID_DonDK", iD_Don);
							object result = commandChiTiet.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy được ID_ChiTiet ");
							}
							ID_ChiTiet= (int)result;
						}

						//Check xem có xóa đc ko 
						string TrangThaiCheck;
						string queryCheck = @"select TrangThai from  DonDK where ID_DonDK = @ID_DonDK";
						using (SqlCommand commandCheck = new SqlCommand(@queryCheck, connection, transaction))
						{
							commandCheck.Parameters.AddWithValue("@ID_DonDK", iD_Don);
							object result = commandCheck.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy được Trạng thái");
							}
							TrangThaiCheck = (string)result;

						}
						if (TrangThaiCheck != TrangThaiDon.ChoXacNhan.ToString())
						{
							return BadRequest("Chỉ xóa khi Trạng thái là Chờ đăng ký");
						}

						//Xóa ChiTiet
						string queryDeleteCT = @"delete from ChiTietDonDK where ID_ChiTietDonDK=@ID_ChiTietDonDK";
						using (SqlCommand commandDeleteCT = new SqlCommand(queryDeleteCT, connection, transaction))
						{
							commandDeleteCT.Parameters.AddWithValue("@ID_ChiTietDonDK", ID_ChiTiet);
							commandDeleteCT.ExecuteNonQuery();
						}
						//Xóa DonDK
						string queryDeleteDK = @"delete from DonDK where ID_DonDK=@ID_DonDK";
						using (SqlCommand commandDeleteDK = new SqlCommand(queryDeleteDK, connection, transaction))
						{
							commandDeleteDK.Parameters.AddWithValue("@ID_DonDK", iD_Don);
							commandDeleteDK.ExecuteNonQuery();
						}
						transaction.Commit();
					}
				}

				return Ok("Delete Success");
			}
			catch (SqlException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}


		}


		//Tạo 1 function lấy ID_NhanVien từ JWT
		private int? GetIDFromJWT()
		{
			var id = User?.Claims?.FirstOrDefault(c => c.Type == "ID_NhanVien")?.Value;
			return int.TryParse(id, out int result) ? result : (int?)null;
		}


		[Route("InsertOnly")]
		[HttpPost]
		[Authorize(Roles = "User")]
		public IActionResult InsertOnly(DonCaNhanRequest request)
		{
			try
			{
			


				if (request == null)
				{
					return BadRequest("Thông tin nhập vào là rỗng");
				}

				//Lấy iD_NhanVien từ jwt
				int? id_nhanvien = GetIDFromJWT();

				//Kiểm tra xem id_NhanVien có tồn tại hay không
				if (id_nhanvien == null)
				{
					return BadRequest("thông tin nhận từ jwt là rỗng");
				}

				//Kiểm tra loại ca ăn nhập vào có đúng là CaNhan hay không
				if (request.LoaiDK.Equals("CaNhan", StringComparison.OrdinalIgnoreCase) == false)
				{
					return BadRequest("Đây là đăng kí cho cá nhân , vui lòng sửa lại LoaiDK");
				}

				//Kiểm tra hạn đăng kí theo các ca
				DateTime hanDangKy;
				if (request.CaAn == 1)
				{
					hanDangKy = DateTime.Today.AddHours(9);
				}
				else if (request.CaAn == 2)
				{
					hanDangKy = DateTime.Today.AddHours(15);
				}
				else if (request.CaAn == 3)
				{
					hanDangKy = DateTime.Today.AddHours(23);
				}
				else
				{
					return BadRequest("Ca ăn nhập không hợp lệ");

				}
				if (DateTime.Now > hanDangKy)
				{
					return BadRequest("Đã quá hạn đăng kí");
				}


				using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					connection.Open();
					using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
					{
						// Kiểm tra đã tồn tại đơn chưa
						string checkQuery = @"
							SELECT COUNT(*) 
							FROM DonDK 
							WHERE ID_NhanVien = @ID_NhanVien 
							  AND LoaiDK = @LoaiDK 
							  AND CaAn = @CaAn 
							  AND CONVERT(date, NgayDK) = CONVERT(date, GETDATE())";

						using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection, transaction))
						{
							checkCommand.Parameters.AddWithValue("@ID_NhanVien", id_nhanvien);
							checkCommand.Parameters.AddWithValue("@LoaiDK", request.LoaiDK);
							checkCommand.Parameters.AddWithValue("@CaAn", request.CaAn);

							int count = (int)checkCommand.ExecuteScalar();
							if (count > 0)
							{
								transaction.Rollback(); // rollback nếu dùng transaction
								return BadRequest("Bạn đã đặt đơn cho ca này trong ngày hôm nay.");
							}
						}

						//Insert bảng DonDK
						string queryDonDK = @"insert into DonDK(LoaiDK,CaAn,ID_NhanVien,TrangThai) values (@LoaiDK,@CaAn,@ID_NhanVien,@TrangThai)";
						using (SqlCommand commanDonDK = new SqlCommand(queryDonDK, connection, transaction))
						{
							commanDonDK.Parameters.AddWithValue("@LoaiDK", request.LoaiDK);
							commanDonDK.Parameters.AddWithValue("@ID_NhanVien", id_nhanvien);
							commanDonDK.Parameters.AddWithValue("@CaAn", request.CaAn);


							// 👇 Chèn enum dưới dạng chuỗi
							string trangThaiStr = TrangThaiDon.ChoXacNhan.ToString(); // "ChoXacNhan"
							commanDonDK.Parameters.AddWithValue("@TrangThai", trangThaiStr);

							
							commanDonDK.ExecuteNonQuery();
						}

						//Lấy ID_DonDK của DonĐK  vừa tạo (thông qua Nhân Viên , ca ăn ,ngày gần nhất và loaiDK ) để chèn vào ChiTietDonDK
						string queryIDDon = @"
							select top 1 ID_DonDK
							from DonDK
							where ID_NhanVien = @ID_NhanVien
							and LoaiDK = @LoaiDK
							and  CaAn = @CaAn
							order by NgayDK desc";
						string iD_DonDK = null;
						using (SqlCommand commandIDDon = new SqlCommand(queryIDDon, connection, transaction))
						{
							commandIDDon.Parameters.AddWithValue("@ID_NhanVien", id_nhanvien);
							commandIDDon.Parameters.AddWithValue("@LoaiDK", request.LoaiDK);
							commandIDDon.Parameters.AddWithValue("@CaAn", request.CaAn);

							object result = commandIDDon.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy lại được ID_DonDK");

							}
							iD_DonDK = result.ToString();
						}

						//Insert vào bảng ChiTietDonDK
						string queryChiTiet = @"insert into ChiTietDonDK(SoLuong,ID_NhanVien,ID_DonDK) values(@SoLuong,@ID_NhanVien,@ID_DonDK)";
						using (SqlCommand commandChiTiet = new SqlCommand(queryChiTiet, connection, transaction))
						{
							commandChiTiet.Parameters.AddWithValue("@SoLuong", request.SoLuong);
							commandChiTiet.Parameters.AddWithValue("@ID_NhanVien", id_nhanvien);
							commandChiTiet.Parameters.AddWithValue("@ID_DonDK", iD_DonDK);
							commandChiTiet.ExecuteNonQuery();
						}
						transaction.Commit();
					}
				}

				return Ok("Insert đơn cá nhân đã thành công");
			}
			catch (SqlException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}


		[Route("InsertFull")]
		[HttpPost]
		[Authorize(Roles = "TapThe")]
		public IActionResult InsertFull(DonFullRequest request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Giá trị đầu vào rỗng");
				}

				//lấy id_nhân viên đăng nhập thông qua hàm
				int? id_NhanVien = GetIDFromJWT();
				//Kiểm tra xem id có ttoofn tại hay không
				if (id_NhanVien == null)
				{
					return BadRequest("ID_Nhân viên không lấy được từ JWT");
				}


				if (request.donDK.LoaiDK.Equals("TapThe", StringComparison.OrdinalIgnoreCase) == false)
				{
					return BadRequest("Nhập sai loaiDK vì ddaay là đăng kí tập thể.Vui lòng Nhập 'TapThe' ");
				}

				//Hạn của từng ca đăng kí
				DateTime hanDangKy;
				if (request.donDK.CaAn == 1)
				{
					hanDangKy = DateTime.Today.AddHours(9);
				}
				else if (request.donDK.CaAn == 2)
				{
					hanDangKy = DateTime.Today.AddHours(15);
				}
				else if (request.donDK.CaAn == 3)
				{
					hanDangKy = DateTime.Today.AddHours(21);
				}
				else
				{
					return BadRequest("không tồn tại ca ăn này");
				}

				if (DateTime.Now > hanDangKy)
				{
					return BadRequest("Hết hạn nhập ca");
				}

				using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					connection.Open();
					using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
					{
						// Kiểm tra đã tồn tại đơn chưa
						string checkQuery = @"
								SELECT COUNT(*) 
								FROM DonDK 
								WHERE ID_NhanVien = @ID_NhanVien 
								  AND LoaiDK = @LoaiDK 
								  AND CaAn = @CaAn 
								  AND CONVERT(date, NgayDK) = CONVERT(date, GETDATE())";

						using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection, transaction))
						{
							checkCommand.Parameters.AddWithValue("@ID_NhanVien", id_NhanVien);
							checkCommand.Parameters.AddWithValue("@LoaiDK", request.donDK.LoaiDK);
							checkCommand.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);

							int count = (int)checkCommand.ExecuteScalar();
							if (count > 0)
							{
								transaction.Rollback(); // rollback nếu dùng transaction
								return BadRequest("Bạn đã đặt đơn cho ca này trong ngày hôm nay.");
							}
						}

						//Insert DonDK
						string queryDonDK = @" insert into DonDK(LoaiDK,CaAn,ID_NhanVien,TrangThai) values(@LoaiDK,@CaAn,@ID_NhanVien,@TrangThai)";
						using (SqlCommand commandDonDK = new SqlCommand(queryDonDK, connection, transaction))
						{
							commandDonDK.Parameters.AddWithValue("@LoaiDK", request.donDK.LoaiDK);
							commandDonDK.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);
							commandDonDK.Parameters.AddWithValue("@ID_NhanVien", id_NhanVien);
							commandDonDK.Parameters.AddWithValue("@TrangThai", TrangThaiDon.ChoXacNhan.ToString());
							commandDonDK.ExecuteNonQuery();
						}

						//Lấy ra id_don vừa tạo
						string queryID_Don = @"select top 1 ID_DonDK
							from DonDK
							where ID_NhanVien = @ID_NhanVien and CaAn = @CaAn and LoaiDK= @LoaiDK
							order by NgayDK desc
							";

						string id_Don;
						using (SqlCommand commandID_Don = new SqlCommand(queryID_Don, connection, transaction))
						{
							commandID_Don.Parameters.AddWithValue("@LoaiDK", request.donDK.LoaiDK);
							commandID_Don.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);
							commandID_Don.Parameters.AddWithValue("@ID_NhanVien", id_NhanVien);

							object resultDon = commandID_Don.ExecuteScalar();

							if (resultDon == null)
							{
								return BadRequest("Không lấy được resultDon");
							}
							id_Don = resultDon.ToString();

						}

						//Kiểm tra xem có lấy đc id don không
						if (id_Don == null)
						{
							return BadRequest("Không lấy được ID_DonĐK");
						}
						//Kiểm tra xem các nhân viên có cùng 1 phòng ban hay không

						var danhsachPhong = new List<string>();

						foreach (var ct in request.listChiTiet)
						{
							string queryPhong = @"select ID_Phong from NhanVien where ID_NhanVien=@ID_NhanVien";
							using (SqlCommand commandPhong = new SqlCommand(queryPhong, connection, transaction))
							{
								commandPhong.Parameters.AddWithValue("@ID_NhanVien", ct.ID_NhanVien);

								object result = commandPhong.ExecuteScalar();
								if (result == null)
								{
									return BadRequest("không có phòng abn nào có nhân viên có id là " + ct.ID_NhanVien);
								}
								danhsachPhong.Add(result.ToString());

							}
						}
						if (danhsachPhong.Distinct().Count() > 1)
						{
							return BadRequest("Có nhân viên không có chung phòng ban");
						}
						foreach (var ct in request.listChiTiet)
						{
							string queryCT = @"
										INSERT INTO ChiTietDonDK (ID_DonDK, ID_NhanVien, SoLuong)
										VALUES (@ID_DonDK, @ID_NhanVien, @SoLuong)";
							using SqlCommand cmdCT = new SqlCommand(queryCT, connection, transaction);
							cmdCT.Parameters.AddWithValue("@ID_DonDK", id_Don);
							cmdCT.Parameters.AddWithValue("@ID_NhanVien", ct.ID_NhanVien);
							cmdCT.Parameters.AddWithValue("@SoLuong", ct.SoLuong);

							cmdCT.ExecuteNonQuery();
						}

						transaction.Commit();
					}
				}

				return Ok("Đăng  kí đơn tập thể thành công");
			}
			catch (SqlException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}


		[Route("ChiTietDon")]
		[HttpGet]
		[Authorize]
		public JsonResult LayChiTietDonTheoID(int idDon)
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
						LoaiDK = "CaNhan"
					});
				}
				else if (loaiDK.Trim().ToUpper() == "TAPTHE")
				{
					string query = @"
					SELECT ct.ID_NhanVien, d.ID_DonDK, d.NgayDK, d.CaAn, nv.HoVaTen, ct.SoLuong, ct.TrangThai
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
						LoaiDK = "TapThe",
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
		[Authorize(Roles = "Admin,User")]
		public IActionResult UpdateDonOnly(DonCaNhanRequest request, int iD)
		{
			try
			{
				if (iD == null)
				{
					return BadRequest("Thông tin ID đơn cần xóa  rỗng");
				}
				if (request == null)
				{
					return BadRequest("Thông tin vào rỗng");
				}
				if (request.LoaiDK.Equals("CaNhan", StringComparison.OrdinalIgnoreCase) == false)
				{
					return BadRequest("Nhập đúng LoaiDK là CaNhan");
				}
				//Lấy Id_nhan viên từ jwt;
				int? id_NhanVien = GetIDFromJWT();
				if (id_NhanVien == null)
				{
					return BadRequest("Không lấy đc id nhân viên từ JWT");
				}

				//Check hạn sửa ca ăn
				DateTime hanDangKy;
				if (request.CaAn == 1)
				{
					hanDangKy = DateTime.Today.AddHours(9);
				}
				else if (request.CaAn == 2)
				{
					hanDangKy = DateTime.Today.AddHours(15);
				}
				else if (request.CaAn == 3)
				{
					hanDangKy = DateTime.Today.AddHours(21);
				}
				else
				{
					return BadRequest("Ca ăn này không tồn tại");
				}

				if (DateTime.Now > hanDangKy)
				{
					return BadRequest("Quá hạn sửa đổi của ca");
				}

				int idNhanVienCheck;
				using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					connection.Open();
					using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
					{
						//Lấy ID_nhân viên đã tạo ra đơn thông qua iD
						string queryNhanVien = @"select ID_NhanVien from DonDK where ID_DonDK = @ID_DonDK";
						using (SqlCommand commandNhanVien = new SqlCommand(queryNhanVien, connection, transaction))
						{
							commandNhanVien.Parameters.AddWithValue("@ID_DonDK", iD);
							object result = commandNhanVien.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy ra được ID_NhanVien của ID_Don" + iD);

							}
							idNhanVienCheck = (int)result;

						}

						if (idNhanVienCheck != id_NhanVien)
						{
							return BadRequest("Chỉ có nhân viên tạo ra mới được sửa");
						}

						// Kiểm tra đã tồn tại đơn chưa
						string checkQuery = @"
							SELECT COUNT(*) 
							FROM DonDK 
							WHERE ID_NhanVien = @ID_NhanVien 
							  AND LoaiDK = @LoaiDK 
							  AND CaAn = @CaAn 
							  AND CONVERT(date, NgayDK) = CONVERT(date, GETDATE())";

						using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection, transaction))
						{
							checkCommand.Parameters.AddWithValue("@ID_NhanVien", id_NhanVien);
							checkCommand.Parameters.AddWithValue("@LoaiDK", request.LoaiDK);
							checkCommand.Parameters.AddWithValue("@CaAn", request.CaAn);

							int count = (int)checkCommand.ExecuteScalar();
							if (count > 0)
							{
								transaction.Rollback(); // rollback nếu dùng transaction
								return BadRequest("Bạn đã đặt đơn cho ca này trong ngày hôm nay.");
							}
						}

						//Check xem trạng thái có là Da Xac Nhan
						string trangThaiDon;
						string queryTrangThai = @"select TrangThai from DonDK where ID_DonDK= @ID_DonDK";
						using(SqlCommand commandTrangThai = new SqlCommand(queryTrangThai, connection, transaction))
						{
							commandTrangThai.Parameters.AddWithValue("@ID_DonDK", iD);
							object result = commandTrangThai.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy được trạng thái của đơn");
							}
							trangThaiDon = result.ToString();

						}
						if (trangThaiDon != TrangThaiDon.ChoXacNhan.ToString())
						{
							return BadRequest("Trạng thái đơn phải là Chờ xác nhận mới có thể update được ");
						}




						//update ca an trong DonDK
						string queryDonDk = @"update DonDK set CaAn = @CaAn
							where ID_DonDK = @ID_DonDK and TrangThai = @TrangThai";
						using (SqlCommand commandDonDK = new SqlCommand(queryDonDk, connection, transaction))
						{
							commandDonDK.Parameters.AddWithValue("@CaAn", request.CaAn);
							commandDonDK.Parameters.AddWithValue("@ID_DonDK", iD);
							commandDonDK.Parameters.AddWithValue("@TrangThai", TrangThaiDon.ChoXacNhan.ToString());
							commandDonDK.ExecuteNonQuery();
						}
						//update so luong  trong ChiTietDonDK
						string queryChiTietDonDK = @"update ct set ct.SoLuong = @SoLuong
							from ChiTietDonDK ct	
							join DonDK dk on dk.ID_DonDK = ct.ID_DonDK
							where dk.ID_DonDK = @ID_DonDK 
							and dk.ID_NhanVien=@ID_NhanVien 
							and dk.TrangThai = @TrangThai 
							and dk.CaAn = @CaAn";
						using (SqlCommand commandChiTietDonDK = new SqlCommand(queryChiTietDonDK, connection, transaction))
						{
							commandChiTietDonDK.Parameters.AddWithValue("@SoLuong", request.SoLuong);
							commandChiTietDonDK.Parameters.AddWithValue("@ID_DonDK", iD);
							commandChiTietDonDK.Parameters.AddWithValue("@TrangThai", TrangThaiDon.ChoXacNhan.ToString());
							commandChiTietDonDK.Parameters.AddWithValue("@CaAn", request.CaAn);
							commandChiTietDonDK.Parameters.AddWithValue("@ID_NhanVien", idNhanVienCheck);
							commandChiTietDonDK.ExecuteNonQuery();
						}
						transaction.Commit();
					}
				}
				return Ok("UpdateDonOnly success");
			}
			catch (SqlException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[Route("UpdateFull")]
		[HttpPut]
		[Authorize(Roles = "TapThe")]
		public IActionResult UpdateFull([FromBody] DonFullRequest request, int iD)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Dữ liệu vào rỗng");
				}
				if (iD == null)
				{
					return BadRequest("Dữ liệu ID đơn cần xóa vào rỗng");
				}
				int? id_NVFromJWT = GetIDFromJWT();
				if (id_NVFromJWT == null)
				{
					return BadRequest("Không lấy được giữ liệu nhân viên đăng nhập từ JWT");
				}

				//Hạn sửa
				DateTime hanDangKy;
				if (request.donDK.CaAn == 1)
				{
					hanDangKy = DateTime.Today.AddHours(9);
				}
				else if (request.donDK.CaAn == 2)
				{
					hanDangKy = DateTime.Today.AddHours(15);
				}
				else if (request.donDK.CaAn == 3)
				{
					hanDangKy = DateTime.Today.AddHours(21);
				}
				else
				{
					return BadRequest("Ca ăn này không tồn tại");
				}


				if (DateTime.Now > hanDangKy)
				{
					return BadRequest("Đã quá hạn sửa ca này");
				}

				using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					connection.Open();

					int idCheck;
					using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
					{
						//lấy id nhân viên đã tạo ra đơn đó
						string queryNhanVien = @"select ID_NhanVien from DonDK where ID_DonDK= @ID_DonDK";
						using (SqlCommand commandNhanVien = new SqlCommand(queryNhanVien, connection, transaction))
						{
							commandNhanVien.Parameters.AddWithValue("@ID_DonDK", iD);
							object result = commandNhanVien.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy được ID_Nhan Vien của đơn hàng này");
							}
							idCheck = (int)result;

						}
						//Kiểm tra xem id nhân viên tạo ra đơn đó vơi sid của người đăng nhập có trùng nhau không
						if (idCheck != id_NVFromJWT)
						{
							return BadRequest("Chỉ nhân viên nào đăng ký đơn này thì mới có quyền sửa");
						}


						// Kiểm tra đã tồn tại đơn chưa
						string checkQuery = @"
							SELECT COUNT(*) 
							FROM DonDK 
							WHERE ID_NhanVien = @ID_NhanVien 
							  AND LoaiDK = @LoaiDK 
							  AND CaAn = @CaAn 
							  AND CONVERT(date, NgayDK) = CONVERT(date, GETDATE())";

						using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection, transaction))
						{
							checkCommand.Parameters.AddWithValue("@ID_NhanVien", id_NVFromJWT);
							checkCommand.Parameters.AddWithValue("@LoaiDK", request.donDK.LoaiDK);
							checkCommand.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);

							int count = (int)checkCommand.ExecuteScalar();
							if (count > 0)
							{
								transaction.Rollback(); // rollback nếu dùng transaction
								return BadRequest("Bạn đã đặt đơn cho ca này trong ngày hôm nay.");
							}
						}

						//Check xem trạng thái có là Da Xac Nhan
						string trangThaiDon;
						string queryTrangThai = @"select TrangThai from DonDK where ID_DonDK= @ID_DonDK";
						using (SqlCommand commandTrangThai = new SqlCommand(queryTrangThai, connection, transaction))
						{
							commandTrangThai.Parameters.AddWithValue("@ID_DonDK", iD);
							object result = commandTrangThai.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy được trạng thái của đơn");
							}
							trangThaiDon = result.ToString();

						}
						if (trangThaiDon != TrangThaiDon.ChoXacNhan.ToString())
						{
							return BadRequest("Trạng thái đơn phải là Chờ xác nhận mới có thể update được ");
						}

						//update ca an trong DonDK
						string queryDonDk = @"update DonDK set CaAn = @CaAn
							where ID_DonDK = @ID_DonDK and TrangThai= @TrangThai";
						using (SqlCommand commandDonDK = new SqlCommand(queryDonDk, connection, transaction))
						{
							commandDonDK.Parameters.AddWithValue("@CaAn", request.donDK.CaAn);
							commandDonDK.Parameters.AddWithValue("@ID_DonDK", iD);
							commandDonDK.Parameters.AddWithValue("@TrangThai", TrangThaiDon.ChoXacNhan.ToString());
							commandDonDK.ExecuteNonQuery();
						}
						var danhsachPhong = new List<string>();

						foreach (var ct in request.listChiTiet)
						{
							string queryPhong = @"select ID_Phong from NhanVien where ID_NhanVien=@ID_NhanVien";
							using (SqlCommand commandPhong = new SqlCommand(queryPhong, connection, transaction))
							{
								commandPhong.Parameters.AddWithValue("@ID_NhanVien", ct.ID_NhanVien);

								object result = commandPhong.ExecuteScalar();
								if (result == null)
								{
									return BadRequest("không có phòng abn nào có nhân viên có id là " + ct.ID_NhanVien);
								}
								danhsachPhong.Add(result.ToString());

							}
						}
						if (danhsachPhong.Distinct().Count() > 1)
						{
							return BadRequest("Có nhân viên không có chung phòng ban");
						}


						//sửa số lượng ở chi tiết đơn
						foreach (var ctt in request.listChiTiet)
						{
							string queryChiTiet = @"update ct set ct.SoLuong= @SoLuong
						from ChitietDonDK ct
						join DonDK dk on ct.ID_DonDK=dk.ID_DonDK
						where ct.ID_DonDK=@ID_DonDK ";
							using (SqlCommand commandChiTTiet = new SqlCommand(queryChiTiet, connection, transaction))
							{
								commandChiTTiet.Parameters.AddWithValue("@SoLuong", ctt.SoLuong);
								
								commandChiTTiet.Parameters.AddWithValue("@ID_DonDK", iD);
								commandChiTTiet.ExecuteNonQuery();

							}
						}




						transaction.Commit();
					}

				}

				return Ok("DonFullRequest success");
			}
			catch (SqlException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}


		[Route("CheckedDon")]
		[HttpPut]
		[Authorize(Roles = "User, Admin")]
		public IActionResult KhoaDon(int iD_Don)
		{
			try
			{
				//Lấy iD nhân viên từ đăng nhập
				int? IDFromJWT = GetIDFromJWT();
				if (IDFromJWT == null)
				{
					return BadRequest("Không lấy được ID nhân viên từ JWT");

				}
				using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("QLCaAn")))
				{
					connection.Open();
					using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
					{
						//lấy id nhân viên đã đăng ký cái đơn này 
						string queryID = @"select ID_NhanVien from DonDK where ID_DonDK = @ID_DonDK";
						int id_NVChecked;
						using (SqlCommand commandID = new SqlCommand(queryID, connection, transaction))
						{
							commandID.Parameters.AddWithValue("@ID_DonDK", iD_Don);
							object result = commandID.ExecuteScalar();
							if (result == null)
							{
								return BadRequest("Không lấy được Id nhân viên đã tạo ra đơn này");
							}

							id_NVChecked = (int)result;
						}
						if (id_NVChecked != IDFromJWT)
						{
							return BadRequest("Đây không là nhân viên đã tạo ra đơn này");
						}
						//sửa trạng thái từ wait -> complete
						string queryChange = @"update DonDK set TrangThai= @TrangThai where ID_DonDK = @ID_DonDK";
						using (SqlCommand commandChange = new SqlCommand(queryChange, connection, transaction))
						{
							commandChange.Parameters.AddWithValue("@TrangThai", TrangThaiDon.DaXacNhan.ToString());
							commandChange.Parameters.AddWithValue("@ID_DonDK", iD_Don);
							commandChange.ExecuteNonQuery();

						}
						transaction.Commit();
					}
				}

				return Ok("Đã khóa đơn không thể hủy hay sửa được nữa");
			}
			catch (SqlException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


	}
}

