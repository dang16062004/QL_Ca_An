using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
	public class NhanVien
	{

		public int ID_NhanVien { get; set; }
		public string HoVaTen { get; set; }
		public DateTime? Namsinh { get; set; }
		public string TenDangNhap { get; set; }
		public string ID_Phong { get; set; } // Giả sử có bảng Phong liên kết

		public string MatKhau { get; set; }

		// Quan hệ N-N với TaiKhoan
		public virtual ICollection<NhanVien_TaiKhoan> NhanVien_TaiKhoans { get; set; }
	}
}
