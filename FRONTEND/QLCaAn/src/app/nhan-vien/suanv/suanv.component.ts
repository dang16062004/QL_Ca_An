import { Component, Input, input, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';
import { AccountService } from '../../account.service';
import { Route, Router } from '@angular/router';
import { FormBuilder, Validator, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { UpNhanVienDTO } from '../nhanvien-model';
import { errorContext } from 'rxjs/internal/util/errorContext';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-suanv',
  standalone: false,
  templateUrl: './suanv.component.html',
  styleUrl: './suanv.component.css',
})
export class suanvComponent implements OnInit {
  qdks = ['TapThe', 'CaNhan'] as const;
  phongBans: { idPhong: number; tenPhong: string }[] = [];
  roles = ['Admin', 'User'] as const;

  form!: FormGroup;
  constructor(
    private activatedRoute: ActivatedRoute,
    private route: Router,
    private as: AccountService,
    private sv: SharedService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('idNhanVien') ?? '';
    //khai báo các tham số nhập vào
    this.form = this.fb.group({
      ID_NhanVien: [id, Validators.required], // ← thêm control này
      HoVaTen: ['', Validators.required],
      NamSinh: ['', Validators.required],
      TenDangNhap: ['', Validators.required],
      MatKhau: ['', Validators.required],
      ID_Phong: [null, Validators.required],
      PhanQuyen: ['User', Validators.required],
      QDK: ['CaNhan', Validators.required],
    });
    //Load danh sach phong
    this.sv.layDSPhongBan().subscribe((res) => {
      this.phongBans = res.map((p: any) => ({
        idPhong: p.id_Phong || p.ID_Phong,
        tenPhong: p.TenPhong || p.tenPhong,
      }));
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    const dto: UpNhanVienDTO = {
      idNhanVien: this.form.value.ID_NhanVien,
      hoVaTen: this.form.value.HoVaTen,
      namSinh: this.form.value.NamSinh,
      tenDangNhap: this.form.value.TenDangNhap,
      matKhau: this.form.value.MatKhau,
      id_Phong: this.form.value.ID_Phong,
      phanQuyen: this.form.value.PhanQuyen,
      qdk: this.form.value.QDK,
    };
    this.as.editNhanVien(dto).subscribe({
      next: () => {
        alert('Sửa nhân viên thành công');
        this.route.navigate(['/nhanvien']);
      },
      error: (err) => {
        alert(err.error ?? 'Lỗi sửa');
      },
    });
  }
}
