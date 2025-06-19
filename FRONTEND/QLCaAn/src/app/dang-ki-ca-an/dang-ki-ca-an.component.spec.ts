import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DangKiCaAnComponent } from './dang-ki-ca-an.component';

describe('DangKiCaAnComponent', () => {
  let component: DangKiCaAnComponent;
  let fixture: ComponentFixture<DangKiCaAnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DangKiCaAnComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DangKiCaAnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
