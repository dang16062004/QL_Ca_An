// src/app/nhan-vien/create-nhanvien/create-nhanvien.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedService } from '../../shared.service';
import { NhanVienDTO } from '../nhanvien-model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AccountService } from '../../account.service';

@Component({
  selector: 'app-create-nhanvien',
  standalone: true,
  templateUrl: './create-nhanvien.component.html',
  styleUrls: ['./create-nhanvien.component.css'],
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class CreateNhanVienComponent implements OnInit {
  form!: FormGroup;
  // chọn option
  roles = ['Admin', 'User'] as const;
  qdks = ['CaNhan', 'TapThe'] as const;
  phongBans: { id_Phong: number; tenPhong: string }[] = [];

  constructor(
    private fb: FormBuilder,
    private st: SharedService,
    private sv: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      HoVaTen: ['', Validators.required],
      NamSinh: ['', Validators.required],
      TenDangNhap: ['', Validators.required],
      MatKhau: ['', Validators.required],
      ID_Phong: [null, Validators.required],
      PhanQuyen: ['User', Validators.required], // default
      QDK: ['CaNhan', Validators.required], // default
    });

    // lấy danh sách phòng ban để đổ dropdown
    this.st.layDSPhongBan().subscribe((res) => {
      this.phongBans = res.map((p: any) => ({
        id_Phong: p.id_Phong || p.ID_Phong,
        tenPhong: p.tenPhong || p.TenPhong,
      }));
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const dto: NhanVienDTO = {
      hoVaTen: this.form.value.HoVaTen,
      namSinh: this.form.value.NamSinh,
      tenDangNhap: this.form.value.TenDangNhap,
      matKhau: this.form.value.MatKhau,
      id_Phong: this.form.value.ID_Phong,
      phanQuyen: this.form.value.PhanQuyen,
      qdk: this.form.value.QDK,
    };

    this.sv.createNhanVien(dto).subscribe({
      next: () => {
        alert('Tạo nhân viên thành công!');
        this.router.navigate(['/nhanvien']); // về danh sách nhân viên
      },
      error: (err) => {
        alert(err.error ?? 'Lỗi khi tạo nhân viên');
        console.error(err);
      },
    });
  }
}
