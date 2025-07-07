import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRespon } from './models/login-respon.models';
import { LoginRequest } from './models/login-request.models';
import { NonNullAssert } from '@angular/compiler';
import { NhanVienDTO } from './nhan-vien/nhanvien-model';
import { jwtDecode } from 'jwt-decode';
// Interface định nghĩa kiểu dữ liệu của payload sau khi decode token(decode token: giải mã token)
export interface JwtPayload {
  // Claim "role" có thể là chuỗi hoặc mảng chuỗi (ví dụ: "Admin" hoặc ["Admin", "User"])
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role':
    | string
    | string[];
}
@Injectable({
  providedIn: 'root',
})
export class SharedService {
  //khai báo 1 số đường dẫn API
  readonly APIUrl = 'https://localhost:7105/api';

  constructor(private http: HttpClient) {}
  layDSPhongBan(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/PhongBan/GetAll');
  }
  layDSTaiKhoan(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/TaiKhoan/GetAll');
  }
  themPhongBan(val: any) {
    return this.http.post(this.APIUrl + '/PhongBan/Insert', val);
  }

  layDSDK(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/DonDK/GetAll');
  }
  xoaDonDK(id: any): Observable<any> {
    return this.http.delete(this.APIUrl + '/DonDK/Delete/' + id);
  }
  layBCcanhan(name: string, phongban: string) {
    return this.http.get<any[]>(
      this.APIUrl +
        '/ChiTietDonDK/GetbyName?name=' +
        name +
        '&maphongban=' +
        phongban
    );
  }
  layBCTheoCa(ngay: string, caAn: number) {
    return this.http.get<any[]>(
      this.APIUrl + '/ChiTietDonDK/GetbyCaAn?date=' + ngay + '&caAn=' + caAn
    );
  }
  layBCTheoThang(ngay: any) {
    return this.http.get<any[]>(
      this.APIUrl + '/ChiTietDonDK/GetbyMonth?date=' + ngay
    );
  }
  dangNhap(val: any): Observable<any[]> {
    return this.http.post<any>(this.APIUrl + '/TaiKhoan/Login', val);
  }
  // dangKyDonCaNhan(val: any): Observable<any[]> {
  //   return this.http.post<any>(this.APIUrl + '/DonDK/InsertOnly', val);
  // }
  layNhanVienTheoTenDangNhap(username: string): Observable<any> {
    return this.http.get<any>(
      this.APIUrl + '/NhanVien/GetByUsername?username=' + username
    );
  }
  // dangKyTapThe(body: any): Observable<any> {
  //   return this.http.post(this.APIUrl + '/DonDK/InsertFull', body);
  // }
  layThongTinTapThe(tenDangNhap: string): Observable<any> {
    return this.http.get<any>(
      this.APIUrl + '/NhanVien/LayThongTinTapThe/' + tenDangNhap
    );
  }
  layChiTietDon(idDon: string): Observable<any> {
    return this.http.get<any>(`${this.APIUrl}/DonDK/ChiTietDon?idDon=${idDon}`);
  }
  // capNhatDonCaNhan(idDon: string, donCaNhan: any) {
  //   const url = `${this.APIUrl}/DonDK/UpdateDonOnly?iD=${idDon}`;
  //   return this.http.put(url, donCaNhan);
  // }
  // capNhatDonTapThe(idDon: string, body: any) {
  //   return this.http.put(
  //     `${this.APIUrl}/DonDK/UpdateFull?Id_DonDK=${idDon}`,
  //     body
  //   );
  // }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }
  getToken(): string | null {
    return localStorage.getItem('token');
  }
  // bên trong SharedService

  login(req: LoginRequest): Observable<LoginRespon> {
    // Gửi POST request đến API đăng nhập với thông tin tài khoản (req)
    return this.http.post<LoginRespon>(`${this.APIUrl}/Auth/Login`, req).pipe(
      tap((res: LoginRespon) => {
        // ✅ Lưu token vào localStorage để dùng cho các request tiếp theo
        localStorage.setItem('token', res.token);
        // ✅ Giải mã token để lấy thông tin bên trong payload (dữ liệu người dùng)
        const payload = jwtDecode<{
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role':
            | string
            | string[];
        }>(res.token);
        // ✅ Trích xuất quyền từ payload với key chuẩn theo định dạng JWT
        const raw =
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ];
        // ✅ Nếu chỉ có 1 quyền thì ép thành mảng để đồng nhất xử lý
        const roles = Array.isArray(raw) ? raw : [raw];
        // ✅ Lưu danh sách quyền vào localStorage để tiện truy cập sau này
        localStorage.setItem('roles', JSON.stringify(roles));
      })
    );
  }

  getRoles(): string[] {
    return JSON.parse(localStorage.getItem('roles') || '[]');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('roles');
  }
}
