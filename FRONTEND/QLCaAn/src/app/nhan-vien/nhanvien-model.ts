// src/app/nhan-vien/nhanvien-model.ts
export interface NhanVienDTO {
  hoVaTen: string;
  namSinh: string; // yyyy-MM-dd
  tenDangNhap: string;
  matKhau: string;
  id_Phong: number;
  phanQuyen: 'Admin' | 'User';
  qdk: 'CaNhan' | 'TapThe';
}

// src/app/nhan-vien/nhanvien-model.ts
export interface UpNhanVienDTO {
  ID_NhanVien: number;
  HoVaTen: string;
  NamSinh: string; // yyyy-MM-dd
  TenDangNhap: string;
  MatKhau: string;
  ID_Phong: string; // ← C# là string, nên gửi string
  PhanQuyen: 'Admin' | 'User';
  QDK: 'CaNhan' | 'TapThe';
}
