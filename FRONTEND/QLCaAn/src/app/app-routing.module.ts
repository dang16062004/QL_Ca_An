import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
import { TaiKhoanComponent } from './tai-khoan/tai-khoan.component';
import { NhanVienComponent } from './nhan-vien/nhan-vien.component';
import { DangnhapComponent } from './tai-khoan/dangnhap/dangnhap.component';
const routes: Routes = [
  { path: '', component: DangnhapComponent },
  { path: 'phongban', component: PhongBanComponent },
  { path: 'taikhoan', component: TaiKhoanComponent },
  { path: 'nhanvien', component: NhanVienComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
