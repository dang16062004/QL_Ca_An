import { Component } from '@angular/core';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-dsnhan-vien',
  standalone: false,
  templateUrl: './dsnhan-vien.component.html',
  styleUrls: ['./dsnhan-vien.component.css'], // ✅ sửa đúng từ 'styleUrl'
})
export class DSNhanVienComponent {
  DSNhanVien: any = [];
  NVienSelected: any;
  dangThemSua: boolean = false;
  tieuDe: string = '';
  isEdit: boolean = false;

  constructor(private service: SharedService) {}

  ngOnInit(): void {
    this.LoadDsNhanVien();
  }

  LoadDsNhanVien() {
    this.service.layDSNhanVien().subscribe((data) => {
      this.DSNhanVien = data;
    });
  }

  dongForm() {
    this.dangThemSua = false;
    this.LoadDsNhanVien();
  }

  moFormThem() {
    this.NVienSelected = {
      ID_NhanVien: '',
      HoVaTen: '',
      NamSinh: '',
      QDK: '',
      PhanQuyen: '',
      TenDangNhap: '',
      ID_Phong: '',
      TenPhong: '',
    };
    this.dangThemSua = true;
    this.isEdit = false;
    this.tieuDe = 'Thêm Nhân Viên';
  }

  moFormSua(nv: any) {
    this.NVienSelected = { ...nv };
    this.isEdit = true;
    this.tieuDe = 'Sửa Nhân Viên';
    this.dangThemSua = true;
  }
}
