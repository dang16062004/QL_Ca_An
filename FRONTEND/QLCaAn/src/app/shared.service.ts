import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  themtaiKhoan(val: any) {
    return this.http.post(this.APIUrl + '/TaiKhoan/Insert', val);
  }
  dangNhap(val: any): Observable<any[]> {
    return this.http.post<any>(this.APIUrl + '/TaiKhoan/Login', val);
  }
  suaPhongBan(val: any) {
    return this.http.put(this.APIUrl + '/PhongBan/Update', val);
  }
  suaTaiKhoan(val: any) {
    return this.http.put(this.APIUrl + '/TaiKhoan/Update', val);
  }
  xoaPhongBan(id: string): Observable<{ success: boolean; message: string }> {
    return this.http.delete<{ success: boolean; message: string }>(
      `${this.APIUrl}/PhongBan/Delete/${id}`
    );
  }
  xoaTaiKhoan(id: string): Observable<{ success: boolean; message: string }> {
    return this.http.delete<{ success: boolean; message: string }>(
      `${this.APIUrl}/TaiKhoan/Delete/${id}`
    );
  }
  layDSNhanVien(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/NhanVien/GetAll');
  }
  themNhanVien(val: any) {
    return this.http.post(this.APIUrl + '/NhanVien/Insert', val);
  }

  suaNhanVien(val: any) {
    return this.http.put(this.APIUrl + '/NhanVien/Update', val);
  }

  xoaNhanVien(id: string): Observable<{ success: boolean; message: string }> {
    return this.http.delete<{ success: boolean; message: string }>(
      `${this.APIUrl}/NhanVien/Delete/${id}`
    );
  }
  // xoaNhanVien(id: any): Observable<any> {
  //   return this.http.delete(this.APIUrl + '/NhanVien/Delete/' + id);
  // }
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
}
