import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-dstai-khoan',
  standalone: false,
  templateUrl: './dstai-khoan.component.html',
  styleUrl: './dstai-khoan.component.css'
})
export class DSTaiKhoanComponent {
  DStaiKhoan : any =[];
  constructor(private service: SharedService) { }
  
  ngOnInit():void{
    this.loadDanhsachTaiKhoan();
  };

  // Biến để lưu thông tin tài khoản được chọn
  TKHOAN: any;
  dangThemSua: boolean = false; // Biến để xác định xem có đang thêm hay sửa tài khoản
  // Hàm để nạp danh sách tài khoản
  loadDanhsachTaiKhoan()
  {
    this.service.layDSTaiKhoan().subscribe(data =>
    {
      this.DStaiKhoan =data;
    });
  };
  chitietTaiKhoan(TKhoan:any)
  {
    this.TKHOAN = TKhoan;
    this.dangThemSua = true; // Hiển thị form thêm sửa
  }
  dongForm(){
    this.dangThemSua = false; // Ẩn form thêm sửa
    this.loadDanhsachTaiKhoan(); // Nạp lại danh sách tài khoản
  }
moFormThem()
{
  this.TKHOAN = {
    ID_TaiKhoan:'',
    MatKhau:'',
    TenDangNhap: '',
    NgayTao: new Date() // Ngày tạo được tạo tự động
  }; // Khởi tạo đối tượng tài khoản mới
  this.dangThemSua = true; // Hiển thị form thêm
}

}
