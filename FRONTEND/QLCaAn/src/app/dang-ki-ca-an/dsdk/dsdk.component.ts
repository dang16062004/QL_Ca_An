import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
import { DondkService } from '../../dondk.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dsdk',
  standalone: false,
  templateUrl: './dsdk.component.html',
  styleUrl: './dsdk.component.css',
})
export class DSDKComponent {
  dsdkList: any[] = [];

  constructor(private dk: DondkService, private router: Router) {
    this.loadDSDK();
  }
  id = localStorage.getItem('ID_NhanVien');
  ngOnInit() {
    if (!this.id) {
      alert('Bạn chưa đăng nhập!');
      this.router.navigate(['']);
      return;
    } else {
      this.loadDSDK();
    }
  }
  loadDSDK() {
    this.dk.layDSDK().subscribe(
      (data) => {
        this.dsdkList = data;
      },
      (error) => {
        console.error('Error fetching DSDK data:', error);
      }
    );
  }
  xoaDKCA(ma: any) {
    debugger;
    if (confirm('Bạn có chắc muốn xóa đơn đăng ký này không?')) {
      this.dk.xoaDK(ma).subscribe({
        next: (msg) => {
          alert(msg);
          alert('Xóa thành công!');
          this.loadDSDK();
        },
        error: (err) => {
          console.error(err.toString());
          console.error('❌ Lỗi xóa đơn đăng ký:', err);
          alert('Không tìm thấy đơn đăng ký với ID: ' + ma);
        },
      });
    }
  }

  XacNhan(ma: any) {
    this.dk.KhoaDon(ma).subscribe({
      next: (msg) => {
        alert(msg);
        alert('Đã khóa đơn');
        this.loadDSDK();
      },
      error: (err) => {
        console.log(err.toString());
        alert('Lỗi: Bạn không phải là người đăng kí ca này');
      },
    });
  }
}
