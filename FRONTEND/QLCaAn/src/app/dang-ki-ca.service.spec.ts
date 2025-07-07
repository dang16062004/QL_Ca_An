import { TestBed } from '@angular/core/testing';

import { DangKiCaService } from './dang-ki-ca.service';

describe('DangKiCaService', () => {
  let service: DangKiCaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DangKiCaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
