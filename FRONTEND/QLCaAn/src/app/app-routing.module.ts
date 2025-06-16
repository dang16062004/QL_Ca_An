import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
import { TaiKhoanComponent } from './tai-khoan/tai-khoan.component';
const routes: Routes = [
  {path:'phongban',component:PhongBanComponent},
  {path:'taikhoan',component:TaiKhoanComponent}
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
