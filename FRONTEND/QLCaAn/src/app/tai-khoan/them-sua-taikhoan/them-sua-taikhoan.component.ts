import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-them-sua-taikhoan',
  standalone: false,
  templateUrl: './them-sua-taikhoan.component.html',
  styleUrl: './them-sua-taikhoan.component.css'
})
export class ThemSuaTaikhoanComponent {
   ID_TaiKhoan: any;
  TenDangNhap: any;
  MatKhau: any;
  constructor(private service: SharedService) { }
  ngOnInit(): void {}
 
  //Ngaytao: any; ngày tạo được tạo tự động
  themTaiKhoanMoi()
  {
    var val={
      ID_TaiKhoan:this.ID_TaiKhoan,
      TenDangNhap: this.TenDangNhap,
      MatKhau: this.MatKhau         
    };
    this.service.themtaiKhoan(val).subscribe(res => {
      alert(res.toString());
    });

  }


}

