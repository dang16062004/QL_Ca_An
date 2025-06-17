import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-dsnhan-vien',
  standalone: false,
  templateUrl: './dsnhan-vien.component.html',
  styleUrl: './dsnhan-vien.component.css',
})
export class DSNhanVienComponent {
  DSNhanVien: any = [];
  NVienSelected: any;
  dangThemSua: boolean = false;
  tieuDe: any;
  isEdit: boolean = false;

  constructor(private service: SharedService) {}
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
    this.LoadDsNhanVien(); // Ẩn form thêm sửa
  }
  moFormThem() {
    this.NVienSelected = {
      ID_NhanVien: '', // Khởi tạo ID_NhanVien với chuỗi rỗng
      HoVaTen: '', // Khởi tạo HoVaTen với chuỗi rỗng
      Namsinh: '', // Khởi tạo Namsinh với chuỗi rỗng
      QDK: '', // Khởi tạo QDK với chuỗi rỗng
      PhanQuyen: '', // Khởi tạo PhanQuyen với chuỗi rỗng
      TenDangNhap: '', // Khởi tạo TenDangNhap với chuỗi rỗng
      ID_Phong: '', // Khởi tạo ID_Phong với chuỗi rỗng
      TenPhong: '', // Khởi tạo TenPhong với chuỗi rỗng
    }; // Đặt lại biến NVienSelected
    this.dangThemSua = true; // Hiển thị form thêm

    this.isEdit = false;
    this.tieuDe = 'Thêm Nhân Viên'; // Cập nhật tiêu đề
    this.LoadDsNhanVien();
  }
  moFormSua(nv: any) {
    this.NVienSelected = { ...nv };
    this.isEdit = true;
    this.tieuDe = 'Sửa Nhân Viên'; // Cập nhật tiêu đề
    this.dangThemSua = true;
  }
  xoaNhanVien(ma: any) {
    debugger;
    if (confirm('Bạn có chắc muốn xóa nhân viên này không?')) {
      this.service.xoaNhanVien(ma).subscribe({
        next: (res) => {
          alert('Đã xóa thành công');
          this.LoadDsNhanVien();
        },
        error: (err) => {
          console.error(err.toString());
          console.error('❌ Lỗi xóa nhân viên:', err);
          alert('Không tìm thấy nhân viên với ID: ' + ma);
        },
      });
    }
  }
}
