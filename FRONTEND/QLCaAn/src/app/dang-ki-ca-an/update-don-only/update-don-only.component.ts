import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';
import {
  FormControl,
  FormGroup,
  FormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

import { DondkService } from '../../dondk.service';

@Component({
  selector: 'app-update-don-only',
  standalone: false,
  templateUrl: './update-don-only.component.html',
  styleUrl: './update-don-only.component.css',
})
export class UpdateDonOnlyComponent implements OnInit {
  donCaNhan = new FormGroup({
    CaAn: new FormControl('', Validators.required),
    SoLuong: new FormControl('', Validators.required),
    LoaiDK: new FormControl('', Validators.required),
  });
  thongTinDon: any;
  idDon: string = '';
  name: any;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: SharedService,

    private dk: DondkService
  ) {}

  ngOnInit(): void {
    this.idDon = this.route.snapshot.paramMap.get('iD_Don') || '';
    this.loadThongTinDon(this.idDon);
  }
  loadThongTinDon(ID: any) {
    this.dk.LayChiTietDonTheoID(ID).subscribe((data) => {
      this.thongTinDon = data;
    });
  }

  capNhatDon() {}
}
