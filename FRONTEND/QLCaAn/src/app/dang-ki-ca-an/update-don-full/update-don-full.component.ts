import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';

import { SharedService } from '../../shared.service';
import { DondkService } from '../../dondk.service';
import { DonFullRequest } from '../dangki.model';
import { NgModel } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-update-don-full',
  standalone: false,
  templateUrl: './update-don-full.component.html',
  styleUrl: './update-don-full.component.css',
})
export class UpdateDonFullComponent implements OnInit {
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
    private router: Router,
    private ra: ActivatedRoute
  ) {}
  idDon: string = '';
  ngOnInit(): void {
    this.idDon = this.ra.snapshot.paramMap.get('iD_Don') || '';
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
  Sua(): void {
    if (this.form.invalid) return;
    console.log(this.idDon);
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

    this.dk.UpdateFull(request, Number(this.idDon)).subscribe(
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
