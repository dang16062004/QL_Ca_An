import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-chi-tiet-don',
  standalone: false,
  templateUrl: './chi-tiet-don.component.html',
  styleUrl: './chi-tiet-don.component.css',
})
export class ChiTietDonComponent implements OnInit {
  thongTin: any;

  constructor(private route: ActivatedRoute, private service: SharedService) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.service.layChiTietDon(id).subscribe(
      (res) => {
        this.thongTin = res;
      },
      (err) => {
        console.error('Lỗi tải chi tiết:', err);
      }
    );
  }
}
