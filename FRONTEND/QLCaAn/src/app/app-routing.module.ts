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
import { ThemcanhanComponent } from './dang-ki-ca-an/themcanhan/themcanhan.component';
import { ThemTapTheComponent } from './dang-ki-ca-an/tapthe/tapthe.component';
import { DstaptheComponent } from './dang-ki-ca-an/dstapthe/dstapthe.component';
import { ChiTietDonComponent } from './dang-ki-ca-an/chi-tiet-don/chi-tiet-don.component';
import { UpdateDonOnlyComponent } from './dang-ki-ca-an/update-don-only/update-don-only.component';
import { UpdateDonFullComponent } from './dang-ki-ca-an/update-don-full/update-don-full.component';
import { LoginComponent } from './login/login.component';
import { CreateNhanVienComponent } from './nhan-vien/create-nhanvien/create-nhanvien.component';
import { AdminGuard } from './auth/admin.guard';
import { suanvComponent } from './nhan-vien/suanv/suanv.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'phongban', component: PhongBanComponent },
  { path: 'taikhoan', component: TaiKhoanComponent },
  { path: 'nhanvien', component: NhanVienComponent },
  { path: 'dondangki', component: DangKiCaAnComponent },
  { path: 'baocao', component: BaocaoComponent },
  { path: 'baocao/theocanhan', component: TheocanhanComponent },
  { path: 'baocao/theoca', component: BaocaotheocaComponent },
  { path: 'baocao/theothang', component: BaocaotheothangComponent },
  { path: 'dang-ki-ca-nhan', component: DSDKComponent },
  { path: 'themcanhan', component: ThemcanhanComponent },
  { path: 'dktapthe', component: ThemTapTheComponent },
  { path: 'dsTapThe', component: DstaptheComponent },
  { path: 'chitietdon/:id', component: ChiTietDonComponent },

  { path: 'capnhat-don/:id', component: UpdateDonOnlyComponent },
  { path: 'capnhat-don-tapthe/:id', component: UpdateDonFullComponent },
  {
    path: 'nhanvien/createNhanVien',
    component: CreateNhanVienComponent,
    canActivate: [AdminGuard],
  },
  { path: 'nhanvien/suaNhanVien', component: suanvComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
