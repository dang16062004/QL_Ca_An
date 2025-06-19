import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaocaotheocaComponent } from './baocaotheoca.component';

describe('BaocaotheocaComponent', () => {
  let component: BaocaotheocaComponent;
  let fixture: ComponentFixture<BaocaotheocaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BaocaotheocaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaocaotheocaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
