import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  //khai báo 1 số đường dẫn API
  readonly APIUrl = "https://localhost:7105";

  constructor(private http:HttpClient) { }
  layDSPhongBan():Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + '/PhongBan/GetAll');
  }
  layDSTaiKhoan():Observable<any[]>
  {
    return this.http.get<any[]>(this.APIUrl+'/TaiKhoan/GetAll')
  }
  themPhongBan(val :any)
  {
    return this.http.post(this.APIUrl+'/PhongBan/Insert',val)
  }
}
