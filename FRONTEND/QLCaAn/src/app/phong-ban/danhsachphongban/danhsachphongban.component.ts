import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-danhsachphongban',
  standalone: false,
  templateUrl: './danhsachphongban.component.html',
  styleUrl: './danhsachphongban.component.css',
})
export class DanhsachphongbanComponent {
  DSPhongBan: any = [];
  PBanSelected: any;
  dangThemSua: boolean = false;
  tieuDe: any;
  checked: boolean = false;

  id = localStorage.getItem('ID_NhanVien');
  constructor(private service: SharedService, private router: Router) {}
  ngOnInit(): void {
    if (!this.id) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['']);
      return;
    }
    this.LoadDsPhongBan();
  }
  LoadDsPhongBan() {
    this.service.layDSPhongBan().subscribe((data) => {
      this.DSPhongBan = data;
    });
  }
  // PBan1:khai báo 1 biến để nhận dữ liệu từ danh sách phòng ban
  chitietPhongBan(PBan1: any) {
    this.PBanSelected = PBan1;

    this.dangThemSua = true; // Hiển thị form thêm sửa
    this.checked = true;
    this.tieuDe = 'Sửa Phòng Ban'; // Cập nhật tiêu đề
  }
  dongForm() {
    this.dangThemSua = false;
    this.LoadDsPhongBan(); // Ẩn form thêm sửa
  }
  moFormThem() {
    this.PBanSelected = {
      ID_Phong: '', // Khởi tạo ID_Phong với chuỗi rỗng
      TenPhong: '', // Khởi tạo TenPhong với chuỗi rỗng
    }; // Đặt lại biến PBanSelected
    this.dangThemSua = true; // Hiển thị form thêm
    this.checked = false;
    this.tieuDe = 'Thêm Phòng Ban'; // Cập nhật tiêu đề
    this.LoadDsPhongBan();
  }
}
