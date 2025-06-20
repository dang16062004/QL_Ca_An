import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';
import { Input } from '@angular/core';

@Component({
  selector: 'app-them-sua-taikhoan',
  standalone: false,
  templateUrl: './them-sua-taikhoan.component.html',
  styleUrl: './them-sua-taikhoan.component.css',
})
export class ThemSuaTaikhoanComponent {
  ID_TaiKhoan: any;
  TenDangNhap: any;
  MatKhau: any;
  @Input() checked2: any; // nhaanj dwx lieeuj twf dstaikhoan.ts
  constructor(private service: SharedService) {}
  ngOnInit(): void {
    this.TenDangNhap = this.taiKhoanSelected.TenDangNhap;
    this.MatKhau = this.taiKhoanSelected.MatKhau;
    this.ID_TaiKhoan = this.taiKhoanSelected.ID_TaiKhoan;
  }

  //Ngaytao: any; ngày tạo được tạo tự động
  @Input() taiKhoanSelected: any;
  // Nhận dữ liệu từ component cha
  themTaiKhoanMoi() {
    var val = {
      ID_TaiKhoan: this.ID_TaiKhoan,
      TenDangNhap: this.TenDangNhap,
      MatKhau: this.MatKhau,
      NgayTao: new Date(), // Ngày tạo được tạo tự động
    };

    this.service.themtaiKhoan(val).subscribe((res) => {
      alert(res.toString());
    });
  }
  suaTaiKhoancu() {
    var val = {
      ID_TaiKhoan: this.ID_TaiKhoan,
      TenDangNhap: this.TenDangNhap,
      MatKhau: this.MatKhau,
      NgayTao: this.taiKhoanSelected.NgayTao, // Giữ nguyên ngày tạo từ tài khoản đã chọn
    };
    this.service.suaTaiKhoan(val).subscribe((res) => {
      alert(res.toString());
      // Sau khi sửa thành công, có thể thực hiện các hành động khác như đóng form hoặc nạp lại danh sách tài khoản
      // this.dongForm(); // Nếu bạn muốn đóng form sau khi sửa
      // this.loadDanhsachTaiKhoan(); // Nạp lại danh sách tài khoản
    });
  }
}
