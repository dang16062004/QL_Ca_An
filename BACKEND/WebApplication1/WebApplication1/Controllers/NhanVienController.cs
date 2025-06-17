using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public NhanVienController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetAll")]
        public JsonResult GetAll()
        {
            DataTable dataTable = new DataTable();
            string dataSource = _configuration.GetConnectionString("QLCaAn");
            string query = "select nv.ID_NhanVien,nv.HoVaTen,nv.Namsinh,nv.QDK,nv.PhanQuyen,nv.TenDangNhap,pb.TenPhong from NhanVien nv " +
                "inner join PhongBan pb on pb.ID_Phong=nv.ID_Phong";
            SqlDataReader sqlDataReader;
            try
            {
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
                Console.WriteLine(ex.ToString());
                return new JsonResult("Lỗi ko xuất được");
            }

        }


        [HttpPost]
        [Route("Insert")]
        public JsonResult Insert(NhanVien nhanVien)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string query = "INSERT INTO NhanVien (ID_NhanVien, HoVaTen, Namsinh, QDK, PhanQuyen, TenDangNhap, ID_Phong) " +
               "VALUES (@ID_NhanVien, @HoVaTen, @Namsinh, @QDK, @PhanQuyen, @TenDangNhap, @ID_Phong)";



                string dataSource = _configuration.GetConnectionString("QLCaAn");
                SqlDataReader sqlDataReader;
                using (SqlConnection sqlConnection = new SqlConnection(dataSource))
                {
                    sqlConnection.Open();
					// 1. Kiểm tra nếu QDK là TAPTHE thì trong phòng đó đã có ai TAPTHE chưa
					if (nhanVien.QDK == "TAPTHE")
					{
						string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE ID_Phong = @ID_Phong AND QDK = 'TAPTHE'";
						using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection))
						{
							checkCmd.Parameters.AddWithValue("@ID_Phong", nhanVien.ID_Phong);
							int count = (int)checkCmd.ExecuteScalar();

							if (count > 0)
							{
								return new JsonResult("Phòng ban này đã có người có quyền đăng ký là TẬP THỂ.");
							}
						}
					}
					// 2. Kiểm tra nếu PhanQuyen là ADMIN → hệ thống đã có ADMIN chưa?
					if (nhanVien.PhanQuyen == "ADMIN")
					{
						string checkAdminQuery = "SELECT COUNT(*) FROM NhanVien WHERE PhanQuyen = 'ADMIN'";
						using (SqlCommand checkCmd = new SqlCommand(checkAdminQuery, sqlConnection))
						{
							int count = (int)checkCmd.ExecuteScalar();
							if (count > 0)
							{
								return new JsonResult("❌ Chỉ được có 1 nhân viên có phân quyền là Quản lý trong hệ thống.");
							}
						}
					}
					// 3. Kiểm tra TenDangNhap đã được gán cho nhân viên nào chưa
					string checkUsernameQuery = "SELECT COUNT(*) FROM NhanVien WHERE TenDangNhap = @TenDangNhap";
					using (SqlCommand checkCmd = new SqlCommand(checkUsernameQuery, sqlConnection))
					{
						checkCmd.Parameters.AddWithValue("@TenDangNhap", nhanVien.TenDangNhap);
						int count = (int)checkCmd.ExecuteScalar();
						if (count > 0)
						{
							return new JsonResult("❌ Tài khoản này đã được gán cho nhân viên khác.");
						}
					}

					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                
                    {
                        sqlCommand.Parameters.AddWithValue("@ID_NhanVien", nhanVien.ID_NhanVien);
                        sqlCommand.Parameters.AddWithValue("@HoVaTen", nhanVien.HoVaTen);
                        sqlCommand.Parameters.AddWithValue("@Namsinh", nhanVien.NamSinh);
                        sqlCommand.Parameters.AddWithValue("@QDK", nhanVien.QDK);
                        sqlCommand.Parameters.AddWithValue("@PhanQuyen", nhanVien.PhanQuyen);
                        sqlCommand.Parameters.AddWithValue("@TenDangNhap", nhanVien.TenDangNhap);
                        sqlCommand.Parameters.AddWithValue("@ID_Phong", nhanVien.ID_Phong);
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


        [HttpPut]
        [Route("Update")]
        public JsonResult Update(NhanVien nhanVien)
        {
            try
            {
                string query = "update NhanVien set HoVaTen = @HoVaTen , Namsinh=@Namsinh, QDK= @QDK,PhanQuyen=@PhanQuyen, TenDangNhap=@TenDangNhap , ID_phong=@ID_phong where ID_NhanVien=@ID_NhanVien ";
                string dataSource = _configuration.GetConnectionString("QLCaAn");
                SqlDataReader sqlDataReader;
                DataTable dataTable = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(dataSource))
                {
                    sqlConnection.Open();
					// 1. Kiểm tra nếu QDK là TAPTHE thì trong phòng đó đã có ai TAPTHE chưa
					if (nhanVien.QDK == "TAPTHE")
					{
						string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE ID_Phong = @ID_Phong AND QDK = 'TAPTHE'";
						using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection))
						{
							checkCmd.Parameters.AddWithValue("@ID_Phong", nhanVien.ID_Phong);
							int count = (int)checkCmd.ExecuteScalar();

							if (count > 0)
							{
								return new JsonResult("Phòng ban này đã có người có quyền đăng ký là TẬP THỂ.");
							}
						}
					}
					// 2. Kiểm tra nếu PhanQuyen là ADMIN → hệ thống đã có ADMIN chưa?
					if (nhanVien.PhanQuyen == "ADMIN")
					{
						string checkAdminQuery = "SELECT COUNT(*) FROM NhanVien WHERE PhanQuyen = 'ADMIN'";
						using (SqlCommand checkCmd = new SqlCommand(checkAdminQuery, sqlConnection))
						{
							int count = (int)checkCmd.ExecuteScalar();
							if (count > 0)
							{
								return new JsonResult("❌ Chỉ được có 1 nhân viên có phân quyền là Quản lý trong hệ thống.");
							}
						}
					}
					// 3. Kiểm tra TenDangNhap đã được gán cho nhân viên nào chưa
					string checkUsernameQuery = "SELECT COUNT(*) FROM NhanVien WHERE TenDangNhap = @TenDangNhap";
					using (SqlCommand checkCmd = new SqlCommand(checkUsernameQuery, sqlConnection))
					{
						checkCmd.Parameters.AddWithValue("@TenDangNhap", nhanVien.TenDangNhap);
						int count = (int)checkCmd.ExecuteScalar();
						if (count > 0)
						{
							return new JsonResult("❌ Tài khoản này đã được gán cho nhân viên khác.");
						}
					}

					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ID_NhanVien", nhanVien.ID_NhanVien);
                        sqlCommand.Parameters.AddWithValue("@HoVaTen", nhanVien.HoVaTen);
                        sqlCommand.Parameters.AddWithValue("@Namsinh", nhanVien.NamSinh);
                        sqlCommand.Parameters.AddWithValue("@QDK", nhanVien.QDK);
                        sqlCommand.Parameters.AddWithValue("@PhanQuyen", nhanVien.PhanQuyen);
                        sqlCommand.Parameters.AddWithValue("@TenDangNhap", nhanVien.TenDangNhap);
                        sqlCommand.Parameters.AddWithValue("@ID_Phong", nhanVien.ID_Phong);
                        sqlDataReader = sqlCommand.ExecuteReader();
                        dataTable.Load(sqlDataReader);
                        sqlDataReader.Close();
                        sqlConnection.Close();
                        
                    }
                }
				return new JsonResult("Success Update");

			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new JsonResult("Lỗi ko update đc");
            }

        }
    
		[HttpDelete("Delete/{id}")]
		public JsonResult Delete(string id)
		{
			try
			{
				string query = @"DELETE FROM NhanVien WHERE ID_NhanVien = @ID_NhanVien";
				string dataSource = _configuration.GetConnectionString("QLCaAn");

				using (SqlConnection connection = new SqlConnection(dataSource))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@ID_NhanVien", id);
						int rowsAffected = command.ExecuteNonQuery();
						return new JsonResult(new { success = true, message = "Xóa thành công", rows = rowsAffected });
					}
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(new { success = false, message = "Lỗi xóa nhân viên", error = ex.Message });
			}
		}












	}
}
