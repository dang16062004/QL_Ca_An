import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DSTaiKhoanComponent } from './dstai-khoan.component';

describe('DSTaiKhoanComponent', () => {
  let component: DSTaiKhoanComponent;
  let fixture: ComponentFixture<DSTaiKhoanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DSTaiKhoanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DSTaiKhoanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
