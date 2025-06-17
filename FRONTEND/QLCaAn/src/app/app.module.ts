import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
import { DanhsachphongbanComponent } from './phong-ban/danhsachphongban/danhsachphongban.component';
import { FormsModule ,ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SharedService } from './shared.service';
import { TaiKhoanComponent } from './tai-khoan/tai-khoan.component';
import { DSTaiKhoanComponent } from './tai-khoan/dstai-khoan/dstai-khoan.component';
import { ThemSuaphongbanComponent } from './phong-ban/them-suaphongban/them-suaphongban.component';
import { ThemSuaTaikhoanComponent } from './tai-khoan/them-sua-taikhoan/them-sua-taikhoan.component';
import { NhanVienComponent } from './nhan-vien/nhan-vien.component';
import { DSNhanVienComponent } from './nhan-vien/dsnhan-vien/dsnhan-vien.component';
import { ThemSuaNhanVienComponent } from './nhan-vien/them-sua-nhan-vien/them-sua-nhan-vien.component';

@NgModule({
  declarations: [
    AppComponent,
    PhongBanComponent,
    DanhsachphongbanComponent,
    TaiKhoanComponent,
    DSTaiKhoanComponent,
    ThemSuaphongbanComponent,
    ThemSuaTaikhoanComponent,
    NhanVienComponent,
    DSNhanVienComponent,
    ThemSuaNhanVienComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
