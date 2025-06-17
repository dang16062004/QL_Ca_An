namespace WebApplication1.Model
{
	public class PhongBan
	{
		public string ID_Phong { get; set; }
		public string TenPhong { get; set; }
		public virtual ICollection<NhanVien> NhanViens { get; set; }

	}
}
