import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-danhsachphongban',
  standalone: false,
  templateUrl: './danhsachphongban.component.html',
  styleUrl: './danhsachphongban.component.css'
})
export class DanhsachphongbanComponent {
  DSPhongBan: any = [];
  PBanSelected: any;
  dangThemSua: boolean = false;
  tieuDe: any;

  constructor(private service: SharedService) {}
  ngOnInit(): void {
    this.LoadDsPhongBan();
  }
  LoadDsPhongBan(){
    this.service.layDSPhongBan().subscribe(data => {
      this.DSPhongBan = data;
    });
  };
  // PBan1:khai báo 1 biến để nhận dữ liệu từ danh sách phòng ban
  chitietPhongBan(PBan1:any) {
    this.PBanSelected = PBan1;
    
    this.dangThemSua = true; // Hiển thị form thêm sửa
    this.tieuDe = "Sửa Phòng Ban"; // Cập nhật tiêu đề
  };
  dongForm() {
    this.dangThemSua = false;
    this.LoadDsPhongBan() // Ẩn form thêm sửa
    };
    moFormThem()
    {
      this.PBanSelected = {
        ID_Phong: '', // Khởi tạo ID_Phong với chuỗi rỗng
        TenPhong: '' // Khởi tạo TenPhong với chuỗi rỗng
      }; // Đặt lại biến PBanSelected
      this.dangThemSua = true; // Hiển thị form thêm
      this.tieuDe = "Thêm Phòng Ban"; // Cập nhật tiêu đề
      this.LoadDsPhongBan()
    }
    xoaPhongBancu(ma:string)
    {
     if(confirm("Bạn có muốn xóa phòng ban này không?"))  // Xác nhận trước khi xóa
     {
      this.service.xoaPhongBan(ma).subscribe(res => {
        alert(res.toString());
        this.LoadDsPhongBan();
        
      });
     }
    }

}