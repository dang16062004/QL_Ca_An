import { Injectable } from '@angular/core';
import { AdminGuard } from './auth/admin.guard';
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
export class AccountService {
  readonly APIUrl = 'https://localhost:7105/api';
  constructor(private http: HttpClient) {}
  layDSNhanVien(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/NhanVien/GetAll');
  }
  createNhanVien(nv: NhanVienDTO): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.post(this.APIUrl + '/NhanVien/Create', nv, { headers });
  }
  deleteNhanVien(id: number): Observable<string> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.delete(
      `${this.APIUrl}/NhanVien/Delete?iD_NhanVien=${id}`,
      { headers, responseType: 'text' as const }
    );
  }
  editNhanVien(nv: NhanVienDTO): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.put(`${this.APIUrl}/NhanVien/UpdateNhanVien`, nv, {
      headers,
    });
  }
}
