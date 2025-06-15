import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
const routes: Routes = [
  {path:'phongban',component:PhongBanComponent}
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
