import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-dstai-khoan',
  standalone: false,
  templateUrl: './dstai-khoan.component.html',
  styleUrl: './dstai-khoan.component.css',
})
export class DSTaiKhoanComponent {
  DStaiKhoan: any = [];
  constructor(private service: SharedService) {}

  ngOnInit(): void {
    this.loadDanhsachTaiKhoan();
  }

  // Biến để lưu thông tin tài khoản được chọn
  // TKHOAN: any;
  // dangThemSua: boolean = false; // Biến để xác định xem có đang thêm hay sửa tài khoản

  // checked2: boolean = false; // dùng cái này truyền sang them-suataikhoan để sửa lại đúng chức năng khi ấn vào thêm mới thì xuất hiện bảng thêm mới cũng như nút "Thêm  mới", tương tự như vậy với sửa

  loadDanhsachTaiKhoan() {
    // Hàm để nạp danh sách tài khoả
    this.service.layDSTaiKhoan().subscribe((data) => {
      this.DStaiKhoan = data;
    });
  }
  // chitietTaiKhoan(TKhoan: any) {
  //   this.TKHOAN = TKhoan;
  //   this.dangThemSua = true; // Hiển thị form thêm sửa
  //   this.checked2 = true;
  // }
  // dongForm() {
  //   this.dangThemSua = false; // Ẩn form thêm sửa
  //   this.loadDanhsachTaiKhoan(); // Nạp lại danh sách tài khoản
  //}
  // moFormThem() {
  //   this.TKHOAN = {
  //     ID_TaiKhoan: '',
  //     Role: '',
  //     NgayTao: new Date(), // Ngày tạo được tạo tự động
  //   }; // Khởi tạo đối tượng tài khoản mới
  //   this.checked2 = false;
  //   this.dangThemSua = true; // Hiển thị form thêm
  // }
  // xoaTaiKhoan(ma: string) {
  //   if (!confirm('Bạn có muốn xóa tài khoản này không?')) return;

  //   this.service.xoaTaiKhoan(ma).subscribe({
  //     next: (res) => {
  //       alert(res.message);
  //       if (res.success) {
  //         this.loadDanhsachTaiKhoan();
  //       }
  //     },
  //     error: (err) => {
  //       console.error(err);
  //       alert('Lỗi khi xóa tài khoản. Vui lòng thử lại sau.');
  //     },
  //   });
  // }
}
