import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaocaotheothangComponent } from './baocaotheothang.component';

describe('BaocaotheothangComponent', () => {
  let component: BaocaotheothangComponent;
  let fixture: ComponentFixture<BaocaotheothangComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BaocaotheothangComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaocaotheothangComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
