using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
	public class ChiTietDonDK
	{
		public string ID_ChiTietDonDK { get; set; }

		public int SoLuong { get; set; }

		public string ID_NhanVien { get; set; }

		public string TrangThai { get; set; }

		
		public string? ID_DonDK { get; set; }

		// Navigation properties
		[ForeignKey("ID_NhanVien")]
		public virtual NhanVien? NhanVien { get; set; }
		[ForeignKey("ID_DonDK")]
		public virtual DonDK? DonDK { get; set; }
	}
}
