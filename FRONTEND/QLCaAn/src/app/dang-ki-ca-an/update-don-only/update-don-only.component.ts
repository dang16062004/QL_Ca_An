import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-update-don-only',
  standalone: false,
  templateUrl: './update-don-only.component.html',
  styleUrl: './update-don-only.component.css',
})
export class UpdateDonOnlyComponent implements OnInit {
  donCaNhan: any = {
    CaAn: 1,
    SoLuong: 1,
    LoaiDK: 'CANHAN',
    TenDangNhap: '',
  };

  idDon: string = '';
  name: any;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: SharedService
  ) {}

  ngOnInit(): void {
    this.idDon = this.route.snapshot.paramMap.get('id') || '';

    const username = localStorage.getItem('tenDangNhap');
    if (username) {
      this.donCaNhan.TenDangNhap = username.trim(); // Trim khoảng trắng nếu có
      console.log('✅ Username sử dụng:', this.donCaNhan.TenDangNhap);
    } else {
      alert('Không tìm thấy tài khoản đăng nhập');
      this.router.navigate(['/dang-ki-ca-nhan']);
    }
    this.name = username;
  }

  capNhatDon() {
    this.service.capNhatDonCaNhan(this.idDon, this.donCaNhan).subscribe({
      next: (res) => {
        if (typeof res === 'string' && res.startsWith('❌')) {
          alert(res); // ❌ báo lỗi trả về từ API
          return;
        }

        alert('✅ Đã cập nhật đơn đăng ký thành công');
        this.router.navigate(['/dang-ki-ca-nhan']);
      },
      error: (err) => {
        alert('❌ Lỗi cập nhật đơn đăng ký: ' + err.error);
      },
    });
  }
}
