import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-dstapthe',
  standalone: false,
  templateUrl: './dstapthe.component.html',
  styleUrl: './dstapthe.component.css',
})
export class DstaptheComponent {
  dsdkList: any[] = [];

  constructor(private sharedService: SharedService) {
    this.loadDSDK();
  }

  loadDSDK() {
    this.sharedService.layDSDK().subscribe(
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
      this.sharedService.xoaDonDK(ma).subscribe({
        next: (res) => {
          alert('Đã xóa thành công');
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
}
