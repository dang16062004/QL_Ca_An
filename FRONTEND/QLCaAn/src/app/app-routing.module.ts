import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
import { TaiKhoanComponent } from './tai-khoan/tai-khoan.component';
import { NhanVienComponent } from './nhan-vien/nhan-vien.component';
import { DangnhapComponent } from './tai-khoan/dangnhap/dangnhap.component';
import { DangKiCaAnComponent } from './dang-ki-ca-an/dang-ki-ca-an.component';
import { BaocaoComponent } from './baocao/baocao.component';
import { TheocanhanComponent } from './baocao/theocanhan/theocanhan.component';
import { BaocaotheocaComponent } from './baocao/baocaotheoca/baocaotheoca.component';
import { BaocaotheothangComponent } from './baocao/baocaotheothang/baocaotheothang.component';
import { DSDKComponent } from './dang-ki-ca-an/dsdk/dsdk.component';
const routes: Routes = [
  { path: '', component: DangnhapComponent },
  { path: 'phongban', component: PhongBanComponent },
  { path: 'taikhoan', component: TaiKhoanComponent },
  { path: 'nhanvien', component: NhanVienComponent },
  { path: 'dondangki', component: DangKiCaAnComponent },
  { path: 'baocao', component: BaocaoComponent },
  { path: 'baocao/theocanhan', component: TheocanhanComponent },
  { path: 'baocao/theoca', component: BaocaotheocaComponent },
  { path: 'baocao/theothang', component: BaocaotheothangComponent },
  { path: 'dang-ki-ca-nhan', component: DSDKComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
