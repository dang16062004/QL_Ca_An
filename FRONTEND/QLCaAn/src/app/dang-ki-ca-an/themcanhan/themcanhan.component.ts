import { Component, OnInit } from '@angular/core';
import { DondkService } from '../../dondk.service';
import { Router } from '@angular/router';
import { SharedService } from '../../shared.service';
import { FormGroup, Validator, Validators } from '@angular/forms';
import { DonCaNhanRequest } from '../dangki.model';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
@Component({
  selector: 'app-themcanhan',
  standalone: false,
  templateUrl: './themcanhan.component.html',
  styleUrl: './themcanhan.component.css',
})
export class ThemcanhanComponent implements OnInit {
  form = new FormGroup({
    SoLuong: new FormControl('', Validators.required),
    CaAn: new FormControl('', Validators.required),
    LoaiDK: new FormControl('', Validators.required),
  });
  hoVaTen: string = '';

  ngOnInit() {
    const tenDangNhap = localStorage.getItem('HoVaTen');
    if (!tenDangNhap) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['/login']);
      return;
    }
  }
  name: any;
  constructor(
    private service: SharedService,
    private router: Router,
    private dk: DondkService
  ) {
    this.name = localStorage.getItem('HoVaTen');
  }

  dangKyDonCaNhan() {
    const HoVaTen = localStorage.getItem('HoVaTen');
    if (!HoVaTen) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['']);
      return;
    }

    const request: any = {
      HoVaTen: HoVaTen,
      LoaiDK: 'CaNhan',
      CaAn: Number(this.form.value.CaAn),
      SoLuong: Number(this.form.value.SoLuong),
    };

    this.dk.InsertOnly(request).subscribe(
      (res: any) => {
        alert(res);
        this.router.navigate(['/dang-ki-ca-nhan']); // quay lại danh sách sau khi thêm
      },
      (err: any) => {
        console.error('Lỗi đăng ký:', err);
        alert('Không thể thêm đơn');
      }
    );
  }
}
