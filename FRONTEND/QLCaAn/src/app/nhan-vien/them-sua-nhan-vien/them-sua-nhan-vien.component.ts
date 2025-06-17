import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';
import { Input } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnChanges } from '@angular/core';

@Component({
  selector: 'app-them-sua-nhan-vien',
  standalone: false,
  templateUrl: './them-sua-nhan-vien.component.html',
  styleUrl: './them-sua-nhan-vien.component.css',
})
export class ThemSuaNhanVienComponent implements OnChanges, OnInit {
  nhanVien: any = {
    ID_NhanVien: '',
    HoVaTen: '',
    Namsinh: '',
    QDK: '',
    PhanQuyen: '',
    TenDangNhap: '',
    ID_Phong: '',
  };

  DSPhongBan: any[] = []; // hoặc khai báo kiểu rõ ràng nếu có interface
  DSTaiKhoan: any[] = []; // Danh sách tài khoản

  @Input() NVienSelected: any;
  @Input() isEdit: boolean = false;
  TieuDe: string = ''; // Tiêu đề mặc định

  constructor(private service: SharedService) {}
  ngOnInit(): void {
    this.loadDSPhongBan();
    this.loadTaiKhoan();
  }
  tatCaTaiKhoan: any[] = [];
  taiKhoanChuaGan: any[] = [];

  loadTaiKhoan() {
    this.service.layDSTaiKhoan().subscribe((all) => {
      this.service.layDSNhanVien().subscribe((nhanViens) => {
        const daGan = nhanViens.map((nv) => nv.TenDangNhap);
        this.taiKhoanChuaGan = all.filter(
          (tk) => !daGan.includes(tk.TenDangNhap)
        );
      });
    });
  }
  themNhanVienmoi() {
    // this.checked=true;
    this.service.themNhanVien(this.nhanVien).subscribe((res) => {
      alert(res.toString());
      if (res === 'Success Insert') {
        this.resetForm();
        this.loadTaiKhoan(); // reload lại danh sách tài khoản chưa gán
      }
    });
  }
  resetForm() {
    this.nhanVien = {
      ID_NhanVien: '',
      HoVaTen: '',
      Namsinh: '',
      QDK: '',
      PhanQuyen: '',
      TenDangNhap: '',
      ID_Phong: '',
    };
  }
  suaNhanVienmoi() {
    debugger;
    this.service.suaNhanVien(this.nhanVien).subscribe((res) => {
      alert(res.toString());
    });
  }

  loadDSPhongBan() {
    this.service.layDSPhongBan().subscribe((data) => {
      this.DSPhongBan = data;
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['NVienSelected'] && this.NVienSelected) {
      this.nhanVien = { ...this.NVienSelected };
    }
  }
}
