namespace WebApplication1.Model
{
	public class NhanVien_TaiKhoan
	{
		public string ID_NhanVien { get; set; }
		public NhanVien NhanVien { get; set; }

		public string ID_TaiKhoan { get; set; }
		public TaiKhoan TaiKhoan { get; set; }
	}
}
