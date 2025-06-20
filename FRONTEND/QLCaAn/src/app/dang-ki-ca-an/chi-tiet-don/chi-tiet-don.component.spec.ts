import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChiTietDonComponent } from './chi-tiet-don.component';

describe('ChiTietDonComponent', () => {
  let component: ChiTietDonComponent;
  let fixture: ComponentFixture<ChiTietDonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChiTietDonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChiTietDonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
