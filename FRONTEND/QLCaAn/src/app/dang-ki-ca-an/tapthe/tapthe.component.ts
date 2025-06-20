import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tapthe',
  standalone: false,
  templateUrl: './tapthe.component.html',
  styleUrls: ['./tapthe.component.css'],
})
export class ThemTapTheComponent implements OnInit {
  tenDangNhap = '';
  hoVaTen = '';
  tenPhong = '';
  caAn: number = 1;

  danhSachNhanVien: any[] = [];

  constructor(private service: SharedService, private router: Router) {}

  ngOnInit(): void {
    const tenDangNhap = localStorage.getItem('tenDangNhap');
    if (!tenDangNhap) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['/login']);
      return;
    }

    this.tenDangNhap = tenDangNhap;

    this.service.layThongTinTapThe(tenDangNhap).subscribe(
      (res: any) => {
        this.hoVaTen = res.hoVaTen;
        this.tenPhong = res.tenPhong;
        this.danhSachNhanVien = res.danhSachNhanVien.map((nv: any) => ({
          ...nv,
          soLuong: 0,
        }));
      },
      (err) => {
        console.error('Lỗi khi lấy thông tin:', err);
        alert('Không thể tải thông tin nhân viên');
      }
    );
  }

  dangKy(): void {
    const chiTiet = this.danhSachNhanVien
      .filter((nv) => nv[`ca${this.caAn}`] > 0)
      .map((nv) => ({
        ID_NhanVien: nv.ID_NhanVien,
        SoLuong: nv[`ca${this.caAn}`],
      }));

    if (chiTiet.length === 0) {
      alert('Vui lòng nhập số lượng cho ít nhất một nhân viên');
      return;
    }

    const request = {
      tenDangNhap: this.tenDangNhap,
      donDK: {
        LoaiDK: 'TAPTHE',
        CaAn: this.caAn,
      },
      listChiTiet: chiTiet,
    };

    this.service.dangKyTapThe(request).subscribe({
      next: (res) => {
        alert(res);
        this.router.navigate(['/dang-ki-ca-an']);
      },
      error: (err) => {
        console.error('Lỗi đăng ký:', err);
        alert('Không thể đăng ký');
      },
    });
  }
}
