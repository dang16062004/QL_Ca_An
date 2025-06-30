using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
	
	public class DonDK
	{
		public int? ID_DonDK { get; set; }

		public DateTime? NgayDK;

		public string? LoaiDK { get; set; }
		public TrangThaiDon TrangThai { get; set; }

		// Foreign key
		public string ?ID_NhanVien { get; set; }

		public int? CaAn { get; set; }

		// Navigation property
		[ForeignKey("ID_NhanVien")]
		public virtual NhanVien? NhanVien { get; set; }
		
		public virtual ICollection<ChiTietDonDK>? ChiTietDonDKs { get; set; }
	}
}
