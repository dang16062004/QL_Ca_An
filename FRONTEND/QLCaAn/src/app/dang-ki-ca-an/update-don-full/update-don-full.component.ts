import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-update-don-full',
  standalone: false,
  templateUrl: './update-don-full.component.html',
  styleUrl: './update-don-full.component.css',
})
export class UpdateDonFullComponent implements OnInit {
  idDon: string = '';
  donDK: any = {
    CaAn: 1,
    LoaiDK: 'TAPTHE',
  };

  listChiTiet: any[] = [];
  tenDangNhap: string = '';
  name: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: SharedService
  ) {}

  ngOnInit(): void {
    this.idDon = this.route.snapshot.paramMap.get('id') || '';
    const username = localStorage.getItem('tenDangNhap');

    if (!username) {
      alert('❌ Không tìm thấy tài khoản đăng nhập');
      this.router.navigate(['/dang-ki-ca-nhan']);
      return;
    }

    this.tenDangNhap = username.trim();
    this.loadChiTietDon();
    this.name = username;
  }

  loadChiTietDon() {
    this.service.layChiTietDon(this.idDon).subscribe({
      next: (res) => {
        console.log('✅ Chi tiết trả về:', res);
        console.log('👉 Dữ liệu 1 dòng:', res[0]);
        this.listChiTiet = Array.isArray(res) ? res : Object.values(res);
      },
      error: (err) => {
        console.error('❌ Lỗi khi tải chi tiết đơn:', err);
        alert('❌ Lỗi khi tải chi tiết đơn: ' + err.error);
      },
    });
  }

  capNhatDonTapThe() {
    const requestBody = {
      TenDangNhap: this.tenDangNhap,
      donDK: this.donDK,
      listChiTiet: this.listChiTiet,
    };

    this.service.capNhatDonTapThe(this.idDon, requestBody).subscribe({
      next: (res) => {
        if (typeof res === 'string' && res.startsWith('❌')) {
          alert(res);
        } else {
          alert('✅ Cập nhật đơn tập thể thành công');
          this.router.navigate(['/dsTapThe']);
        }
      },
      error: (err) => {
        alert('❌ Lỗi cập nhật đơn: ' + err.error);
      },
    });
  }
}
