using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
	public class NhanVien
	{
		
		public int ID_NhanVien { get; set; }

		
		public string HoVaTen { get; set; }

		
		public DateOnly NamSinh { get; set; }

		[Required]
		[RegularExpression("^(TAPTHE|CANHAN)$",
			ErrorMessage = "QDK phải là TAPTHE hoặc CANHAN.")]
		public string QDK { get; set; }

		[Required]
		[RegularExpression("^(ADMIN|USER)$",
	   ErrorMessage = "PhanQuyen phải là ADMIN hoặc USER.")]
		public string PhanQuyen { get; set; }

		
		public string TenDangNhap { get; set; }
		public string MatKhau { get; set; }

	
	
		public string ID_Phong { get; set; }

		[ForeignKey("ID_Phong")]
		public virtual PhongBan? PhongBan { get; set; } //để ? để ASP.NET coi là không bắt buộc

		[ForeignKey("TenDangNhap")]
		public virtual TaiKhoan? TaiKhoan { get; set; }
	}
}
