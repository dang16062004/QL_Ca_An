import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';
import { Input } from '@angular/core';



@Component({
  selector: 'app-them-suaphongban',
  standalone: false,
  templateUrl: './them-suaphongban.component.html',
  styleUrl: './them-suaphongban.component.css'
})
export class ThemSuaphongbanComponent  {
  ID_Phong:any;
  TenPhong:any;
  @Input() PBangSelected1: any;
  @Input() checked:any//dùng để xuất hiện lại chữ "Sửa Phòng Ban" khi nhấn sửa và "Thêm phòng ban "khi nhấn thêm mới

  constructor(private service: SharedService){};
  ngOnInit():void{
    this.TenPhong=this.PBangSelected1.TenPhong;
    this.ID_Phong=this.PBangSelected1.ID_Phong;
  };
 
  themPhongBanmoi() {
    var val ={
      ID_Phong:this.ID_Phong,
      TenPhong:this.TenPhong  
    };
   // this.checked=true;
    this.service.themPhongBan(val).subscribe(res => {
      alert(res.toString());
    });
  };
  suaPhongBancu()
  {
    var val ={
      ID_Phong:this.ID_Phong,
      TenPhong:this.TenPhong
    };
   // this.checked=false;
    this.service.suaPhongBan(val).subscribe(res =>
    {
      alert(res.toString());
     
    });
  };
}

