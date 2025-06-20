import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-themcanhan',
  standalone: false,
  templateUrl: './themcanhan.component.html',
  styleUrl: './themcanhan.component.css',
})
export class ThemcanhanComponent implements OnInit {
  form = {
    CaAn: '',
    SoLuong: 1,
  };
  hoVaTen: string = '';

  ngOnInit() {
    const tenDangNhap = localStorage.getItem('tenDangNhap');
    if (!tenDangNhap) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['/login']);
      return;
    }

    this.service.layNhanVienTheoTenDangNhap(tenDangNhap).subscribe(
      (res) => {
        this.hoVaTen = res['HoVaTen'];
        console.log('Lấy thông tin với username:', tenDangNhap);
        this.name = tenDangNhap;
      },
      (err) => {
        console.error('Lỗi lấy tên nhân viên:', err);
      }
    );
  }
  name: any;
  constructor(private service: SharedService, private router: Router) {}

  dangKyDonCaNhan() {
    const tenDangNhap = localStorage.getItem('tenDangNhap');
    if (!tenDangNhap) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['/login']);
      return;
    }

    const request = {
      TenDangNhap: tenDangNhap,
      LoaiDK: 'CANHAN',
      CaAn: this.form.CaAn,
      SoLuong: this.form.SoLuong,
    };

    this.service.dangKyDonCaNhan(request).subscribe(
      (res: any) => {
        alert(res);
        this.router.navigate(['/dang-ki-ca-nhan']); // quay lại danh sách sau khi thêm
      },
      (err: any) => {
        console.error('Lỗi đăng ký:', err);
        alert('Không thể thêm đơn');
      }
    );
  }
}
