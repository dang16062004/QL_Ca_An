import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-baocaotheothang',
  standalone: false,
  templateUrl: './baocaotheothang.component.html',
  styleUrl: './baocaotheothang.component.css',
})
export class BaocaotheothangComponent {
  ngayChon: string = '';

  dsTheoPhong: { [key: string]: any[] } = {};
  objectKeys = Object.keys;

  constructor(private sharedService: SharedService) {}
  ngOnInit() {}

  timBaoCao() {
    if (!this.ngayChon) {
      alert('Vui lòng chọn đầy đủ ngày và ca ăn!');
      return;
    }
    this.sharedService.layBCTheoThang(this.ngayChon).subscribe(
      (data) => {
        this.groupTheoPhong(data);
      },
      (error) => {
        console.error('Lỗi truy vấn:', error);
      }
    );
  }

  groupTheoPhong(data: any[]) {
    this.dsTheoPhong = {};
    for (let item of data) {
      const tenPhong = item.TenPhong?.trim();
      if (!this.dsTheoPhong[tenPhong]) {
        this.dsTheoPhong[tenPhong] = [];
      }
      this.dsTheoPhong[tenPhong].push(item);
    }
  }

  getTongSoLuong(items: any[]): number {
    return items.reduce((sum, x) => sum + x.TongSoLuong, 0);
  }

  getTongThanhTien(items: any[]): number {
    return items.reduce((sum, x) => sum + x.ThanhTien, 0);
  }
  getTongTheoPhong(phong: string, ca: 'Ca1' | 'Ca2' | 'Ca3'): number {
    return this.dsTheoPhong[phong].reduce(
      (sum, item) => sum + (item[ca] || 0),
      0
    );
  }

  // Object.values(this.dsTheoPhong): lấy tất cả các mảng nhân viên trong các phòng.

  // .flat(): gộp tất cả thành 1 mảng duy nhất.

  // .reduce(...): cộng dồn theo từng giá trị Ca1, Ca2, Ca3.

  getTongSoLuongTatCa(): number {
    return Object.values(this.dsTheoPhong)
      .flat()
      .reduce((sum, item: any) => sum + item.TongSoLuong, 0);
  }
}
