import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AdminGuard } from './admin.guard';
import { SharedService } from '../shared.service';

describe('AdminGuard (class)', () => {
  let guard: AdminGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AdminGuard,
        // mock SharedService & Router nếu cần
        { provide: SharedService, useValue: { getRoles: () => ['Admin'] } },
        { provide: Router, useValue: { parseUrl: () => '/' } },
      ],
    });
    guard = TestBed.inject(AdminGuard); // ✔ lấy instance
  });

  it('should allow Admin', () => {
    const result = guard.canActivate(); // gọi method, không phải gọi class
    expect(result).toBeTrue();
  });
});
