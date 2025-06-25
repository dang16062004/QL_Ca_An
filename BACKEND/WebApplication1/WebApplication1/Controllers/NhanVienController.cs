using Microsoft.AspNetCore.Authorization;
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


		
		
		[Route("GetAll")]
		//[Authorize]
		[HttpGet]
		public IActionResult GetAll()
		{
			try
			{
				DataTable data = new DataTable();
				SqlDataReader reader;
				using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("QLCaAn"));

				string query = @"
							SELECT 
					nv.ID_NhanVien,
					nv.HoVaTen,
					nv.NamSinh,
					nv.TenDangNhap,
					pb.TenPhong,

					-- Xác định PhanQuyen: Ưu tiên Admin > User
					CASE 
						WHEN SUM(CASE WHEN tk.Role = 'Admin' THEN 1 ELSE 0 END) >= 1 THEN 'Admin'
						WHEN SUM(CASE WHEN tk.Role = 'User' THEN 1 ELSE 0 END) >= 1 THEN 'User'
						ELSE 'Không xác định'
					END AS PhanQuyen,

					-- Xác định QDK: Ưu tiên TapThe > CaNhan
					CASE 
						WHEN SUM(CASE WHEN tk.Role = 'TapThe' THEN 1 ELSE 0 END) >= 1 THEN 'TapThe'
						WHEN SUM(CASE WHEN tk.Role = 'CaNhan' THEN 1 ELSE 0 END) >= 1 THEN 'CaNhan'
						ELSE 'Không xác định'
					END AS QDK

				FROM NhanVien nv
				JOIN PhongBan pb ON nv.ID_Phong = pb.ID_Phong
				JOIN NhanVien_TaiKhoan nvtk ON nvtk.ID_NhanVien = nv.ID_NhanVien
				JOIN TaiKhoan tk ON tk.ID_TaiKhoan = nvtk.ID_TaiKhoan

				GROUP BY 
					nv.ID_NhanVien, nv.HoVaTen, nv.NamSinh, nv.TenDangNhap, pb.TenPhong

		";

				conn.Open();
				using SqlCommand command = new SqlCommand(query, conn);
				reader = command.ExecuteReader();
				data.Load(reader);
				reader.Close();
				conn.Close();
				return Ok(data);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.ToString());
			}
		}


	

		[Route("Create")]
		[HttpPost]
		[Authorize(Roles = "Admin")]// ✅ Chỉ người có role "Admin" trong JWT mới gọi được API này
		public IActionResult Create([FromBody] CreateNhanVienRequest req)
		{
			// 0️. Kiểm tra dữ liệu đầu vào có hợp lệ không
			if (req == null) return BadRequest("Body rỗng");

			// ✅ Kiểm tra PhanQuyen và QDK có đúng định dạng cho phép không
			if (!new[] { "Admin", "User" }.Contains(req.PhanQuyen, StringComparer.OrdinalIgnoreCase) ||
				!new[] { "CaNhan", "TapThe" }.Contains(req.QDK, StringComparer.OrdinalIgnoreCase))
				return BadRequest("PhanQuyen hoặc QDK không hợp lệ");

			try
			{
				using SqlConnection conn = new(_configuration.GetConnectionString("QLCaAn"));
				conn.Open();

				// ✅ Mở Transaction để đảm bảo toàn bộ quá trình thêm nhân viên là nguyên tử(có tính liên kết)

				using SqlTransaction tran = conn.BeginTransaction(IsolationLevel.Serializable);

				// 1️. Nếu tạo Admin mới → kiểm tra đã có Admin nào tồn tại chưa (Admin = ID_TaiKhoan = 1)
				if (req.PhanQuyen.Equals("Admin", StringComparison.OrdinalIgnoreCase))
				{
					const string sqlCheckAdmin = @"SELECT COUNT(*) FROM NhanVien_TaiKhoan WHERE ID_TaiKhoan = 1";
					using var cmd = new SqlCommand(sqlCheckAdmin, conn, tran);
					var hasAdmin = (int)cmd.ExecuteScalar() > 0;
					if (hasAdmin)
						// ❌ Nếu đã có Admin rồi → trả lỗi
						return Conflict("Đã tồn tại người có quyền Admin. Không thể thêm Admin thứ hai.");
				}

				// 2️. Nếu QDK là TapThe → kiểm tra phòng đã có ai TapThe chưa (TapThe = ID_TaiKhoan = 4)
				if (req.QDK.Equals("TapThe", StringComparison.OrdinalIgnoreCase))
				{
					const string sqlCheckTapThe = @"
                SELECT COUNT(*) 
                FROM NhanVien nv
                JOIN NhanVien_TaiKhoan nvtk ON nv.ID_NhanVien = nvtk.ID_NhanVien
                WHERE nv.ID_Phong = @IDPhong AND nvtk.ID_TaiKhoan = 4";   // 4 = TapThe
					using var cmd = new SqlCommand(sqlCheckTapThe, conn, tran);
					cmd.Parameters.AddWithValue("@IDPhong", req.ID_Phong);
					var hasTapThe = (int)cmd.ExecuteScalar() > 0;
					if (hasTapThe)
						// ❌ Nếu phòng đã có TapThe → từ chối
						return Conflict($"Phòng {req.ID_Phong} đã có 1 người TapThe. Vui lòng chọn CaNhan.");
				}

				// 3️. Chèn thông tin nhân viên mới (ID tự tăng)
				const string sqlInsNV = @"
            INSERT INTO NhanVien (HoVaTen, NamSinh, TenDangNhap, MatKhau, ID_Phong)
            VALUES (@HoTen, @NamSinh, @TenDN, @MatKhau, @IDPhong);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

				int idNhanVien;
				using (var cmd = new SqlCommand(sqlInsNV, conn, tran))
				{
					cmd.Parameters.AddWithValue("@HoTen", req.HoVaTen);
					cmd.Parameters.AddWithValue("@NamSinh", req.NamSinh);
					cmd.Parameters.AddWithValue("@TenDN", req.TenDangNhap);
					cmd.Parameters.AddWithValue("@MatKhau", req.MatKhau);
					cmd.Parameters.AddWithValue("@IDPhong", req.ID_Phong);
					idNhanVien = (int)cmd.ExecuteScalar(); // ✅ Lưu ID_NhanVien mới tạo
				}

				// 4️. Xác định ID_TaiKhoan tương ứng với quyền (1: Admin, 2: User, 3: CaNhan, 4: TapThe)
				var tkIds = new List<int>();
				tkIds.Add(req.PhanQuyen.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? 1 : 2); // Admin/User
				tkIds.Add(req.QDK.Equals("CaNhan", StringComparison.OrdinalIgnoreCase) ? 3 : 4); // CaNhan/TapThe

				// 5️⃣ Chèn các liên kết giữa nhân viên và quyền vào bảng trung gian
				const string sqlLink = @"
            INSERT INTO NhanVien_TaiKhoan (ID_NhanVien, ID_TaiKhoan)
            VALUES (@IDNV, @IDTK);";

				foreach (var idTk in tkIds.Distinct())
				{
					using var cmd = new SqlCommand(sqlLink, conn, tran);
					cmd.Parameters.AddWithValue("@IDNV", idNhanVien);
					cmd.Parameters.AddWithValue("@IDTK", idTk);
					cmd.ExecuteNonQuery();
				}

				tran.Commit();// ✅ Xác nhận toàn bộ transaction thành công
				return Ok(new { message = "✅ Đã tạo nhân viên", ID_NhanVien = idNhanVien });
			}
			catch (SqlException ex)    // catch riêng để gỡ lỗi ROLLBACK
			{// ❌ Lỗi SQL thường do trùng khóa hoặc transaction → rollback sẽ được thực hiện tự động khi `using` kết thúc
				return BadRequest("SQL Error: " + ex.Message);
			}
			catch (Exception ex)
			{   // ❌ Lỗi không xác định
				return BadRequest("Lỗi: " + ex.Message);
			}
		}
















	}
}
