import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../shared.service';
import { DondkService } from '../../dondk.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-chi-tiet-don',
  standalone: false,
  templateUrl: './chi-tiet-don.component.html',
  styleUrl: './chi-tiet-don.component.css',
})
export class ChiTietDonComponent implements OnInit {
  constructor(
    private routerAC: ActivatedRoute,
    private service: SharedService,
    private dk: DondkService,
    private route: Router
  ) {}
  thongTinDon: any;
  ngOnInit() {
    const id = this.routerAC.snapshot.paramMap.get('id')!;
    this.dk.LayChiTietDonTheoID(+id).subscribe(
      (res) => {
        this.thongTinDon = res;
      },
      (err) => {
        console.error('Lỗi tải chi tiết:', err);
      }
    );
  }
  BackToDS() {
    if (this.thongTinDon.LoaiDK.toUpperCase() === 'CANHAN') {
      this.route.navigate(['/dang-ki-ca-nhan']);
    }
    if (this.thongTinDon.LoaiDK.toUpperCase() === 'TAPTHE') {
      this.route.navigate(['/dsTapThe']);
    }
  }
}
