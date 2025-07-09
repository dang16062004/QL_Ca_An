// import { Component } from '@angular/core';
// import { SharedService } from '../shared.service';
// import { Router } from '@angular/router';
// import { LoginRequest } from '../models/login-request.models';
// import { FormBuilder, FormGroup, Validators } from '@angular/forms';

// @Component({
//   selector: 'app-login',
//   standalone: false,
//   templateUrl: './login.component.html',
//   styleUrls: ['./login.component.css'], // ✅ sửa: `styleUrl` → `styleUrls`
// })
// export class LoginComponent {
//   loginForm: FormGroup;

//   constructor(
//     private fb: FormBuilder,
//     private service: SharedService,
//     private router: Router
//   ) {
//     // ✅ sửa: thêm `this.` để gán giá trị cho biến class
//     this.loginForm = this.fb.group({
//       TenDangNhap: ['', Validators.required],
//       MatKhau: ['', Validators.required],
//     });
//   }

//   onSubmit(): void {
//     if (this.loginForm.invalid) return;

//     const loginData: LoginRequest = this.loginForm.value as LoginRequest;

//     this.service.login(loginData).subscribe({
//       next: (res) => {
//         this.service.saveToken(res.token);
//         // ✅ sửa: đúng tên thuộc tính (phân biệt hoa thường) căn cứ vào dạng Json trả về
//         localStorage.setItem('Role', res.Role);
//         localStorage.setItem('ID_NhanVien', res.ID_NhanVien);
//         localStorage.setItem('ID_TaiKhoan', res.ID_TaiKhoan);
//         localStorage.setItem('HoVaTen', res.HoVaTen);

//         // ✅ Thêm console log để kiểm tra
//         console.log('Đã lưu token:', res.token);
//         console.log('Đã lưu Role:', res.Role);
//         console.log('ID_NhanVien:', res.ID_NhanVien);
//         console.log('ID_TaiKhoan:', res.ID_TaiKhoan);
//         console.log('HoVaTen:', res.HoVaTen);
//         this.router.navigate(['/phongban']);
//       },
//       // ✅ sửa: sai cú pháp `error(err)=>` → đúng là `error: (err) =>`
//       error: (err) => {
//         alert(
//           'Đăng nhập thất bại. Vui lòng kiểm tra lại tên đăng nhập hoặc mật khẩu.'
//         );
//         console.error(err);
//       },
//     });
//   }
// }
import { Component, Output } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { LoginRequest } from '../models/login-request.models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private service: SharedService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      TenDangNhap: ['', Validators.required],
      MatKhau: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    const loginData: LoginRequest = this.loginForm.value as LoginRequest;

    this.service.login(loginData).subscribe({
      next: (res) => {
        this.service.saveToken(res.token);

        // ✅ Lưu đầy đủ thông tin từ phản hồi server
        localStorage.setItem('Role', res.Role);
        localStorage.setItem('ListRole', JSON.stringify(res.ListRole)); // lưu mảng role
        localStorage.setItem('ID_NhanVien', res.ID_NhanVien.toString());
        localStorage.setItem('DSTaiKhoan', JSON.stringify(res.DSTaiKhoan)); // lưu mảng tài khoản
        localStorage.setItem('HoVaTen', res.HoVaTen);

        // ✅ Ghi log kiểm tra
        console.log('Đã lưu token:', res.token);
        console.log('Role:', res.Role);
        console.log('ListRole:', res.ListRole);
        console.log('DSTaiKhoan:', res.DSTaiKhoan);
        console.log('ID_NhanVien:', res.ID_NhanVien);
        console.log('HoVaTen:', res.HoVaTen);

        this.router.navigate(['/phongban']);
      },
      error: (err) => {
        alert(
          'Đăng nhập thất bại. Vui lòng kiểm tra lại tên đăng nhập hoặc mật khẩu.'
        );
        console.error(err);
      },
    });
  }
}
