import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
import { DondkService } from '../../dondk.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dstapthe',
  standalone: false,
  templateUrl: './dstapthe.component.html',
  styleUrl: './dstapthe.component.css',
})
export class DstaptheComponent {
  dsdkList: any[] = [];
  id = localStorage.getItem('ID_NhanVien');
  constructor(private dk: DondkService, private router: Router) {}

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
        alert('Lỗi');
      },
    });
  }
}
