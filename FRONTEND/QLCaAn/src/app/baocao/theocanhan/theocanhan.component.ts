import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Input } from '@angular/core';

@Component({
  selector: 'app-theocanhan',
  standalone: false,
  templateUrl: './theocanhan.component.html',
  styleUrl: './theocanhan.component.css',
})
export class TheocanhanComponent {
  constructor(private sharedService: SharedService) {}

  id: string = '';
  phongban: string = '';
  dsdkList: any[] = [];

  ngOnInit() {
    this.loadBCCN();
    this.loadDSPhongBan();
  }

  DSPhongBan: any[] = []; // hoặc khai báo kiểu rõ ràng nếu có interface
  loadBCCN() {
    this.sharedService.layBCcanhan(this.id, this.phongban).subscribe((data) => {
      if (data && data.length > 0) {
        this.dsdkList = data;
        console.log('Data:', this.dsdkList);
      } else {
        alert('Không có dữ liệu thống kê cá nhân cho ID và phòng ban đã chọn.');
      }
    });
  }

  getTong(ca: 'Ca1' | 'Ca2' | 'Ca3'): number {
    return this.dsdkList.reduce((sum, row) => sum + (row[ca] || 0), 0);
  }
  loadDSPhongBan() {
    this.sharedService.layDSPhongBan().subscribe(
      (data) => {
        this.DSPhongBan = data;
      },
      (error) => {
        console.error('Lỗi khi load danh sách phòng ban:', error);
      }
    );
  }
}
