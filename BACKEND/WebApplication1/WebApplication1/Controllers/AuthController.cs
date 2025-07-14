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

				// Bước 1: Xác thực tài khoản
				string sqlAuth = "SELECT ID_NhanVien, HoVaTen FROM NhanVien WHERE TenDangNhap = @gmail AND MatKhau = @mk";
				using SqlCommand cmdAuth = new SqlCommand(sqlAuth, conn);
				cmdAuth.Parameters.AddWithValue("@gmail", request.TenDangNhap);
				cmdAuth.Parameters.AddWithValue("@mk", request.MatKhau);

				using var readerAuth = cmdAuth.ExecuteReader();
				if (!readerAuth.Read())
					return BadRequest("Sai tài khoản hoặc mật khẩu.");

				int idNhanVien = Convert.ToInt32(readerAuth["ID_NhanVien"]);
				string hoVaTen = readerAuth["HoVaTen"].ToString();
				readerAuth.Close();

				// Bước 2: Lấy toàn bộ Role và ID_TaiKhoan từ ID_NhanVien
				string sqlRoles = @"
					SELECT tk.ID_TaiKhoan, tk.Role
					FROM NhanVien_TaiKhoan nvtk
					JOIN TaiKhoan tk ON tk.ID_TaiKhoan = nvtk.ID_TaiKhoan
					WHERE nvtk.ID_NhanVien = @idNV";

				using SqlCommand cmdRoles = new SqlCommand(sqlRoles, conn);
				cmdRoles.Parameters.AddWithValue("@idNV", idNhanVien);

				using var readerRoles = cmdRoles.ExecuteReader();
				List<string> listRole = new();
				List<string> listTaiKhoan = new();

				while (readerRoles.Read())
				{
					listRole.Add(readerRoles["Role"].ToString());
					listTaiKhoan.Add(readerRoles["ID_TaiKhoan"].ToString());
				}

				// Tạo claims và token
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, hoVaTen),
					new Claim("ID_NhanVien", idNhanVien.ToString())
				};

				foreach (var r in listRole.Distinct())
					claims.Add(new Claim(ClaimTypes.Role, r));

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-1234567890-abcdef"));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.Now.AddDays(7),
					signingCredentials: creds
				);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					ID_NhanVien = idNhanVien,
					HoVaTen = hoVaTen,
					ListRole = listRole.Distinct().ToList(),
					DSTaiKhoan = listTaiKhoan.Distinct().ToList()
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


	}
}
