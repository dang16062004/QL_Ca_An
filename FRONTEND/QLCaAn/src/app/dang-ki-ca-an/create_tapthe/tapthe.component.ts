import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';

import { SharedService } from '../../shared.service';
import { DondkService } from '../../dondk.service';
import { DonFullRequest } from '../dangki.model';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-tapthe',
  standalone: false,
  templateUrl: './tapthe.component.html',
  styleUrls: ['./tapthe.component.css'],
})
export class ThemTapTheComponent implements OnInit {
  hoVaTen = '';
  danhSachNhanVienCungPhong: any[] = [];

  /** Khai báo form gốc */
  form!: FormGroup;
  /** Getter tiện truy cập FormArray trong TS & HTML */
  get listChiTiet(): FormArray {
    return this.form.get('listChiTiet') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private service: SharedService,
    private dk: DondkService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.hoVaTen = localStorage.getItem('HoVaTen') ?? '';
    const idNhanVienLogin = localStorage.getItem('ID_NhanVien');
    if (!idNhanVienLogin) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['/login']);
      return;
    }

    /* khởi tạo form trước (rỗng) */
    this.form = this.fb.group({
      CaAn: ['', Validators.required],
      listChiTiet: this.fb.array([]), // sẽ thêm phần tử sau
    });

    this.LoadDSNhanVien();
  }

  /** Lấy danh sách NV cùng phòng và gắn vào FormArray */
  LoadDSNhanVien(): void {
    this.dk.LayThongTinNhanVien().subscribe((nvArr) => {
      this.danhSachNhanVienCungPhong = nvArr;

      /* clear trước nếu cần */
      this.listChiTiet.clear();

      /* tạo FormGroup con cho MỖI nhân viên */
      nvArr.forEach((nv) => {
        this.listChiTiet.push(
          this.fb.group({
            ID_NhanVien: [nv.ID_NhanVien],
            SoLuong: [1, [Validators.required, Validators.min(1)]],
            Selected: [false],
          })
        );
      });
    });
  }

  /** Gửi request đăng ký */
  dangKy(): void {
    if (this.form.invalid) return;

    /* Lọc những NV được tick Selected */
    const chiTiet = this.listChiTiet.value
      .filter((ct: any) => ct.Selected)
      .map((ct: any) => ({
        ID_NhanVien: Number(ct.ID_NhanVien),
        SoLuong: Number(ct.SoLuong),
      }));

    if (chiTiet.length === 0) {
      alert('Bạn chưa chọn nhân viên nào!');
      return;
    }

    const request: DonFullRequest = {
      donDK: {
        LoaiDK: 'TapThe',
        CaAn: Number(this.form.value.CaAn),
      },
      listChiTiet: chiTiet,
    };

    this.dk.InsertFull(request).subscribe(
      (res) => {
        alert(res);
        this.router.navigate(['/dsTapThe']);
      },
      (err) => {
        alert('Lỗi');
        console.error(err);
      }
    );
  }
}
