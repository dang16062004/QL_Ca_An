import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhongBanComponent } from './phong-ban.component';

describe('PhongBanComponent', () => {
  let component: PhongBanComponent;
  let fixture: ComponentFixture<PhongBanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PhongBanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhongBanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
