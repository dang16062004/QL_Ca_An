import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';
@Component({
  selector: 'app-them-suaphongban',
  standalone: false,
  templateUrl: './them-suaphongban.component.html',
  styleUrl: './them-suaphongban.component.css'
})
export class ThemSuaphongbanComponent  {
  ID_Phong:any;
  TenPhong:any;
  constructor(private service: SharedService){};
  ngOnInit():void{
    
  }
  themPhongBanmoi() {
    var val ={
      ID_Phong:this.ID_Phong,
      TenPhong:this.TenPhong  
    };
    this.service.themPhongBan(val).subscribe(res => {
      alert(res.toString());
    });
  }
}
