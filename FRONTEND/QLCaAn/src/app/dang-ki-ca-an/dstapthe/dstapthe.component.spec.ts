import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DstaptheComponent } from './dstapthe.component';

describe('DstaptheComponent', () => {
  let component: DstaptheComponent;
  let fixture: ComponentFixture<DstaptheComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DstaptheComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DstaptheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
