namespace WebApplication1.Model
{
	public class DonCaNhanRequest
	{
		//public string? ID_Don { get; set; }
		
		//public DateTime? NgayDK { get; set; }         // Ngày đăng ký đơn
		public string? LoaiDK { get; set; } = "CaNhan"; // Luôn là 'CANHAN' trong method  InsertOnlynày >
		public int CaAn { get; set; }             // Ca ăn: "1", "2", "3" (dạng so)
		public int SoLuong { get; set; } = 1;        // Mặc định đăng ký 1 suất
	}
}
