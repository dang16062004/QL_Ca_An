import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { SharedService } from '../shared.service';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {
  constructor(private auth: SharedService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    const hasAdmin = this.auth.getRoles().includes('Admin');

    if (!hasAdmin) {
      alert('Bạn không có quyền truy cập là Admin!');
      return this.router.parseUrl('/nhanvien');
    }
    return true;
  }
}
