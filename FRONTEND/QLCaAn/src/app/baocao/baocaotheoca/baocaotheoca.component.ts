import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Input } from '@angular/core';

@Component({
  selector: 'app-baocaotheoca',
  standalone: false,
  templateUrl: './baocaotheoca.component.html',
  styleUrl: './baocaotheoca.component.css',
})
export class BaocaotheocaComponent {
  ngayChon: string = '';
  caChon: number | null = null;

  dsTheoPhong: { [key: string]: any[] } = {};
  objectKeys = Object.keys;

  constructor(private sharedService: SharedService) {}
  ngOnInit() {
    this.timBaoCao();
  }

  timBaoCao() {
    if (!this.ngayChon || !this.caChon) {
      alert('Vui lòng chọn đầy đủ ngày và ca ăn!');
      return;
    }

    this.sharedService.layBCTheoCa(this.ngayChon, this.caChon).subscribe(
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
    return items.reduce((sum, x) => sum + x.SoLuong, 0);
  }

  getTongThanhTien(items: any[]): number {
    return items.reduce((sum, x) => sum + x.ThanhTien, 0);
  }

  getTongSoLuongTatCa(): number {
    return Object.values(this.dsTheoPhong)
      .flat()
      .reduce((sum, item: any) => sum + item.SoLuong, 0);
  }
}
