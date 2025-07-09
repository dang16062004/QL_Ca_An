import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
export interface JwtPayload {
  // Claim "role" có thể là chuỗi hoặc mảng chuỗi (ví dụ: "Admin" hoặc ["Admin", "User"])
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role':
    | string
    | string[];
}
@Injectable({
  providedIn: 'root',
})
export class DondkService {
  readonly APIUrl = 'https://localhost:7105/api';
  constructor(private http: HttpClient) {}
  layDSDK(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/DonDK/GetAll');
  }
  xoaDK(id: any): Observable<any> {
    const token = localStorage.getItem('token'); //lấy ra JWT
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`); //	Gắn JWT vào header
    return this.http.delete<any>(this.APIUrl + `/DonDK/Delete?iD_Don=${id}`, {
      //Gửi request xóa đến API
      headers,
      responseType: 'text' as 'json', //	Ép Angular không lỗi khi backend trả chuỗi "Delete Success"
    });
  }
}
