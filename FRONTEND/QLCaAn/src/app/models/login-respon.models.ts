export interface LoginRespon {
  token: string;
  Role: string;
  ListRole: string[]; // ✅ thêm: danh sách vai trò
  DSTaiKhoan: string[]; // ✅ thêm: danh sách ID tài khoản
  ID_NhanVien: number;
  HoVaTen: string;
}
