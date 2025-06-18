namespace WebApplication1.Model
{
	public class TaiKhoan
	{
		public string? ID_TaiKhoan { get; set; }
		public string TenDangNhap { get; set; }
		public DateTime NgayTao;
		public string MatKhau { get; set; }
		public virtual NhanVien? NhanVien { get; set; }
	}
}
