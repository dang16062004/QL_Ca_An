import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dangnhap',
  standalone: false,
  templateUrl: './dangnhap.component.html',
  styleUrl: './dangnhap.component.css',
})
export class DangnhapComponent {
  constructor(private service: SharedService, private router: Router) {}

  // Dữ liệu nhập từ form
  tendangNhap: string = '';
  matKhau: string = '';

  // Biến lưu tài khoản
  taiKhoan: any = {
    TenDangNhap: '',
    MatKhau: '',
  };

  // Kết quả từ API
  table: any[] = [];
  anFormDangNhap: boolean = false;
  ngOnInit() {}

  // Hàm xử lý đăng nhập
  dangNhapTaiKhoan() {
    // Lấy thông tin từ form
    this.taiKhoan.TenDangNhap = this.tendangNhap;
    this.taiKhoan.MatKhau = this.matKhau;

    // Gọi API
    this.service.dangNhap(this.taiKhoan).subscribe({
      next: (data) => {
        console.log('Dữ liệu trả về:', data);
        this.table = data;

        // Kiểm tra nếu trả về là mảng và có Column1 == 1
        if (
          Array.isArray(this.table) &&
          this.table.length > 0 &&
          this.table[0].Column1 == 1
        ) {
          alert('Đăng nhập thành công');

          // Lưu thông tin vào localStorage
          localStorage.setItem('tenDangNhap', this.table[0].TenDangNhap);

          // Chuyển hướng
          this.router.navigate(['/phongban']);

          this.anFormDangNhap = true;
        } else {
          alert('Đăng nhập thất bại');
        }
      },
      error: (err) => {
        console.error('Lỗi đăng nhập:', err);
        alert('Lỗi hệ thống hoặc sai tên đăng nhập/mật khẩu');
      },
    });
  }
}
// import { Component } from '@angular/core';
// import { SharedService } from '../../shared.service';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-dangnhap',
//   standalone: false,
//   templateUrl: './dangnhap.component.html',
//   styleUrl: './dangnhap.component.css',
// })
// export class DangnhapComponent {
//   constructor(private service: SharedService, private router: Router) {}

//   tendangNhap: string = '';
//   matKhau: string = '';

//   taiKhoan: any = {
//     TenDangNhap: '',
//     MatKhau: '',
//   };

//   table: any[] = [];
//   anFormDangNhap: boolean = false;

//   ngOnInit() {}

//   dangNhapTaiKhoan() {
//     this.taiKhoan.TenDangNhap = this.tendangNhap;
//     this.taiKhoan.MatKhau = this.matKhau;

//     this.service.dangNhap(this.taiKhoan).subscribe({
//       next: (data) => {
//         console.log('Dữ liệu trả về:', data);
//         console.log('🧾 Phản hồi từ API:', data); // ← thêm dòng này
//         this.table = data;

//         if (
//           Array.isArray(this.table) &&
//           this.table.length > 0 &&
//           this.table[0].Column1 == 1
//         ) {
//           alert('Đăng nhập thành công');

//           // ✅ Ghi đúng Tên Đăng Nhập vào localStorage
//           const tenDangNhap = this.table[0]?.TenDangNhap?.trim();
//           if (tenDangNhap) {
//             localStorage.setItem('tenDangNhap', tenDangNhap);
//           } else {
//             alert('❌ Không lấy được tên đăng nhập từ phản hồi');
//           }

//           this.router.navigate(['/phongban']);
//           this.anFormDangNhap = true;
//         } else {
//           alert('Đăng nhập thất bại');
//         }
//       },
//       error: (err) => {
//         console.error('Lỗi đăng nhập:', err);
//         alert('Lỗi hệ thống hoặc sai tên đăng nhập/mật khẩu');
//       },
//     });
//   }
// }
