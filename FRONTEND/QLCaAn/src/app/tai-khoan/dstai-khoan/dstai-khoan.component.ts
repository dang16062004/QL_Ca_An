import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-dstai-khoan',
  standalone: false,
  templateUrl: './dstai-khoan.component.html',
  styleUrl: './dstai-khoan.component.css'
})
export class DSTaiKhoanComponent {
  DStaiKhoan : any =[];
  constructor(private service: SharedService) { }
  
  ngOnInit():void{
    this.loadDanhsachTaiKhoan();
  };
  loadDanhsachTaiKhoan()
  {
    this.service.layDSTaiKhoan().subscribe(data =>
    {
      this.DStaiKhoan =data;
    }
    )
  }


}
