namespace WebApplication1.Model
{
	public class UpdateNhanVienRequest
	{
		public int ID_NhanVien { get; set; }
		public string HoVaTen { get; set; }
		public DateTime NamSinh { get; set; }
		public string TenDangNhap { get; set; }
		public string MatKhau { get; set; }
		public string ID_Phong { get; set; }   // FK -> PhongBan
		public string PhanQuyen { get; set; }   // "Admin" | "User"
		public string QDK { get; set; }   // "CaNhan" | "TapThe"
	}
}
