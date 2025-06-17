import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThemSuaNhanVienComponent } from './them-sua-nhan-vien.component';

describe('ThemSuaNhanVienComponent', () => {
  let component: ThemSuaNhanVienComponent;
  let fixture: ComponentFixture<ThemSuaNhanVienComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ThemSuaNhanVienComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThemSuaNhanVienComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
