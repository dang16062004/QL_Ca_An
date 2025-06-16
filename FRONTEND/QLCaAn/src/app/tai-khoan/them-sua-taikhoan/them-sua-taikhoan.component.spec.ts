import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThemSuaTaikhoanComponent } from './them-sua-taikhoan.component';

describe('ThemSuaTaikhoanComponent', () => {
  let component: ThemSuaTaikhoanComponent;
  let fixture: ComponentFixture<ThemSuaTaikhoanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ThemSuaTaikhoanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThemSuaTaikhoanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
