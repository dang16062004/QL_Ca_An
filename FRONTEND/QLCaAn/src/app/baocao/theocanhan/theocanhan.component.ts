import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';

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
  }

  loadBCCN() {
    this.sharedService.layBCcanhan(this.id, this.phongban).subscribe(
      (data) => {
        this.dsdkList = data;
        console.log('Data:', this.dsdkList);
      },
      (error) => {
        console.error('Lỗi khi load dữ liệu thống kê cá nhân:', error);
      }
    );
  }

  getTong(ca: 'Ca1' | 'Ca2' | 'Ca3'): number {
    return this.dsdkList.reduce((sum, row) => sum + (row[ca] || 0), 0);
  }
}
