import { TestBed } from '@angular/core/testing';

import { DondkService } from './dondk.service';

describe('DondkService', () => {
  let service: DondkService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DondkService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
