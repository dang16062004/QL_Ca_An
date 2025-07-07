import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRespon } from './models/login-respon.models';
import { LoginRequest } from './models/login-request.models';
import { NonNullAssert } from '@angular/compiler';
import { NhanVienDTO } from './nhan-vien/nhanvien-model';
import { jwtDecode } from 'jwt-decode';

export interface JwtPayload {
  // Claim "role" có thể là chuỗi hoặc mảng chuỗi (ví dụ: "Admin" hoặc ["Admin", "User"])
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role':
    | string
    | string[];
}
@Injectable({
  providedIn: 'root',
})
export class DangKiCaService {
  readonly APIUrl = 'https://localhost:7105/api';

  constructor(private http: HttpClient) {}
  // dangKyDonCaNhan(val: any): Observable<any[]> {
  //   return this.http.post<any>(this.APIUrl + '/DonDK/InsertOnly', val);

  dangKyDonCaNhan(val: any): Observable<any> {
    return this.http.post<any>(this.APIUrl + 'DonDK/InsertOnly', val);
  }
  // }// dangKyTapThe(body: any): Observable<any> {
  //   return this.http.post(this.APIUrl + '/DonDK/InsertFull', body);
  // } // capNhatDonCaNhan(idDon: string, donCaNhan: any) {
  //   const url = `${this.APIUrl}/DonDK/UpdateDonOnly?iD=${idDon}`;
  //   return this.http.put(url, donCaNhan);
  // }
  // capNhatDonTapThe(idDon: string, body: any) {
  //   return this.http.put(
  //     `${this.APIUrl}/DonDK/UpdateFull?Id_DonDK=${idDon}`,
  //     body
  //   );
  // }
}
