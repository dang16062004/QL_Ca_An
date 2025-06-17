import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DSNhanVienComponent } from './dsnhan-vien.component';

describe('DSNhanVienComponent', () => {
  let component: DSNhanVienComponent;
  let fixture: ComponentFixture<DSNhanVienComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DSNhanVienComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DSNhanVienComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
