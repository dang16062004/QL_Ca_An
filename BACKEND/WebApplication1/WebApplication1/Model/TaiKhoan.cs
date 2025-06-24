namespace WebApplication1.Model
{
	public class TaiKhoan
	{
		public int ID_TaiKhoan { get; set; }
		public DateTime? NgayTao { get; set; }
		public string Role { get; set; }

		// Quan hệ N-N với NhanVien
		public virtual ICollection<NhanVien_TaiKhoan> NhanVien_TaiKhoans { get; set; }


	}
}
