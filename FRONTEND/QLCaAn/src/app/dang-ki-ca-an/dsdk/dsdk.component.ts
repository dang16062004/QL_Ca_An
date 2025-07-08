import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
import { DondkService } from '../../dondk.service';

@Component({
  selector: 'app-dsdk',
  standalone: false,
  templateUrl: './dsdk.component.html',
  styleUrl: './dsdk.component.css',
})
export class DSDKComponent {
  dsdkList: any[] = [];

  constructor(private dk: DondkService) {
    this.loadDSDK();
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
