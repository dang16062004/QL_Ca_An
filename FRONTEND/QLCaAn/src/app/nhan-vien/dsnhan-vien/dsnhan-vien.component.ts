import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { AccountService } from '../../account.service';

@Component({
  selector: 'app-dsnhan-vien',
  standalone: false,
  templateUrl: './dsnhan-vien.component.html',
  styleUrls: ['./dsnhan-vien.component.css'], // ✅ sửa đúng từ 'styleUrl'
})
export class DSNhanVienComponent {
  DSNhanVien: any = [];
  NVienSelected: any;
  dangThemSua: boolean = false;
  tieuDe: string = '';
  isEdit: boolean = false;

  constructor(private service: AccountService) {}

  ngOnInit(): void {
    this.LoadDsNhanVien();
  }

  LoadDsNhanVien() {
    this.service.layDSNhanVien().subscribe((data) => {
      this.DSNhanVien = data;
    });
  }

  dongForm() {
    this.dangThemSua = false;
    this.LoadDsNhanVien();
  }

  moFormThem() {
    this.NVienSelected = {
      ID_NhanVien: '',
      HoVaTen: '',
      NamSinh: '',
      QDK: '',
      PhanQuyen: '',
      TenDangNhap: '',
      ID_Phong: '',
      TenPhong: '',
    };
    this.dangThemSua = true;
    this.isEdit = false;
  }

  XoaNhanVien(nv: any) {
    if (!confirm(`Bạn có muốn xóa nhân viên ${nv.HoVaTen}?`)) return;

    this.service.deleteNhanVien(nv.ID_NhanVien).subscribe({
      next: (msg) => {
        alert(msg);
        alert('Xóa thành công!');
        this.LoadDsNhanVien();
      },
      error: (err) => {
        console.error('Error response:', err);
        // Hiển thị rõ mã lỗi và thông báo từ server
        alert(`Lỗi ${err.status}: ${err.error || err.message}`);
      },
    });
  }
}
