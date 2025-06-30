using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
	public enum TrangThaiDon
	{
		[Display(Name = "Chờ xác nhận")]
		ChoXacNhan=0,
		[Display(Name = "Đã xác nhận")]
		DaXacNhan=1,


	};




	public class DonCaNhanRequest
	{
		public string? LoaiDK { get; set; }  // Luôn là 'CANHAN' trong method  InsertOnlynày >
		public int CaAn { get; set; }             // Ca ăn: "1", "2", "3" (dạng so)
		public int SoLuong { get; set; } = 1;        // Mặc định đăng ký 1 suất
		
	}
}
