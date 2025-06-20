import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-themcanhan',
  templateUrl: './themcanhan.component.html',
  styleUrls: ['./themcanhan.component.css'],
})
export class ThemcanhanComponent {
  // form = {
  //   CaAn: '',
  //   SoLuong: 1,
  // };
  // constructor(private service: SharedService, private router: Router) {}
  // dangKyDonCaNhan() {
  //   const tenDangNhap = localStorage.getItem('tenDangNhap');
  //   if (!tenDangNhap) {
  //     alert('❌ Bạn chưa đăng nhập');
  //     this.router.navigate(['/login']);
  //     return;
  //   }
  //   const request = {
  //     TenDangNhap: tenDangNhap,
  //     LoaiDK: 'CANHAN',
  //     CaAn: this.form.CaAn,
  //     SoLuong: this.form.SoLuong,
  //   };
  //   this.service.dangKyDonCaNhan(request).subscribe(
  //     (res) => {
  //       alert(res);
  //     },
  //     (err) => {
  //       console.error(err);
  //       alert('❌ Lỗi khi gửi đăng ký');
  //     }
  //   );
  // }
}
