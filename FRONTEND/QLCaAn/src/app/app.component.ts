import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css',
})
export class AppComponent {
  constructor(private router: Router) {}
  title = 'QLCaAn';
  name: any;
  ngOnInit() {
    this.name = localStorage.getItem('HoVaTen');
  }
  logout(): void {
    localStorage.clear();
    //location.reload(); // hoặc dùng this.router.navigate(['/login']);
    this.router.navigate(['/']);
  }
}
