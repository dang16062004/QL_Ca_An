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
}
