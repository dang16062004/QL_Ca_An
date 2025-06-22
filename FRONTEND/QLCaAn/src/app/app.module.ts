import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
import { DanhsachphongbanComponent } from './phong-ban/danhsachphongban/danhsachphongban.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SharedService } from './shared.service';
import { TaiKhoanComponent } from './tai-khoan/tai-khoan.component';
import { DSTaiKhoanComponent } from './tai-khoan/dstai-khoan/dstai-khoan.component';
import { ThemSuaphongbanComponent } from './phong-ban/them-suaphongban/them-suaphongban.component';
import { ThemSuaTaikhoanComponent } from './tai-khoan/them-sua-taikhoan/them-sua-taikhoan.component';
import { NhanVienComponent } from './nhan-vien/nhan-vien.component';
import { DSNhanVienComponent } from './nhan-vien/dsnhan-vien/dsnhan-vien.component';
import { ThemSuaNhanVienComponent } from './nhan-vien/them-sua-nhan-vien/them-sua-nhan-vien.component';
import { DangnhapComponent } from './tai-khoan/dangnhap/dangnhap.component';
import { DangKiCaAnComponent } from './dang-ki-ca-an/dang-ki-ca-an.component';
import { DSDKComponent } from './dang-ki-ca-an/dsdk/dsdk.component';
import { BaocaoComponent } from './baocao/baocao.component';
import { TheocanhanComponent } from './baocao/theocanhan/theocanhan.component';
import { BaocaotheocaComponent } from './baocao/baocaotheoca/baocaotheoca.component';
import { BaocaotheothangComponent } from './baocao/baocaotheothang/baocaotheothang.component';
import { ThemcanhanComponent } from './dang-ki-ca-an/themcanhan/themcanhan.component';
import { ThemTapTheComponent } from './dang-ki-ca-an/tapthe/tapthe.component';
import { DstaptheComponent } from './dang-ki-ca-an/dstapthe/dstapthe.component';
import { ChiTietDonComponent } from './dang-ki-ca-an/chi-tiet-don/chi-tiet-don.component';
import { UpdateDonOnlyComponent } from './dang-ki-ca-an/update-don-only/update-don-only.component';
import { UpdateDonFullComponent } from './dang-ki-ca-an/update-don-full/update-don-full.component';

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
    ThemSuaNhanVienComponent,
    DangnhapComponent,
    DangKiCaAnComponent,
    DSDKComponent,
    BaocaoComponent,
    TheocanhanComponent,
    BaocaotheocaComponent,
    BaocaotheothangComponent,
    ThemcanhanComponent,
    ThemTapTheComponent,
    DstaptheComponent,
    ChiTietDonComponent,
    UpdateDonOnlyComponent,
    UpdateDonFullComponent,
  ],
  imports: [
    RouterModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
