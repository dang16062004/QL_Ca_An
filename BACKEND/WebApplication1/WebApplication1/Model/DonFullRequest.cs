namespace WebApplication1.Model
{
	public class DonFullRequest
	{
		public string TenDangNhap { get; set; }
		public DonDK donDK { get; set; }// lưu đơn
		public List<ChiTietDonDK> listChiTiet { get; set; }// lưu danh sách chi tiết đơn kí của đăng kí tập thể
		
	}
}
