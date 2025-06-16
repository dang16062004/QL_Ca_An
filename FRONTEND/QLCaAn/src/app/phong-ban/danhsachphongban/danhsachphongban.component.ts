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
  constructor(private service: SharedService) {}
  ngOnInit(): void {
    this.LoadDsPhongBan();
  }
  LoadDsPhongBan(){
    this.service.layDSPhongBan().subscribe(data => {
      this.DSPhongBan = data;
    });
  };
}
