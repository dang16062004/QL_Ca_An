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
