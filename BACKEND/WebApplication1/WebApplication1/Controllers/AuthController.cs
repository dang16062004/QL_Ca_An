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

		//[HttpPost("Login")]
		//public IActionResult Login([FromBody] LoginRequest request)
		//{
		//	try
		//	{
		//		using SqlConnection conn = new SqlConnection(_config.GetConnectionString("QLCaAn"));
		//		conn.Open();

		//		string sql = @"
		//SELECT nv.ID_NhanVien, nv.HoVaTen, tk.ID_TaiKhoan, tk.Role
		//FROM NhanVien nv
		//JOIN NhanVien_TaiKhoan nvtk ON nv.ID_NhanVien = nvtk.ID_NhanVien
		//JOIN TaiKhoan tk ON tk.ID_TaiKhoan = nvtk.ID_TaiKhoan
		//WHERE nv.TenDangNhap = @gmail AND nv.MatKhau = @mk";

		//		using SqlCommand command = new SqlCommand(sql, conn);
		//		command.Parameters.AddWithValue("@gmail", request.TenDangNhap);
		//		command.Parameters.AddWithValue("@mk", request.MatKhau);

		//		using var reader = command.ExecuteReader();

		//		List<(string ID_TaiKhoan, string Role)> taiKhoanRoles = new();
		//		int? idNhanVien = null;
		//		string hoVaTen = "";

		//		while (reader.Read())
		//		{
		//			if (idNhanVien == null)
		//			{
		//				idNhanVien = Convert.ToInt32(reader["ID_NhanVien"]);
		//				hoVaTen = reader["HoVaTen"].ToString();
		//			}

		//			string idTaiKhoan = reader["ID_TaiKhoan"].ToString();
		//			string role = reader["Role"].ToString();

		//			taiKhoanRoles.Add((idTaiKhoan, role));
		//		}

		//		if (idNhanVien == null || taiKhoanRoles.Count == 0)
		//			return BadRequest("Sai tài khoản hoặc mật khẩu.");

		//		// Ưu tiên: Admin > User > TapThe > CaNhan
		//		string[] rolePriority = { "Admin", "User", "TapThe", "CaNhan" };
		//		string roleFinal = "Unknown";
		//		string idTaiKhoanFinal = "";

		//		foreach (var role in rolePriority)
		//		{
		//			var match = taiKhoanRoles.FirstOrDefault(x => x.Role == role);
		//			if (!string.IsNullOrEmpty(match.ID_TaiKhoan))
		//			{
		//				roleFinal = match.Role;
		//				idTaiKhoanFinal = match.ID_TaiKhoan;
		//				break;
		//			}
		//		}

		//		// 1. Thu thập các claim
		//		var claims = new List<Claim>
		//		{
		//			new Claim(ClaimTypes.Name, hoVaTen),
		//			new Claim("ID_NhanVien", idNhanVien.ToString())
		//		};

		//		foreach (var r in taiKhoanRoles.Select(x => x.Role).Distinct())
		//			claims.Add(new Claim(ClaimTypes.Role, r));   // ✔️ chuẩn

		//		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-1234567890-abcdef"));
		//		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		//		var token = new JwtSecurityToken(
		//			claims: claims,
		//			expires: DateTime.Now.AddDays(7),
		//			signingCredentials: creds);

		//		return Ok(new
		//		{
		//			token = new JwtSecurityTokenHandler().WriteToken(token),
		//			Role = roleFinal,
		//			ID_TaiKhoan = idTaiKhoanFinal,
		//			ListRole = taiKhoanRoles.Select(x => x.Role).Distinct().ToList(),
		//			DSTaiKhoan = taiKhoanRoles.Select(x => x.ID_TaiKhoan).Distinct().ToList(),
		//			ID_NhanVien = idNhanVien,
		//			HoVaTen = hoVaTen
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		return BadRequest(ex.ToString());
		//	}
		//}
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
