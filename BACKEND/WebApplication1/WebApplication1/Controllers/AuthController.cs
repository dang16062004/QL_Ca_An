using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _config;
		public AuthController(IConfiguration config)
		{
			_config = config;
		}
		[HttpPost("Login")]
		public IActionResult Login([FromBody] LoginRequest request)
		{
			try
			{
				using SqlConnection conn = new SqlConnection(_config.GetConnectionString("QLCaAn"));
				conn.Open();
				string sql = @"
				select nv.HoVaTen,nv.TenDangNhap, tk.ID_TaiKhoan, tk.Role, nv.ID_NhanVien
				from NhanVien nv
				join NhanVien_TaiKhoan nvtk on nv.ID_NhanVien = nvtk.ID_NhanVien
				join TaiKhoan tk on tk.ID_TaiKhoan = nvtk.ID_TaiKhoan
				where nv.TenDangNhap = @gmail and nv.MatKhau= @mk
				";
				using SqlCommand command = new SqlCommand(sql, conn);
				command.Parameters.AddWithValue("@gmail", request.TenDangNhap);
				command.Parameters.AddWithValue("@mk", request.MatKhau);

				using var reader = command.ExecuteReader();
				if (!reader.Read())
				{
					return BadRequest("Lỗi đăng nhập");
				}


				//Thiết lập tạo JWT và xác thực người dùng (Nội dung thư)
				var claims = new[]
				{
					new Claim(ClaimTypes.Name,reader["HoVaTen"].ToString()),  //constant (hằng số) của .NET, đại diện cho "Tên người dùng".
					new Claim("ID_NhanVien", reader["ID_NhanVien"].ToString()),
					new Claim("Role",reader["Role"].ToString()),
					new Claim("ID_TaiKhoan", reader["ID_TaiKhoan"].ToString())//vì new Claim(string type, string value)
				};

				//khai báo key dùng để làm chìa khóa cho việc ký và xác minh
				//Dùng để đảm bảo token không bị chỉnh sửa bởi ai khác (vì chỉ server biết key này).
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-1234567890-abcdef"));//Con dấu

				// Dùng để mã hóa key theo thuật toán HmacSha256 (kiểu đóng dấu)
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);




				//
				//Gửi thư
				var token = new JwtSecurityToken(
					claims: claims,//nội dung thư
					expires: DateTime.Now.AddDays(7),//hạn của thư nếu hết ngày thì quay lại phần đăng nhập
					signingCredentials: creds//cách đóng dấu đỏ để người khác xác minh
					);


				//Mã hóa thành chuỗi và trả về cho Client
				return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token),
					Role = reader["Role"].ToString(),
					ID_NhanVien = reader["ID_NhanVien"].ToString(),
					ID_TaiKhoan = reader["ID_TaiKhoan"].ToString(),
					HoVaTen = reader["HoVaTen"].ToString(),
				});//Đóng gọi phong thư và chuyển cho người nhận

			}
			catch (Exception ex)
			{
				return BadRequest(ex.ToString());

			}

		}


	

	}
}
