export interface DonCaNhanRequest {
  LoaiDK?: string; // Luôn là 'CANHAN' trong method  InsertOnlynày >
  CaAn: number; // Ca ăn: "1", "2", "3" (dạng so)
  SoLuong: number; // Mặc định đăng ký 1 suất
}

export enum TrangThaiDon {
  ChoXacNhan = 0,
  DaXacNhan = 1,
}
export interface ChiTietDonDK {
  ID_ChiTietDonDK?: number;

  SoLuong: number;

  ID_NhanVien: number;

  ID_DonDK?: number;
}
class DonDK {
  ID_DonDK?: number;

  NgayDK?: any;

  LoaiDK?: any;
  TrangThai?: TrangThaiDon;

  ID_NhanVien?: any;

  CaAn?: any;
}
export interface DonFullRequest {
  //public string TenDangNhap { get; set; }
  donDK?: DonDK;
  listChiTiet?: ChiTietDonDK[];
}
